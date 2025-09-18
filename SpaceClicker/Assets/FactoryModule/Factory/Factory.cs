using System;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
public class Factory : MonoBehaviour
{
    public string saveID = Guid.NewGuid().ToString();
    public Image fillbar;
    public string currencyName;
    private float productionCnt;
    private int productionInterval = 1;
    public float speed;
    public int manualProductionOutput;
    public int manualProductionOutputMultiplier;
    public int automaticProductionOutput;
    public int automaticProductionOutputMultiplier;
    public int amount;
    public int maxAmount;
    public int lifetimeAmount;
    public int workers; // can be factories that produce, ships that transport... be creative
    public int autoCollect; // 1 == true
    public bool autoCollectState;
    public string collectRewardName;
    public int reward;
    public int rewardPrice;
    public bool collectOnlyIfAmountIsFull;
    public Toggle autoCollectToggle;
    public TextMeshProUGUI storageStatsLabel;
    public TextMeshProUGUI collectLabel;
    public TextMeshProUGUI speedLabel;
    public Texture2D outputResourceIcon;
    public Button collectBtn;
    public UnityEvent onCollect;
    public SpecialObject[] specialObjects;
    [Header("In World Objects")]
    public bool keepPreviousObject;
    public int currentSpecialObjectIndex;
    public bool hasEraAttribute; // if true, will update the era taskUI on collect
    public int amountToNextEra;
    public int eraIndex;// assigned in inspector, used for taskUI only
    public int reqAttribute;
    public GameObject collectInfoPanel;
    public GameObject clickClickClick;
    

