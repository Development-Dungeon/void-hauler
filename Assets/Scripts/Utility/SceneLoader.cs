using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>Load a scene by name (must be in File → Build Profiles / Build Settings).</summary>
        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadLevel1()
        {
            SceneManager.LoadScene("Level 1");
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }

        public void LoadUpgradeShop()
        {
            SceneManager.LoadScene("Upgrade Shop");
        }
        
    }
}