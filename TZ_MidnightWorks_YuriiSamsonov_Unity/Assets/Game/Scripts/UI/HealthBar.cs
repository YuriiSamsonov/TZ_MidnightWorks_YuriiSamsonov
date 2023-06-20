using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class HealthBar : MonoBehaviour
    {
        [field: SerializeField] 
        private Slider slider;
        
        [field: SerializeField] 
        private bool isNeedToRotate;

        private Camera _mainCamera;

        public void UpdateHealthBar(float currentHP, float maxHP)
        {
            slider.value = currentHP / maxHP;
        }
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }
        
        private void Update()
        {
            if (isNeedToRotate)
                transform.rotation = _mainCamera.transform.rotation;
        }
    }
}