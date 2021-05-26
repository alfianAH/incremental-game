using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private Button localButton;
    [SerializeField] private Button cloudButton;

    // Start is called before the first frame update
    private void Start()
    {
        // Disable button's interactable to prevent spam clicks
        localButton.onClick.AddListener(() =>
        {
            SetButtonInteractable(false);
            UserDataManager.LoadFromLocal();
            SceneManager.LoadScene(1);
        });

        cloudButton.onClick.AddListener(() =>
        {
            SetButtonInteractable(false);
            StartCoroutine(UserDataManager.LoadFromCloud(() =>
            {
                SceneManager.LoadScene(1);
            }));
        });
    }
    
    /// <summary>
    /// Set button's interactable
    /// </summary>
    /// <param name="interactable">Is button interactable?</param>
    private void SetButtonInteractable(bool interactable)
    {
        localButton.interactable = interactable;
        cloudButton.interactable = interactable;
    }
}