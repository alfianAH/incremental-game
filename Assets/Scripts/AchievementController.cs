using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementController : MonoBehaviour
{
    private static AchievementController instance;

    public static AchievementController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<AchievementController>();

            return instance;
        }
    }

    [SerializeField] private Transform popUpTransform;
    [SerializeField] private Text popUpText;
    [SerializeField] private float popUpShowDuration = 3f;
    [SerializeField] private List<AchievementData> achievementList;

    private float popUpShowDurationCounter;
    
    // Update is called once per frame
    private void Update()
    {
        if (popUpShowDurationCounter > 0)
        {
            // Decrease duration when pop up duration is greater than 0
            popUpShowDurationCounter -= Time.unscaledDeltaTime;
            popUpTransform.localScale = Vector3.LerpUnclamped(popUpTransform.localScale, Vector3.one, 0.5f);
        }
        else
        {
            popUpTransform.localScale = Vector2.LerpUnclamped(popUpTransform.localScale, Vector2.right, 0.5f);
        }
    }

    public void UnlockAchievement(AchievementType type, string value)
    {
        AchievementData achievement = achievementList.Find(a => a.type == type && a.value == value);

        if (achievement != null && !achievement.isUnlocked)
        {
            achievement.isUnlocked = true;
            ShowAchievementPopUp(achievement);
        }
    }

    private void ShowAchievementPopUp(AchievementData achievement)
    {
        popUpText.text = achievement.title;
        popUpShowDurationCounter = popUpShowDuration;
        popUpTransform.localScale = Vector2.right;
    }
}

[System.Serializable]
public class AchievementData
{
    public string title;
    public AchievementType type;
    public string value;
    public bool isUnlocked;
}

public enum AchievementType
{
    UnlockResource
}
