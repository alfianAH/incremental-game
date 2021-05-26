using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Load scene by scene index
    /// </summary>
    /// <param name="sceneIndex">Scene Index in Build Settings</param>
    public void LoadScene(int sceneIndex)
    {
        UserDataManager.Save(true);
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }
}
