using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class PlayerMono : MonoBehaviour
    {
        [field: SerializeField] 
        private int maxHp = 20;
        
        [field: SerializeField] 
        private UIManager uiManager;

        [field: SerializeField] 
        private HealthBar healthBar;
        
        public Transform gunHolder;

        public GameObject inputManagerParent;

        private GunMono _gun;
        
        private Inventory _inventory;

        private GunMono _gunHold;

        private GameStateManager _gameStateManager;
        
        private float _fireInput;
        
        private int _amountOfBullets;
        private int _bulletsWaisted;
        private int _currentHp;

        private bool _isButtonPressed;
        private bool _isGunInHands;

        private void Awake()
        {
            _gameStateManager = FindObjectOfType<GameStateManager>();
        }

        private void Start()
        {
            _currentHp = maxHp;
            _inventory = new Inventory();
            uiManager.UiInventory.SetInventory(_inventory);
            UpdateUIScreen();
            healthBar.UpdateHealthBar(_currentHp, maxHp);
            _isGunInHands = false;
        }

        private void FixedUpdate()
        {
            if (_isGunInHands)
            {
                switch (_fireInput)
                {
                    case 1:
                        _gun.TryToFire(true);
                        UpdateUIScreen();
                        break;
                    case 0:
                        _gun.TryToFire(false);  
                        UpdateUIScreen();
                        break;
                }
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            switch (col.tag)
            {
                case "Mag":
                    var mag = col.gameObject.GetComponent<Mag>();
                    _amountOfBullets += mag.MagSize;
                    Destroy(mag.gameObject);
                    UpdateUIScreen();
                    break;
                case "Gun":
                    _inventory.AddItem(col.gameObject.GetComponent<GunMono>());
                    col.gameObject.SetActive(false);
                    UpdateUIScreen();
                    break;
            }
        }

        public void SetActiveGun(GunMono activeGun)
        {
            _gun = activeGun;
            _isGunInHands = true;
        }

        public void OnReloadButton(InputAction.CallbackContext context)
        {
            if (_gun != null)
            {
                _gun.Reload(_amountOfBullets, out var bulletsToCharge);

                _bulletsWaisted = bulletsToCharge;
            
                if (_amountOfBullets >= _bulletsWaisted)
                    _amountOfBullets -= _bulletsWaisted;
                else
                    _amountOfBullets = 0;

                UpdateUIScreen();
            }
        }

        private void UpdateUIScreen()
        {
            uiManager.UpdatePlayerBulletsScreen(_amountOfBullets);
            _inventory.RefreshInventory();
        }

        public void OnHit(int damage)
        {
            _currentHp -= damage;
            healthBar.UpdateHealthBar(_currentHp, maxHp);

            if (_currentHp <= 0)
                _gameStateManager.LoseGame();
        }

        public void TakeGunFromSlot(int slot)
        {
            if (_inventory.InventoryItems.Count >= slot)
                _inventory.TakeGun(slot - 1, gunHolder);
        }

        public void ReceiveInput(float fireInput)
        {
            _fireInput = fireInput;
        }
    }
}