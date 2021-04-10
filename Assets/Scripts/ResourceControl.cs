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
        resourceButton.onClick.AddListener(() =>
        {
            if (IsUnlocked)
                UpgradeLevel();
            else
                UnlockResource();
        });
    }

    public void SetConfig(ResourceConfig configuration)
    {
        config = configuration;

        resourceDescription.text = $"{config.name} Lv. {level}\n+{GetOutput():0}";
        resourceUnlockCost.text = $"Unlock Cost\n{config.unlockCost}";
        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
    }

    public double GetOutput()
    {
        return config.output * level;
    }

    public double GetUpgradeCost()
    {
        return config.upgradeCost * level;
    }

    public double GetUnlockCost()
    {
        return config.unlockCost;
    }

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();

        if (GameManager.Instance.TotalGold < upgradeCost) return;

        GameManager.Instance.AddGold(-upgradeCost);
        level++;

        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        resourceDescription.text = $"{config.name} Lv. {level}\n+{GetOutput():0}";
    }

    public void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        
        if(GameManager.Instance.TotalGold < unlockCost) return;

        SetUnlocked(true);
        GameManager.Instance.ShowNextResource();
        
        AchievementController.Instance.UnlockAchievement(AchievementType.UnlockResource, config.name);
    }

    public void SetUnlocked(bool unlocked)
    {
        IsUnlocked = unlocked;
        resourceImage.color = IsUnlocked ? Color.white : Color.grey;
        resourceUnlockCost.gameObject.SetActive(!unlocked);
        resourceUpgradeCost.gameObject.SetActive(unlocked);
    }
}
