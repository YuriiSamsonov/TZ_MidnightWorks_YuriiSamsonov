using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class UISlot : MonoBehaviour
    {
        [field: SerializeField] 
        private Image image;

        [field: SerializeField] 
        private Text amountOfBullets;

        private Inventory _inventory;
        private Sprite _sprite;

        public void AddUIItem(GunMono item)
        {
            image.sprite = item.Data.Sprite;
            amountOfBullets.text = item.CurrentBulletCount.ToString();
            image.gameObject.SetActive(true);
        }
    }
}