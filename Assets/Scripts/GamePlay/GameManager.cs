using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class GameManager : SingletonBaseClass<GameManager>
    {
        [Range(0, 1)] public float autoCollectPercentage = 0.1f;
        public ResourceConfig[] resourceConfigs;
        public Sprite[] resourceSprites;
        public TapText tapTextPrefab;
    
        public Transform coinIcon;
        public Transform resourceParent;
        public ResourceControl resourcePrefab;

        [SerializeField] private Text goldInfo,
            autoCollectInfo;

        private AchievementController achievementController;
        private List<ResourceControl> activeResources = new List<ResourceControl>();
        private List<TapText> tapTextPool = new List<TapText>();
        private float collectSecond;

        // Start is called before the first frame update
        private void Start()
        {
            achievementController = AchievementController.Instance;
            AddAllResources();
            goldInfo.text = $"Gold: {UserDataManager.Progress.gold:0}";
        }

        // Update is called once per frame
        private void Update()
        {
            collectSecond += Time.unscaledDeltaTime;

            if (collectSecond >= 1f)
            {
                CollectPerSecond();
                collectSecond = 0f;
            }
        
            CheckResourceCost();
        
            // Check gold milestone achievement
            achievementController.GoldMilestoneAchievement(
                AchievementType.GoldMilestone, UserDataManager.Progress.gold);
        
            // Coin's animation
            coinIcon.transform.localScale = 
                Vector3.LerpUnclamped(coinIcon.transform.localScale, Vector3.one * 2f, 0.15f);
        
            coinIcon.transform.Rotate(0f, 0f, Time.deltaTime * -100f);
        }
    
        /// <summary>
        /// Add all resources in resource configurations
        /// </summary>
        private void AddAllResources()
        {
            bool showResources = true;
            int index = 0;
        
            foreach (ResourceConfig config in resourceConfigs)
            {
                // Instantiate the resource's object
                GameObject obj = Instantiate(resourcePrefab.gameObject, resourceParent, false);
                ResourceControl resource = obj.GetComponent<ResourceControl>();
            
                // Set resource's configuration
                resource.SetConfig(index, config);
                obj.SetActive(showResources);
            
                // If resource isn't unlocked yet, don't show resources
                if (showResources && !resource.IsUnlocked)
                {
                    showResources = false; 
                }
            
                // Add resource to active resources
                activeResources.Add(resource);
                index++;
            }
        }
    
        /// <summary>
        /// Show next resource in active resources
        /// </summary>
        public void ShowNextResource()
        {
            foreach (ResourceControl resource in activeResources)
            {
                // If the next resource isn't active yet, ...
                if (!resource.gameObject.activeSelf)
                {
                    // Activate it
                    resource.gameObject.SetActive(true);
                    break; // Break the loop
                }
            }
        }
    
        /// <summary>
        /// Set the value of auto collect per second 
        /// </summary>
        private void CollectPerSecond()
        {
            double output = 0;
            foreach (ResourceControl resource in activeResources)
            {
                // Get the output from all unlocked resources
                if(resource.IsUnlocked)
                    output += resource.GetOutput();
            }
        
            output *= autoCollectPercentage;
            // Update auto collect UI
            autoCollectInfo.text = $"Auto Collect: {output:F1} / second";
        
            AddGold(output);
        }
    
        /// <summary>
        /// Add gold
        /// </summary>
        /// <param name="value">Value of gold</param>
        public void AddGold(double value)
        {
            UserDataManager.Progress.gold += value;
            goldInfo.text = $"Gold: {UserDataManager.Progress.gold:0}";
            
            // Save gold
            UserDataManager.Save();
        }
    
        /// <summary>
        /// Collect gold by tap the coin
        /// </summary>
        /// <param name="tapPosition">Tap position in screen</param>
        /// <param name="parent">Tap Area transform</param>
        public void CollectByTap(Vector3 tapPosition, Transform parent)
        {
            double output = 0;

            foreach (ResourceControl resource in activeResources)
            {
                // Get the output from all unlocked resources
                if(resource.IsUnlocked)
                    output += resource.GetOutput();
            }
        
            // Get or create tap text
            TapText tapText = GetOrCrateTapText();
        
            Transform tapTextTransform = tapText.transform;
            // Set tap text object to tap area
            (tapTextTransform).SetParent(parent, false);
            // Set tap text's position
            tapTextTransform.position = tapPosition;
        
            // Set tap text 
            tapText.tapText.text = $"+{output:0}";
            tapText.gameObject.SetActive(true);
            // Shrink the gold's scale (for animation)
            coinIcon.transform.localScale = Vector3.one * 1.75f;
        
            AddGold(output);
            AudioManager.Instance.Play(ListSound.CollectCoin);
        }
    
        /// <summary>
        /// Get existing tap text or create new tap text
        /// </summary>
        /// <returns>Tap Text</returns>
        private TapText GetOrCrateTapText()
        {
            // Find an inactive tap text
            TapText tapText
                = tapTextPool.Find(t => !t.gameObject.activeSelf);
        
            // If tap text can't be found, ...
            if (tapText == null)
            {
                // Create new tap text
                tapText = Instantiate(tapTextPrefab).GetComponent<TapText>();
                tapTextPool.Add(tapText); // Add new one to pool
            }

            return tapText;
        }
    
        /// <summary>
        /// Check resource's cost
        /// </summary>
        private void CheckResourceCost()
        {
            foreach (ResourceControl resource in activeResources)
            {
                bool isBuyable;
            
                // If the resource is unlocked, set isBuyable
                if (resource.IsUnlocked)
                {
                    isBuyable = UserDataManager.Progress.gold >= resource.GetUpgradeCost();
                }
                else // If the resource isn't unlocked yet, set isBuyable
                {
                    isBuyable = UserDataManager.Progress.gold >= resource.GetUnlockCost();
                }
            
                // Set resource's sprite if it's buyable or not
                resource.resourceImage.sprite = resourceSprites[isBuyable ? 1 : 0];
            }
        }
    }

    [System.Serializable]
    public struct ResourceConfig
    {
        public string name;
        public double unlockCost;
        public double upgradeCost;
        public double output;
    }
}