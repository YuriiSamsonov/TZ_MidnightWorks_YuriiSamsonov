using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class InputManager : MonoBehaviour
    {
        [field: SerializeField]     
        private PlayerMono playerMono; 
        
        [field: SerializeField]     
        private PlayerMovement playerMovement;
    
        [field: SerializeField] 
        private MouseLook mouseLook;
    
        [field: SerializeField] 
        private CameraController cameraController;

        [field: SerializeField] 
        private PlayerAnimationController playerAnimationController;
        
        private PlayerControls _playerControls; 
        private PlayerControls.PlayerActionsActions _playerActions;
    
        private Vector2 _horizontalInput;
        private Vector2 _mouseInput; 
        
        private float _sprintInput; 
        private float _fireInput;
    
        private void Awake()
        {
            _playerControls = new PlayerControls();
            _playerActions = _playerControls.PlayerActions;

            _playerActions.HorizontalMovement.performed += ctx => 
                _horizontalInput = ctx.ReadValue<Vector2>();
            
            _playerActions.MouseX.performed += ctx =>
                _mouseInput.x = ctx.ReadValue<float>();
            
            _playerActions.MouseY.performed += ctx =>
                _mouseInput.y = ctx.ReadValue<float>();

            _playerActions.Sprint.performed += ctx =>
                _sprintInput = ctx.ReadValue<float>();
            
            _playerActions.Fire.performed += ctx =>
                _fireInput = ctx.ReadValue<float>();

            _playerActions.Jump.performed += playerMovement.OnJumpButton;
            _playerActions.Reload.performed += playerMono.OnReloadButton;
            _playerActions.Aim.performed += playerMovement.OnAimButton;
            _playerActions.Aim.performed += cameraController.OnAimButton;
            _playerActions.Slot1.performed += _ => playerMono.TakeGunFromSlot(1);
            _playerActions.Slot2.performed += _ => playerMono.TakeGunFromSlot(2);
            _playerActions.Slot3.performed += _ => playerMono.TakeGunFromSlot(3);
        }
        
        private void Update()
        {
            playerMovement.ReceiveInput(_horizontalInput, _sprintInput, _fireInput);
            playerMono.ReceiveInput(_fireInput);
            playerAnimationController.ReceiveInput(_horizontalInput);
            mouseLook.ReceiveInput(_mouseInput);
        }
        
        private void OnEnable()
        {
            _playerControls.PlayerActions.Enable();
        }
        
        private void OnDisable()
        {
            _playerControls.PlayerActions.Disable();
        }
    }
}