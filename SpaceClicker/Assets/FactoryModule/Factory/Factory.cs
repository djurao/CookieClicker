using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    public Image fillbar;
    public string currencyName;
    public float productionCnt;
    public int productionInterval;
    public float speed;
    public int productionOutput;
    public int amount;
    public int lifetimeAmount;
    public int maxAmount;
    public int workers; // can be factories that produce, ships that transport... be creative
    public int autoCollect; // 1 == true
    public bool autoCollectState;
    public Toggle autoCollectToggle;
    public int reward;
    public int rewardPrice;
    public int ProductionOutputMultiplier;
    public bool collectOnlyIfAmountIsFull;
    public TextMeshProUGUI storageStatsLabel;
    public TextMeshProUGUI collectLabel;
    public TextMeshProUGUI speedLabel;

    public string collectRewardName;
    public Button collectBtn;
    public UnityEvent onCollect;
    [Header("In World Objects")]
    public bool keepPreviousObject;
    public int currentSpecialObjectIndex;
    public SpecialObject[] specialObjects;
    public UpgradePrice[] upgradePrices; // assigned in inspector
    private void Start()
    {
        UpdateLabels();
    }
    public void ToggleAutoCollect() 
    {
        autoCollectState = !autoCollectState;
        autoCollectToggle.isOn = autoCollectState;
    }
    public void Collect() 
    {

        collectLabel.text = $"+{Reward()} {collectRewardName}";
        collectLabel.gameObject.SetActive(true);
        onCollect?.Invoke();
        lifetimeAmount += amount;
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
        IncreaseAmount(productionOutput * ProductionOutputMultiplier);
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
                IncreaseAmount(productionOutput * ProductionOutputMultiplier);
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
        storageStatsLabel.text = $"+{productionOutput*ProductionOutputMultiplier}    {amount} / {maxAmount}";
        speedLabel.text = $"production speed {speed:0.0}x";
    }
}
[Serializable]
public class SpecialObject 
{
    public string name;
    public int cost;
    public GameObject @object;
}
public enum UpgradeType 
{
    AutoCollect,
    Storage,
    CollectionSpeed,
    ProductionOutputMultiplier
}
[Serializable]
public class UpgradePrice
{
    public UpgradeType upgradeType;
    public int price;
}