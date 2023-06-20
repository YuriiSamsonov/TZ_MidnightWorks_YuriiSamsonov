using UnityEngine;

namespace Game.Scripts
{
    public class RagdollPlayer : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        private Collider[] _colliders;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();

            for (int i = 0; i < _rigidbodies.Length; i++)
                _rigidbodies[i].isKinematic = true;

            for (int i = 0; i < _colliders.Length; i++)
                _colliders[i].enabled = false;
        }
    }
}