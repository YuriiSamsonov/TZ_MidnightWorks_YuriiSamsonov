using UnityEngine;

namespace Game.Scripts
{
    public class UIInventory : MonoBehaviour
    {
        public Transform itemsParent;
        private Inventory _inventory;
        private UISlot[] _slots;
        
        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
            _inventory.Init(UpdateUI);
        
            _slots = itemsParent.GetComponentsInChildren<UISlot>();
        }
        
        private void UpdateUI()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (i < _inventory.InventoryItems.Count)
                    if (_inventory.InventoryItems[i] != null)
                        _slots[i].AddUIItem(_inventory.InventoryItems[i]);
            }
        }
    }
}