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

    private int index;

    private int Level
    {
        set
        {
            // Save set value to the resource's level in progress data
            UserDataManager.Progress.resourcesLevel[index] = value;
            UserDataManager.Save();
        }

        get =>
            // Check if index is in user's data
            // If there is no index, return level 1
            // If there is, return level according to progress data
            !UserDataManager.HasResources(index) 
                ? 1 
                : UserDataManager.Progress.resourcesLevel[index];
    }

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
    /// <param name="index">Resource's index</param>
    /// <param name="configuration">Resource configuration</param>
    public void SetConfig(int index, ResourceConfig configuration)
    {
        this.index = index;
        config = configuration;

        resourceDescription.text = $"{config.name} Lv. {Level}\n+{GetOutput():0}";
        resourceUnlockCost.text = $"Unlock Cost\n{config.unlockCost}";
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        
        SetUnlocked(config.unlockCost == 0.0f || UserDataManager.HasResources(this.index));
    }

    /// <summary>
    /// Get resource's output
    /// </summary>
    /// <returns>Configuration's output times by level</returns>
    public double GetOutput()
    {
        return config.output * Level;
    }
    
    /// <summary>
    /// Get resource's upgrade cost
    /// </summary>
    /// <returns>Configuration's upgrade cost times level</returns>
    public double GetUpgradeCost()
    {
        return config.upgradeCost * Level;
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
        
        if (UserDataManager.Progress.gold < upgradeCost) return;
        
        // Decrease the gold 
        GameManager.Instance.AddGold(-upgradeCost);
        Level++; // Increase resource's level
        
        // Update resource's UI
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        resourceDescription.text = $"{config.name} Lv. {Level}\n+{GetOutput():0}";
    }
    
    /// <summary>
    /// Unlock resource
    /// </summary>
    private void UnlockResource()
    {
        // Check unlock cost
        double unlockCost = GetUnlockCost();
        
        if(UserDataManager.Progress.gold < unlockCost) return;
        
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

        if (unlocked)
        {
            // If new resources is just unlocked and there is not in progress data yet, ...
            if (!UserDataManager.HasResources(index))
            {
                // Add resource level
                UserDataManager.Progress.resourcesLevel.Add(Level);
                UserDataManager.Save();
            }
        }
        
        // Set resource's color 
        resourceImage.color = IsUnlocked ? Color.white : Color.grey;
        
        resourceUnlockCost.gameObject.SetActive(!unlocked);
        resourceUpgradeCost.gameObject.SetActive(unlocked);
    }
}
