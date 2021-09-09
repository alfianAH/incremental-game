using Audio;
using UnityEngine;
using UnityEngine.UI;

public class ResourceControl : MonoBehaviour
{
    public Text resourceDescription,
        resourceUpgradeCost,
        resourceUnlockCost;
    public Button resourceButton;
    public Image resourceImage;

    private ResourceConfig config;

    private int level = 1;

    public bool IsUnlocked { get; private set; }

    private void Start()
    {
        // Add listener to resource button
        resourceButton.onClick.AddListener(() =>
        {
            // If isUnlocked, upgrade level
            if (IsUnlocked)
                UpgradeLevel();
            else // Else, unlock the resource
                UnlockResource();
        });
    }
    
    /// <summary>
    /// Set resource configuration
    /// </summary>
    /// <param name="configuration">Resource configuration</param>
    public void SetConfig(ResourceConfig configuration)
    {
        config = configuration;

        resourceDescription.text = $"{config.name} Lv. {level}\n+{GetOutput():0}";
        resourceUnlockCost.text = $"Unlock Cost\n{config.unlockCost}";
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        
        SetUnlocked(config.unlockCost == 0.0f);
    }

    /// <summary>
    /// Get resource's output
    /// </summary>
    /// <returns>Configuration's output times by level</returns>
    public double GetOutput()
    {
        return config.output * level;
    }
    
    /// <summary>
    /// Get resource's upgrade cost
    /// </summary>
    /// <returns>Configuration's upgrade cost times level</returns>
    public double GetUpgradeCost()
    {
        return config.upgradeCost * level;
    }
    
    /// <summary>
    /// Get resource's unlock cost
    /// </summary>
    /// <returns>Configuration's unlock cost</returns>
    public double GetUnlockCost()
    {
        return config.unlockCost;
    }
    
    /// <summary>
    /// Upgrade resource's level
    /// </summary>
    private void UpgradeLevel()
    {
        // Check upgrade cost
        double upgradeCost = GetUpgradeCost();
        
        if (GameManager.Instance.TotalGold < upgradeCost)
        {
            // Play resources error audio
            AudioManager.Instance.Play(ListSound.ResourcesError);
            return;
        }
        
        // Play resource level up audio
        AudioManager.Instance.Play(ListSound.ResourcesLevelUp);
        
        // Decrease the gold 
        GameManager.Instance.AddGold(-upgradeCost);
        level++; // Increase resource's level
        
        // Update resource's UI
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        resourceDescription.text = $"{config.name} Lv. {level}\n+{GetOutput():0}";
    }
    
    /// <summary>
    /// Unlock resource
    /// </summary>
    private void UnlockResource()
    {
        // Check unlock cost
        double unlockCost = GetUnlockCost();
        
        if(GameManager.Instance.TotalGold < unlockCost)
        {
            // Play resources error audio
            AudioManager.Instance.Play(ListSound.ResourcesError);
            return;
        }
        
        // Play resource unlocked audio
        AudioManager.Instance.Play(ListSound.ResourcesUnlocked);
        
        SetUnlocked(true); // Set is unlock to true
        // Show next level of resource
        GameManager.Instance.ShowNextResource();
        // Show achievement
        AchievementController.Instance.UnlockAchievement(AchievementType.UnlockResource, config.name);
    }
    
    /// <summary>
    /// Set resource's unlocked status
    /// </summary>
    /// <param name="unlocked">Value of isUnlocked</param>
    private void SetUnlocked(bool unlocked)
    {
        IsUnlocked = unlocked;
        // Set resource's color 
        resourceImage.color = IsUnlocked ? Color.white : Color.grey;
        
        resourceUnlockCost.gameObject.SetActive(!unlocked);
        resourceUpgradeCost.gameObject.SetActive(unlocked);
    }
}
