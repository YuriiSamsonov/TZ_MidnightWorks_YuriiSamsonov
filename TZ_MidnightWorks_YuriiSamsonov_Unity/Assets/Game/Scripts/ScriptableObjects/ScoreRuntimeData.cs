using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "Data/CreateScoreRuntimeData", fileName = "newScoreRuntimeData")]
    public class ScoreRuntimeData : ScriptableObject
    {
        [field: SerializeField] 
        public int winCount;
        
        [field: SerializeField] 
        public int loseCount;
    }
}