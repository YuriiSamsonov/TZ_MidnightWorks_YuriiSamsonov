using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        public void OnPauseMenuStartButton()
        {
            SceneManager.LoadScene(1);
            Time.timeScale = 1f;
        }

        public void OnPauseMenuExitButton()
        {
            Application.Quit();
        }
    }
}