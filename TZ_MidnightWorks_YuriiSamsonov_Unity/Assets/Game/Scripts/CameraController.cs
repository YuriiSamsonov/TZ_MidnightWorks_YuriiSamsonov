using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] 
        private GameObject mainCamera;
        
        [field: SerializeField] 
        private GameObject aimCamera;
        
        [field: SerializeField] 
        private GameObject crosshair;

        private bool _isAiming;

        public void OnAimButton(InputAction.CallbackContext context)
        {
            _isAiming = !_isAiming;
            
            if (_isAiming)
            {
                mainCamera.SetActive(false);
                aimCamera.SetActive(true);
                crosshair.SetActive(enabled);
            }
            else
            {
                mainCamera.SetActive(true);
                aimCamera.SetActive(false);
                crosshair.SetActive(false);
            }
        }
    }
}