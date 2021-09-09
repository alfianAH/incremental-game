using UnityEngine;
using UnityEngine.UI;

public class TapText : MonoBehaviour
{
    [SerializeField] private float spawnTimeOrigin = 0.5f;
    public Text tapText;

    private float spawnTime;

    private void OnEnable()
    {
        // Set spawn time to its origin
        spawnTime = spawnTimeOrigin;
    }

    // Update is called once per frame
    private void Update()
    {
        // Reduce spawn time
        spawnTime -= Time.unscaledDeltaTime;
        
        // If spawn time is up, deactivate tap text
        if (spawnTime <= 0f)
        {
            gameObject.SetActive(false);
        }
        else // Else, ...
        {
            // Decrease tap text's alpha to 0
            tapText.CrossFadeAlpha(0f, 0.5f, false);
            // If tap text's alpha is zero, deactivate tap text
            if (tapText.color.a <= 0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