    private void Start()
    {
        SaveLoad.Instance.OnSaved.AddListener(Save);
        SaveLoad.Instance.OnLoaded.AddListener(Load);
        Load(); // Always load when starting the game
        UpdateLabels();
        if (hasEraAttribute) 
        {
            var attribute = DNA.Instance.evolutionRequirements[eraIndex].requirementsAttributes[reqAttribute];
            attribute.SetIcon(outputResourceIcon);
            attribute.UpdateProgress(0); // so that UI is Initizlied
            attribute.taskUI.gameObject.SetActive(true);
        }
        if (clickClickClick != null && !PlayerPrefs.HasKey("clickClickClick")) 
        {
            clickClickClick.SetActive(true);
        }
    }
    private void Save() 
    {
        FactorySaveData saveData = new FactorySaveData();
        saveData.saveID = saveID;
        saveData.amount = amount;
        saveData.lifetimeAmount = lifetimeAmount;
        saveData.workers = workers;
        saveData.autoCollect = autoCollectState ? 1 : 0;
        saveData.autoCollectState = autoCollectState;
        saveData.currentSpecialObjectIndex = currentSpecialObjectIndex;
        saveData.speed = speed;
        saveData.maxAmount = maxAmount;
        saveData.ProductionOutputMultiplier = manualProductionOutputMultiplier;
        saveData.AutomaticProductionOutputMultiplier = automaticProductionOutputMultiplier;
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText($"{SaveLoad.Instance.directoryPath}/{saveID}.json", json);
    }
    public void Load() 
    {
        var filePath = $"{SaveLoad.Instance.directoryPath}/{saveID}.json";
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            FactorySaveData saveData = JsonUtility.FromJson<FactorySaveData>(json);
            amount = saveData.amount;
            lifetimeAmount = saveData.lifetimeAmount;
            workers = saveData.workers;
            autoCollectState = saveData.autoCollect == 1;
            autoCollectToggle.SetIsOnWithoutNotify(autoCollectState);
            currentSpecialObjectIndex = saveData.currentSpecialObjectIndex;
            speed = saveData.speed;
            maxAmount = saveData.maxAmount;
            manualProductionOutputMultiplier = saveData.ProductionOutputMultiplier;
            automaticProductionOutputMultiplier = saveData.AutomaticProductionOutputMultiplier;
            UpdateLabels();
            TryUpdateSpecialObjects(lifetimeAmount);
        }
        else
        {
            Debug.LogWarning("No save file found");
        }
    }
    public void ToggleAutoCollect() 
    {
        autoCollectState = !autoCollectState;
        autoCollectToggle.isOn = autoCollectState;
    }
    public void CloseClickClickClick() 
    {
        PlayerPrefs.SetInt("clickClickClick", 1);
        clickClickClick.SetActive(false);
    }
    public void Collect() 
    {

        if (collectInfoPanel != null && !PlayerPrefs.HasKey("collectInfoPanel")) 
        {
            collectInfoPanel.SetActive(true);
            PlayerPrefs.SetInt("collectInfoPanel", 1);
        }
        collectLabel.text = $"+{Reward()} {collectRewardName}";
        collectLabel.gameObject.SetActive(true);
        onCollect?.Invoke();
        lifetimeAmount += manualProductionOutput;
        if(hasEraAttribute) DNA.Instance.evolutionRequirements[eraIndex].requirementsAttributes[reqAttribute].UpdateProgress(lifetimeAmount);
        amount = 0;
        collectBtn.interactable = false;
        TryUpdateSpecialObjects(lifetimeAmount);
        UpdateLabels();
        Invoke(nameof(DisableLabel), 2);
    }
    // assigned to onCollect in the inspector
    public void IncreasePopullation() => Population.Instance.IncreaseCurrentPopulation(Reward());
    private int Reward() 
    {
        var result = RoundDownAmountWithDeduction(amount);
        var finalReward = reward * (result.roundedAmount / rewardPrice);
        return finalReward;
    }
    (int roundedAmount, int remainder) RoundDownAmountWithDeduction(int amount)
    {
        int remainder = amount % 10;
        int roundedAmount = amount - remainder;
        return (roundedAmount, remainder);
    }
    private void DisableLabel() => collectLabel.gameObject.SetActive(false);

    public void AddAmountManualy() 
    {
        IncreaseAmount(manualProductionOutput * manualProductionOutputMultiplier);
    }
    private void Update()
    {
        if (workers > 0) 
        {
            collectBtn.interactable = CollectBtnStatus;
            if (amount >= maxAmount) 
            {
                amount = maxAmount;
                fillbar.fillAmount = 1;
                if (autoCollectState)
                {
                    Collect();
                }
                return;
            }
            if (productionCnt < productionInterval)
            {
                productionCnt += Time.deltaTime * speed;
                fillbar.fillAmount = Mathf.Clamp01(productionCnt / (float)productionInterval);
            }
            else 
            {
                productionCnt = 0;  
                IncreaseAmount(automaticProductionOutput * automaticProductionOutputMultiplier);
            }
        }
    }
    private bool CollectBtnStatus => (amount >= rewardPrice && !collectOnlyIfAmountIsFull) || (amount == maxAmount && collectOnlyIfAmountIsFull);
    private void IncreaseAmount(int amountToAdd) 
    {
        amount += amountToAdd;
        if (amount >= maxAmount)
        {
            fillbar.fillAmount = 1;
            amount = maxAmount;
            if (autoCollectState) 
            {
                Collect();
            }
        }
        UpdateLabels();
    }
    public void DecreaseAmount(int amount) 
    {
        this.amount -= amount;
        UpdateLabels();
    }
    private void TryUpdateSpecialObjects(int progress) 
    {
        if (specialObjects.Length == 0) return;
        if (currentSpecialObjectIndex >= specialObjects.Length) return;

        var currentSpecialObject = specialObjects[currentSpecialObjectIndex];
        if (progress >= currentSpecialObject.cost)
        {
            currentSpecialObject.@object.SetActive(true);

            if (currentSpecialObjectIndex - 1 >= 0)
            {
                var previousSpecialObject = specialObjects[currentSpecialObjectIndex - 1];
                previousSpecialObject.@object.SetActive(keepPreviousObject);
                currentSpecialObjectIndex++;
                return;
            }
            currentSpecialObjectIndex++;
        }
    }
    public void UpdateLabels() 
    {
        storageStatsLabel.text = $"+{manualProductionOutput*manualProductionOutputMultiplier}    {amount} / {maxAmount}";
        speedLabel.text = $"production speed {speed:0.0}x{automaticProductionOutputMultiplier}";
    }
}
[Serializable]
public class SpecialObject 
{
    public string name;
    public int cost;
    public GameObject @object;
}
public enum ModifierType 
{
    AutoCollect,
    Capacity,
    ProductionSpeed,
    ProductionOutputMultiplier,
    AutomaticProductionOutputMultiplier
}
public class FactorySaveData 
{
    public string saveID;
    public int amount;
    public int lifetimeAmount;
    public int workers;
    public int autoCollect; // 1 == true
    public bool autoCollectState;
    public int currentSpecialObjectIndex;
    public float speed;
    public int maxAmount;
    public int ProductionOutputMultiplier;
    public int AutomaticProductionOutputMultiplier;
}