using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class Inventory
    {
        private readonly List<GunMono> _inventoryItems;
        public List<GunMono> InventoryItems => _inventoryItems;
        
        private GunMono _item;

        private Action _onItemChangedCallback;
        
        public void Init(Action onItemChangedCallback)
        {
            _onItemChangedCallback = onItemChangedCallback;
        }
        
        public Inventory()
        {
            _inventoryItems = new List<GunMono>();
        }

        public void TakeGun(int slot, Transform gunHolder)
        {
            if (_inventoryItems[slot] != null)
            {
                for (int i = 0; i < _inventoryItems.Count; i++)
                    _inventoryItems[i].gameObject.SetActive(false);
                
                _inventoryItems[slot].TakeToHands(gunHolder);
            }
        }

        public void AddItem(GunMono item)
        {
            if (!HasItem(item))
                _inventoryItems.Add(item);
        }
        
        private bool HasItem(GunMono item)
        {
            for (int i = 0; i < _inventoryItems.Count; i++)
            {
                if (_inventoryItems[i] != null && _inventoryItems[i].Data.MaxBulletCount == item.Data.MaxBulletCount)
                    return true;
            }
            return false;
        }

        public void RefreshInventory()
        {
            _onItemChangedCallback();
        }
    }
}