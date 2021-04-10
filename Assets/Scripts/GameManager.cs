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
    public Sprite[] resourceSprites;
    public TapText tapTextPrefab;
    
    public Transform coinIcon;
    public Transform resourceParent;
    public ResourceControl resourcePrefab;

    [SerializeField] private Text goldInfo,
        autoCollectInfo;
    
    private List<ResourceControl> activeResources = new List<ResourceControl>();
    private List<TapText> tapTextPool = new List<TapText>();
    private float collectSecond;
    private double totalGold;

    public double TotalGold => totalGold;

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
        
        CheckResourceCost();

        coinIcon.transform.localScale = 
            Vector3.LerpUnclamped(coinIcon.transform.localScale, Vector3.one * 2f, 0.15f);
        
        coinIcon.transform.Rotate(0f, 0f, Time.deltaTime * -100f);
    }

    private void AddAllResources()
    {
        bool showResources = true;
        
        foreach (ResourceConfig config in resourceConfigs)
        {
            GameObject obj = Instantiate(resourcePrefab.gameObject, resourceParent, false);
            ResourceControl resource = obj.GetComponent<ResourceControl>();
            
            resource.SetConfig(config);
            obj.SetActive(showResources);

            if (showResources && !resource.IsUnlocked)
            {
                showResources = false;
            }
            
            activeResources.Add(resource);
        }
    }

    public void ShowNextResource()
    {
        foreach (ResourceControl resource in activeResources)
        {
            if (!resource.gameObject.activeSelf)
            {
                resource.gameObject.SetActive(true);
                break;
            }
        }
    }

    private void CollectPerSecond()
    {
        double output = 0;
        foreach (ResourceControl resource in activeResources)
        {
            if(resource.IsUnlocked)
                output += resource.GetOutput();
        }

        output *= autoCollectPercentage;
        autoCollectInfo.text = $"Auto Collect: {output:F1} / second";

        AddGold(output);
    }

    public void AddGold(double value)
    {
        totalGold += value;
        goldInfo.text = $"Gold: {totalGold:0}";
    }

    public void CollectByTap(Vector3 tapPosition, Transform parent)
    {
        double output = 0;

        foreach (ResourceControl resource in activeResources)
        {
            if(resource.IsUnlocked)
                output += resource.GetOutput();
        }

        TapText tapText = GetOrCrateTapText();
        tapText.transform.SetParent(parent, false);
        tapText.transform.position = tapPosition;

        tapText.tapText.text = $"+{output:0}";
        tapText.gameObject.SetActive(true);
        coinIcon.transform.localScale = Vector3.one * 1.75f;
        
        AddGold(output);
    }

    private TapText GetOrCrateTapText()
    {
        TapText tapText
            = tapTextPool.Find(t => !t.gameObject.activeSelf);
        
        if (tapText == null)
        {
            tapText = Instantiate((tapTextPrefab)).GetComponent<TapText>();
            tapTextPool.Add(tapText);
        }

        return tapText;
    }

    private void CheckResourceCost()
    {
        foreach (ResourceControl resource in activeResources)
        {
            bool isBuyable;

            if (resource.IsUnlocked)
            {
                isBuyable = totalGold >= resource.GetUpgradeCost();
            }
            else
            {
                isBuyable = totalGold >= resource.GetUnlockCost();
            }

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
