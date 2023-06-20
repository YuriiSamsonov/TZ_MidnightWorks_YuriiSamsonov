using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class BulletsScreen : MonoBehaviour
    {
        [field: SerializeField] 
        private Text bulletsCountText;

        public void UpdatePlayerBulletsCount(int bulletsCount)
        {
            bulletsCountText.text = "Bullets: " + bulletsCount;
        }
    }
}