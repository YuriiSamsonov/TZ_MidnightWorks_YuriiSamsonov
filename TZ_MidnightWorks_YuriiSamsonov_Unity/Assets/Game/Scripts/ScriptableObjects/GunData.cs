using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Data/CreateGunData", fileName = "new GunData")]
    public class GunData : ScriptableObject
    {
        [field: SerializeField] 
        private float fireRate;

        public float FireRate => fireRate;
        
        [field: SerializeField] 
        private int damage;

        public int Damage => damage;
        
        [field: SerializeField] 
        private bool isAutomatic;

        public bool IsAutomatic => isAutomatic;
        
        [field: SerializeField] 
        private int maxBulletCount;

        public int MaxBulletCount => maxBulletCount;
        
        [field: SerializeField] 
        private Sprite sprite;

        public Sprite Sprite => sprite;
    }
}