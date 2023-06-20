using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.Scripts
{
    public class EnemyMono : MonoBehaviour
    {
        [field: SerializeField] 
        private EnemyData data;

        [field: SerializeField] 
        private HealthBar healthBar;
        
        [field: SerializeField] 
        private LayerMask playerMask;
        
        [field: SerializeField]
        private LayerMask groundMask;
        
        [field: SerializeField] 
        private GameObject mag;
        
        [field: SerializeField] 
        private Transform posToSpawnMag;

        [field: SerializeField] 
        private float walkPointRange;
        
        private bool _playerInSightRange;
        private bool _playerInAttackRange;
        private bool _alreadyAttacked;
        private bool _walkPointSet;
        private bool _isAlive;
        
        private int _currentHealth;

        private readonly float _distanceTolerance = 1f;
        private readonly float _rayDistance = 2f;
        private const float TimeToDestroy = 10f;

        private Vector3 _walkPoint;
        
        private PlayerMono _playerMono;
        private Animator _animator;
        private NavMeshAgent _agent;
        private Collider _collider;
        private EnemyManager _enemyManager;
        
        private static readonly int Damaged = Animator.StringToHash("Damaged");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int Moving = Animator.StringToHash("Moving");
        
        private void Awake()
        {
            _playerMono = FindObjectOfType<PlayerMono>();
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
            _enemyManager = FindObjectOfType<EnemyManager>();
            gameObject.transform.localScale = new Vector3(data.Scale, data.Scale, data.Scale);
        }

        private void Start()
        {
            _currentHealth = data.MaxHealth;
            healthBar.UpdateHealthBar(_currentHealth, data.MaxHealth);
            _animator.SetBool(Death, false);
            _animator.SetBool(Moving, true);
            _isAlive = true;
        }

        private void Update()
        {
            _playerInSightRange = Physics.CheckSphere(transform.position, data.SightRange, playerMask);
            _playerInAttackRange = Physics.CheckSphere(transform.position, data.AttackRange, playerMask);

            if (!_playerInSightRange && !_playerInAttackRange) Patroling();
            if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
            if (_playerInSightRange && _playerInAttackRange) AttackPlayer();
        }
        
        public void OnHit(int damage)
        {
            if (_isAlive)
            {
                _currentHealth -= damage;
            
                healthBar.UpdateHealthBar(_currentHealth, data.MaxHealth);
                if (_currentHealth <= 0)
                    OnZeroHP();
                
                _animator.SetTrigger(Damaged);
            }
        }

        private void Patroling()
        {
            if(!_walkPointSet) FindNextWalkPoint();

            if (_walkPointSet)
                _agent.SetDestination(_walkPoint);

            if (Vector3.Distance(transform.position, _walkPoint) < _distanceTolerance)
                _walkPointSet = false;
        }

        private void FindNextWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);

            _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

            if (Physics.Raycast(_walkPoint, -transform.up, _rayDistance, groundMask))
                _walkPointSet = true;
        }
        
        private void ChasePlayer()
        {
            _agent.SetDestination(_playerMono.transform.position);
        }
        private void AttackPlayer()
        {
            _agent.SetDestination(transform.position);

            if (!_agent.isStopped)
                transform.LookAt(_playerMono.transform);

            if (!_alreadyAttacked && !_agent.isStopped)
            {
                _alreadyAttacked = true;
                _animator.SetBool(Attack, _alreadyAttacked);

                _playerMono.OnHit(data.Damage);

                StartCoroutine(ResetAttackWithDelay());
            }
        }
        
        private IEnumerator ResetAttackWithDelay()
        {
            yield return new WaitForSeconds(data.TimeBetweenAttacks);
            ResetAttack();
        }

        private void ResetAttack()
        {
            _alreadyAttacked = false;
            _animator.SetBool(Attack, _alreadyAttacked);
        }

        private void OnZeroHP()
        {
            _isAlive = false;
            _enemyManager.EnemyDied(this);
            _animator.SetBool(Death, true);
            _animator.SetBool(Moving, false);
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _collider.enabled = false;
            healthBar.gameObject.SetActive(false);
            StartCoroutine(DestroyObjectWithDelay());

            Instantiate(mag, posToSpawnMag.position, quaternion.identity);
        }

        private IEnumerator DestroyObjectWithDelay()
        {
            yield return new WaitForSeconds(TimeToDestroy);
            Destroy(gameObject);
        }
    }
}