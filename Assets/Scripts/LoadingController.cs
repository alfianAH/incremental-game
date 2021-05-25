using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        UserDataManager.Load();
        SceneManager.LoadScene(1);
    }
}
