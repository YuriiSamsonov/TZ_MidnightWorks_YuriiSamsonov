using System;
using UnityEngine;

namespace Game.Scripts
{
    public class GameStateManager : MonoBehaviour
    {
        [field: SerializeField]
        private ScoreRuntimeData scoreRuntimeData;
        
        public event Event<EventArgs> OnPlayerDiedEvent, OnPlayerWinEvent;
        
        public void WonGame()
        {
            scoreRuntimeData.winCount++;
            OnPlayerWinEvent(EventArgs.Empty);
        }

        public void LoseGame()
        {
            scoreRuntimeData.loseCount++;
            OnPlayerDiedEvent(EventArgs.Empty);
        }
    }
}