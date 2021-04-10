using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();

            return instance;
        }
    }

    [Range(0, 1)] public float autoCollectPercentage = 0.1f;
    public ResourceConfig[] resourceConfigs;

    public Transform resourceParent;
    public ResourceControl resourcePrefab;

    [SerializeField] private Text goldInfo,
        autoCollectInfo;
    
    private List<ResourceControl> activeResources = new List<ResourceControl>();
    private float collectSecond;
    private double totalGold;

    // Start is called before the first frame update
    private void Start()
    {
        AddAllResources();
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
    }

    private void AddAllResources()
    {
        foreach (ResourceConfig config in resourceConfigs)
        {
            GameObject obj = Instantiate(resourcePrefab.gameObject, resourceParent, false);
            ResourceControl resource = obj.GetComponent<ResourceControl>();
            
            resource.SetConfig(config);
            activeResources.Add(resource);
        }
    }

    private void CollectPerSecond()
    {
        double output = 0;
        foreach (ResourceControl resource in activeResources)
        {
            output += resource.GetOutput();
        }

        output *= autoCollectPercentage;
        autoCollectInfo.text = $"Auto Collect: {output:F1} / second";

        AddGold(output);
    }

    private void AddGold(double value)
    {
        totalGold += value;
        goldInfo.text = $"Gold: {totalGold:0}";
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
