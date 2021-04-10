using UnityEngine;
using UnityEngine.UI;

public class TapText : MonoBehaviour
{
    [SerializeField] private float spawnTime = 0.5f;
    public Text tapText;

    // Update is called once per frame
    private void Update()
    {
        spawnTime -= Time.unscaledDeltaTime;
        if (spawnTime <= 0f)
        {
            gameObject.SetActive(false);
        }
        else
        {
            tapText.CrossFadeAlpha(0f, 0.5f, false);
            if (tapText.color.a <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
