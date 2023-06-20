using System.Collections;
using UnityEngine;

namespace Game.Scripts
{
    public class MouseLook : MonoBehaviour
    {
        [field: SerializeField] 
        private float sensitivityX = 10f;
        
        [field: SerializeField] 
        private float sensitivityY = 10f;

        [field: SerializeField] 
        private float minCameraRotationX = -17f;
        
        [field: SerializeField] 
        private float maxCameraRotationX = 55f;
        
        [field: SerializeField] 
        private Transform followTarget;

        private float _mouseX;
        private float _mouseY;
        private float _xRotation;
        private float _yRotation;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            StartCoroutine(UnlockMouseWithDelay(0.5f));
            enabled = false;
        }

        private void Update()
        {
            _yRotation += _mouseX;
            _xRotation -= _mouseY;
            _xRotation = Mathf.Clamp(_xRotation, minCameraRotationX, maxCameraRotationX);

            followTarget.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        }

        public void ReceiveInput(Vector2 mouseInput)
        {
            _mouseX = mouseInput.x * Time.deltaTime * sensitivityX;
            _mouseY = mouseInput.y * Time.deltaTime * sensitivityY;
        }

        private IEnumerator UnlockMouseWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            enabled = true;
        }
    }
}