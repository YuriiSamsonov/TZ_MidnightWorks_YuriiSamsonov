using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Data/CreateEnemyData", fileName = "new EnemyData")]
    public class EnemyData : ScriptableObject
    {
        [field: SerializeField] 
        private float scale = 1f;

        public float Scale => scale;

        [field: SerializeField] 
        private int damage = 1;

        public int Damage => damage;
        
        [field: SerializeField] 
        private int maxHealth = 20;

        public int MaxHealth => maxHealth;
        
        [field: SerializeField] 
        private float timeBetweenAttacks = 1f;

        public float TimeBetweenAttacks => timeBetweenAttacks;

        [field: SerializeField] 
        private float attackRange = 1.5f;

        public float AttackRange => attackRange;
        
        [field: SerializeField] 
        private float sightRange = 12f;

        public float SightRange => sightRange;
    }
}