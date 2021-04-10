using UnityEngine;
using UnityEngine.UI;

public class TapText : MonoBehaviour
{
    [SerializeField] private float spawnTimeOrigin = 0.5f;
    public Text tapText;

    private float spawnTime;

    private void OnEnable()
    {
        spawnTime = spawnTimeOrigin;
    }

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
