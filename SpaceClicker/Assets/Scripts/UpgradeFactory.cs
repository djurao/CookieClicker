using UnityEngine;
using System.Linq;
using TMPro;
public class UpgradeFactory : MonoBehaviour
{
    public static UpgradeFactory Instance;
    public Factory currentFactorySubject;
    public GameObject upgradePanel;
    public GameObject noEnoughCurrencyPopup;
    public GameObject autoCollectUpgradeThumb;
    private void Awake()
    {
        Instance = this;
    }
    public void Open(Factory factorySubject)
    {
        currentFactorySubject = factorySubject;
        upgradePanel.SetActive(true);
    }

    public void Close()
    {
        currentFactorySubject = null;
        upgradePanel.SetActive(false);
    }
    private bool PayForUpgrade(UpgradeType upgradeType) 
    {
        var cost = currentFactorySubject.upgradePrices.FirstOrDefault(x => x.upgradeType == upgradeType);
        if (currentFactorySubject.amount >= cost.price) 
        {
            currentFactorySubject.amount -= cost.price;
            currentFactorySubject.UpdateLabels();
            return true;
        }
        noEnoughCurrencyPopup.SetActive(true);
        noEnoughCurrencyPopup.transform.GetComponentInChildren<TextMeshProUGUI>().text = $"Not enough {currentFactorySubject.currencyName}";
        Invoke(nameof(DisableNoEnoughCurrency), 2);
        return false;
    }
    private void DisableNoEnoughCurrency() => noEnoughCurrencyPopup.SetActive(false);
    public void UpgradeAutoCollect() 
    {
        if (!PayForUpgrade(UpgradeType.AutoCollect)) return;
        currentFactorySubject.autoCollect = 1;
        currentFactorySubject.autoCollectToggle.gameObject.SetActive(true);
        autoCollectUpgradeThumb.SetActive(false);
    }
    public void UpgradeStorage() 
    {
        if (!PayForUpgrade(UpgradeType.Storage)) return;
        currentFactorySubject.maxAmount += 20;
        currentFactorySubject.UpdateLabels();
    }
    public void UpgradeCollectionSpeed()
    {
        if (!PayForUpgrade(UpgradeType.CollectionSpeed)) return;
        currentFactorySubject.speed += 1;
        currentFactorySubject.UpdateLabels();
    }
    public void UpgradeCollectMultiplier()
    {
        if (!PayForUpgrade(UpgradeType.ProductionOutputMultiplier)) return;
        currentFactorySubject.ProductionOutputMultiplier += 1;
        currentFactorySubject.manualAmountLabel.text = $"+{currentFactorySubject.ProductionOutputMultiplier}";
    }
}
