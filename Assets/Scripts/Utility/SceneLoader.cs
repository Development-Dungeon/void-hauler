using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Hook UI buttons here. Build order must be: Main Menu (0), then Level 1 (1), etc.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>Load a scene by name (must be in File → Build Profiles / Build Settings).</summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>Optional: wire Play with no arguments — loads build index 1 (Level 1 when menu is index 0).</summary>
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }
}