using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class Score : MonoBehaviour
    {
        [field: SerializeField] 
        private ScoreRuntimeData scoreRuntimeData;
        
        [field: SerializeField] 
        private Text winText;
        
        [field: SerializeField] 
        private Text loseText;

        private void OnEnable()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            winText.text = "Win Count: " + scoreRuntimeData.winCount;
            loseText.text = "Lose Count: " + scoreRuntimeData.loseCount;
        }
    }
}