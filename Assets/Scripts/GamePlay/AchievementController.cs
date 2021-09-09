using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class AchievementController : SingletonBaseClass<AchievementController>
    {
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
    
        /// <summary>
        /// Unlock new achievement
        /// </summary>
        /// <param name="type">The achievement type</param>
        /// <param name="value">Configuration name</param>
        public void UnlockAchievement(AchievementType type, string value)
        {
            AchievementData achievement = achievementList.Find(
                a => a.type == type && a.value == value);
        
            // If achievement is not null and not unlocked yet, ...
            if (achievement != null && !achievement.isUnlocked)
            {
                // Set achievement is unlocked to true 
                achievement.isUnlocked = true;
                // Show achievement pop up
                ShowAchievementPopUp(achievement);
            }
        }

        /// <summary>
        /// Gold milestone achievement
        /// </summary>
        /// <param name="type">The achievement type</param>
        /// <param name="goldValue">Current gold value</param>
        public void GoldMilestoneAchievement(AchievementType type, double goldValue)
        {
            // Find achievement in achievement list
            AchievementData achievement = achievementList.Find(
                a => a.type == type && a.goldValue <= goldValue && !a.isUnlocked);

            if (achievement != null && !achievement.isUnlocked)
            {
                // Set achievement is unlocked to true 
                achievement.isUnlocked = true;
                // Show achievement pop up
                ShowAchievementPopUp(achievement);
            }
        }
    
        /// <summary>
        /// Show achievement pop up
        /// </summary>
        /// <param name="achievement">Achievement data</param>
        private void ShowAchievementPopUp(AchievementData achievement)
        {
            // Play audio
            AudioManager.Instance.Play(ListSound.AchievementUnlocked);
        
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
    
        [DrawIf("type", AchievementType.UnlockResource)]
        public string value;

        [DrawIf("type", AchievementType.GoldMilestone)]
        public double goldValue;
    
        public bool isUnlocked;
    }

    public enum AchievementType
    {
        UnlockResource,
        GoldMilestone
    }
}