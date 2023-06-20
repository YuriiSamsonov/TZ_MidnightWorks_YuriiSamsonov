using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    public class PlayerMovement : MonoBehaviour
    {
        #region Movement

        [field: SerializeField, Header("Movement")] 
        private Transform orientation;
        
        [field: SerializeField] 
        private Transform followTarget;
        
        [field: SerializeField] 
        private float moveSpeed = 4f;
        
        [field: SerializeField] 
        private float dragMultiplier = 3f;
        
        [field: SerializeField] 
        private float sprintMultiplier = 4f;
        
        [field: SerializeField] 
        private float groundSpeedMultiplier = 10f;

        #endregion

        #region Jump

        [field: SerializeField, Header("Jump")] 
        private float jumpForce = 7;
        
        [field: SerializeField] 
        private float airSpeedMultiplier = .5f;
        
        private bool _isReadyToJump = true;

        #endregion

        #region GroundCheck

        [field: SerializeField, Header("GroundCheck")] 
        private Transform rayStart;
        
        [field: SerializeField] 
        private LayerMask ground;
        
        [field: SerializeField] 
        private float rayLength = .5f;

        private bool _isGrounded;

        #endregion

        private Rigidbody _rigidbody;

        private Vector2 _horizontalInput;
        private Vector3 _moveDirection;
        private Vector3 _vel;

        private bool _isAiming;
        public bool IsJumping { get; private set; }
        public bool IsSprinting() => _sprintInput == 1;

        private float _sprintInput;
        private float _fireInput;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;
        }
        
        private void FixedUpdate()
        {
            Movement();
        }

        private void Update()
        {
            SpeedControl();
            
            if (_horizontalInput.magnitude > 0f || _isAiming || _fireInput == 1)
                PassRotation();

            IsJumping = !_isGrounded;
            
            _rigidbody.drag = _isGrounded ? dragMultiplier : 0f;
        }
        
        private void Movement()
        {
            _moveDirection = orientation.forward * _horizontalInput.y + orientation.right * _horizontalInput.x;
            
            _isGrounded = Physics.Raycast(rayStart.position, Vector3.down, rayLength, ground);
            
            if (_isGrounded)
            {
                switch (_sprintInput)
                {
                    case 0:
                        _rigidbody.AddForce(_moveDirection.normalized * (moveSpeed * groundSpeedMultiplier), ForceMode.Force);
                        _isReadyToJump = true;
                        break;
                    case 1:
                        _rigidbody.AddForce(_moveDirection.normalized * (moveSpeed * groundSpeedMultiplier * sprintMultiplier), ForceMode.Force);
                        _isReadyToJump = true;
                        break;
                }
            }

            else if (!_isGrounded)
                _rigidbody.AddForce(_moveDirection.normalized * (moveSpeed * airSpeedMultiplier), ForceMode.Force);
        }

        private void PassRotation()
        {
            var targetRotation = followTarget.transform.rotation;
            var yRotation = targetRotation.eulerAngles.y;
            var targetEulerAngles = orientation.transform.eulerAngles;
            targetEulerAngles.y = yRotation;
            orientation.transform.eulerAngles = targetEulerAngles;
        }
        
        private void SpeedControl()
        {
            var velocity = _rigidbody.velocity;
            Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                _rigidbody.velocity = new Vector3(limitedVel.x, _rigidbody.velocity.y, limitedVel.z);
            }
        }

        public void OnJumpButton(InputAction.CallbackContext context)
        {
            if (_isReadyToJump && _isGrounded)
            {
                _isReadyToJump = false;
                
                Jump();
            }
        }

        private void Jump()
        {
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);

            _rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        public void OnAimButton(InputAction.CallbackContext context)
        {
            _isAiming = !_isAiming;
        }
        
        public void ReceiveInput(Vector2 horizontalInput, float sprintInput, float fireInput)
        {
            _horizontalInput = horizontalInput;
            _sprintInput = sprintInput;
            _fireInput = fireInput;
        }
    }
}