using UnityEngine;
using UnityEngine.UI;

public class ResourceControl : MonoBehaviour
{
    [SerializeField] private Text resourceDescription,
        resourceUpgradeCost,
        resourceUnlockCost;

    private ResourceConfig config;

    private int level = 1;

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

    public double GetUnlockCost()
    {
        return config.unlockCost;
    }
}
