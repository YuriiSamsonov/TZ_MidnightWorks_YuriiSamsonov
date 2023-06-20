using UnityEngine;

namespace Game.Scripts
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [field: SerializeField] 
        private Animator animator;
        
        [field: SerializeField] 
        private PlayerMovement playerMovement;

        private Vector2 _horizontalInput;

        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsSprinting = Animator.StringToHash("IsSprinting");
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
        
        private void Update()
        {
            animator.SetBool(IsJumping, playerMovement.IsJumping);
            animator.SetBool(IsSprinting, playerMovement.IsSprinting());
            
            if (_horizontalInput.magnitude > 0)
                animator.SetBool(IsMoving, true);
            else
                animator.SetBool(IsMoving, false);
        }
        
        public void ReceiveInput(Vector2 horizontalInput)
        {
            _horizontalInput = horizontalInput;
        }
    }
}