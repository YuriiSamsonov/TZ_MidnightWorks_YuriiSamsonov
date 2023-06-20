using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class EnemyManager : MonoBehaviour
    {
        private List<EnemyMono> _allEnemies;

        [field: SerializeField] 
        private GameStateManager stateManager;

        private void Start()
        {
            _allEnemies = new List<EnemyMono>(FindObjectsOfType<EnemyMono>());
        }

        public void EnemyDied(EnemyMono enemyMono)
        {
            _allEnemies.Remove(enemyMono);

            if (_allEnemies.Count == 0)
                stateManager.WonGame();
        }
    }
}