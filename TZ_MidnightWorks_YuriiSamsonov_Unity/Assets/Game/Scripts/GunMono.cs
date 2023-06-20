using UnityEngine;

namespace Game.Scripts
{
    public class GunMono : MonoBehaviour
    {
        [field: SerializeField] 
        private GunData data;

        public GunData Data => data;

        [field: SerializeField] 
        private ParticleSystem fireParticle;
        
        [field: SerializeField] 
        private ParticleSystem bulletImpactParticle;

        [field: SerializeField] 
        private LayerMask enemyLayerMask;
        
        [field: SerializeField] 
        private LayerMask groundLayerMask;
        
        [field: SerializeField] 
        private Transform rayStartPos;

        private PlayerMono _playerMono;
        private Camera _mainCamera;
        private Animator _animator;
        private Rigidbody _rigidbody;
        private Collider _collider;

        private RaycastHit _rayHitGround;
        private RaycastHit _rayHitEnemy;
        private Vector3 _impactPointGround;
        private Vector3 _impactPointEnemy;

        private readonly Quaternion _gunInHandsRotation = Quaternion.Euler(0, 180, 0);

        private readonly RaycastHit[] _rayHitsEnemy = new RaycastHit[10];
        private readonly RaycastHit[] _rayHitsGround = new RaycastHit[10];

        private static readonly int Fire = Animator.StringToHash("Fire");

        private float _lastShootTime;
        private int _magSize;
        private int _currentBulletCount;
        public int CurrentBulletCount => _currentBulletCount;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
            _animator = GetComponent<Animator>();
            _mainCamera = FindObjectOfType<Camera>();
            _playerMono = FindObjectOfType<PlayerMono>();
        }
        
        private void Start()
        {
            _magSize = data.MaxBulletCount;
            _currentBulletCount = _magSize;
            _lastShootTime = 0;
        }

        public void TryToFire(bool canShoot)
        {
            if (_currentBulletCount > 0)
            {
                if (canShoot && Time.time > _lastShootTime)
                {
                    FireAction();
                    
                    if(data.IsAutomatic) 
                        _lastShootTime = Time.time + data.FireRate;
                    else
                        _lastShootTime = Mathf.Infinity;
                }
                else if (!canShoot)
                    _lastShootTime = 0;
            }
        }

        public void Reload(int playersBullets, out int bulletsToCharge)
        {
            bulletsToCharge = _magSize - _currentBulletCount;

            if (playersBullets > bulletsToCharge)
                _currentBulletCount = _magSize;
        }

        private void FireAction()
        {
            PlayShootEffect();
            
            _currentBulletCount--;
            
            var startPos = rayStartPos.localToWorldMatrix.GetPosition();
            var dir = _mainCamera.transform.forward;
            float distanceToGround = Mathf.Infinity;
            float distanceToEnemy = Mathf.Infinity;

            int sizeGround = LookForHits(startPos, dir,
                _rayHitsGround,
                groundLayerMask);

            int sizeEnemy = LookForHits(startPos, dir,
                _rayHitsEnemy,
                enemyLayerMask);

            if (sizeGround > 0)
            {
                _rayHitGround = _rayHitsGround[sizeGround - 1];
                _impactPointGround = _rayHitGround.point;
                distanceToGround = Vector3.Distance(startPos, _impactPointGround);
            }
            
            if (sizeEnemy > 0)
            {
                _rayHitEnemy = _rayHitsEnemy[sizeEnemy - 1];
                _impactPointEnemy = _rayHitEnemy.point;
                distanceToEnemy = Vector3.Distance(startPos, _impactPointEnemy);
            }

            if(distanceToEnemy < distanceToGround)
                BulletOnEnemyLayer();
            else if(sizeGround > 0)
                BulletOnGroundLayer();
        }

        private int LookForHits(Vector3 startPos, Vector3 dir, RaycastHit[] rayHits, LayerMask layerMask)
        {
            return Physics.RaycastNonAlloc(startPos, dir, 
                rayHits, 
                Mathf.Infinity, 
                layerMask);
        }

        private void BulletOnGroundLayer()
        {
            Instantiate(bulletImpactParticle, _impactPointGround, FacingGun(_impactPointGround), _rayHitGround.transform);
        }

        private void BulletOnEnemyLayer()
        {
            Instantiate(bulletImpactParticle, _impactPointEnemy, FacingGun(_impactPointEnemy), _rayHitEnemy.transform);
            if (_rayHitEnemy.transform.TryGetComponent<EnemyMono>(out var enemyMono))
                enemyMono.OnHit(data.Damage);
        }
        
        private Quaternion FacingGun(Vector3 posInWorld)
        {
            Vector3 gunPosition = rayStartPos.position;
            Vector3 directionToGun = gunPosition - posInWorld;
            Quaternion rotation = Quaternion.LookRotation(directionToGun);
            
            return rotation;
        }

        public void TakeToHands(Transform gunHolder)
        {
            gameObject.SetActive(true);
            _playerMono.SetActiveGun(this);

            Destroy(_rigidbody);
            _rigidbody = null;

            _collider.enabled = false;

            transform.SetParent(gunHolder);
            transform.localPosition = Vector3.zero;
            transform.localRotation = _gunInHandsRotation;
        }

        private void PlayShootEffect()
        { 
            _animator.SetTrigger(Fire);
            fireParticle.Play();
        }
    }
}