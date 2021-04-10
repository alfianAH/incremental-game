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

    private void Start()
    {
        resourceButton.onClick.AddListener(UpgradeLevel);
    }

    public void SetConfig(ResourceConfig configuration)
    {
        this.config = configuration;

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

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();

        if (GameManager.Instance.TotalGold < upgradeCost) return;

        GameManager.Instance.AddGold(-upgradeCost);
        level++;

        resourceUpgradeCost.text = $"Upgrade Cost\n{GetUpgradeCost()}";
        resourceDescription.text = $"{config.name} Lv. {level}\n+{GetOutput():0}";
    } 
}
