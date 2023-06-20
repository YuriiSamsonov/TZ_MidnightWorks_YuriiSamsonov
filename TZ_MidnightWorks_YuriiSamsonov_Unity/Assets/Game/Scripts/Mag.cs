using System;
using UnityEngine;

namespace Game.Scripts
{
    public class Mag : MonoBehaviour
    {
        [field: SerializeField] 
        private float rotationsPerMinute = 10;
        
        [field: SerializeField] 
        private int magSize = 100;

        public int MagSize => magSize;
        
        private float _rotationsPerTick;
        
        private void Start()
        {
            var rotationsPerSecond = rotationsPerMinute / 60;
            var degreesPerSecond = 360 * rotationsPerSecond;
            _rotationsPerTick = degreesPerSecond / (1 / Time.fixedDeltaTime);
        }

        private void FixedUpdate()
        {
            transform.Rotate(0,_rotationsPerTick,0, Space.Self);;
        }
    }
}