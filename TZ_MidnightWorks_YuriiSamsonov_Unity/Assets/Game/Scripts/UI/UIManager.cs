using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class UIManager : MonoBehaviour
    {
        [field: SerializeField] 
        private UIInventory uiInventory;

        public UIInventory UiInventory => uiInventory;

        [field: SerializeField]
        private PlayerMono playerMono;

        [field: SerializeField, Header("On Pause Interface")] 
        private GameObject exitButton;
        
        [field: SerializeField] 
        private GameObject restartButton;
        
        [field: SerializeField] 
        private GameObject score;
        
        [field: SerializeField, Header("On Play Interface")] 
        private GameObject hpBar;
        
        [field: SerializeField] 
        private GameObject inventory;
        
        [field: SerializeField] 
        private GameObject crosshair;
        
        [field: SerializeField] 
        private BulletsScreen playerBulletsScreen;

        [field: SerializeField, Header("Screens")] 
        private GameObject loseScreen;
        
        [field: SerializeField] 
        private GameObject winScreen;
        
        [field: SerializeField] 
        private GameObject damageScreen;

        private GameStateManager _gameStateManager;

        private bool _isPlayerDead;

        private void Awake()
        {
            _gameStateManager = FindObjectOfType<GameStateManager>();
        }

        private void Start()
        {
            _gameStateManager.OnPlayerDiedEvent += OnPlayerDeadEventHandler;
            _gameStateManager.OnPlayerWinEvent += OnPlayerWinEventHandler;
        }

        public void UpdatePlayerBulletsScreen(int bulletsCount)
        {
            playerBulletsScreen.UpdatePlayerBulletsCount(bulletsCount);
        }

        private void OnPlayerWinEventHandler(EventArgs _)
        {
            OnPause();
            winScreen.SetActive(true);
        }
        
        private void OnPlayerDeadEventHandler(EventArgs _)
        {
            OnPause();
            loseScreen.SetActive(true);
        }

        private void OnPause()
        {
            Time.timeScale = 0f;
            hpBar.SetActive(false);
            playerBulletsScreen.transform.parent.gameObject.SetActive(false);
            inventory.SetActive(false);
            playerMono.inputManagerParent.SetActive(false);
            playerBulletsScreen.gameObject.SetActive(false);
            crosshair.SetActive(false);
            damageScreen.SetActive(false);
            ToggleCursorVisibilityAndAvailability(true);
            restartButton.SetActive(true);
            exitButton.SetActive(true);
            score.SetActive(true);
        }

        private void ToggleCursorVisibilityAndAvailability(bool state)
        {
            Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = state;
        }
    }
}