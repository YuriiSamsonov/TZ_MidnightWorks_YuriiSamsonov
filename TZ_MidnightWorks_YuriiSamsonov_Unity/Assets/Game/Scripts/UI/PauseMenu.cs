using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public void OnPauseMenuRestartButton()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1f;
        }

        public void OnPauseMenuExitButton()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1f;
        }
    }
}