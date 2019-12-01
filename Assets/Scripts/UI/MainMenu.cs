using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        
        public void Play()
        {
            SceneManager.LoadScene("Scenes/GameScene");
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void Settings()
        {
            
        }
    }
}
