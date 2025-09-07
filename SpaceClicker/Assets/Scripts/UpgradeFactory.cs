using UnityEngine;
using System.Linq;
using TMPro;
public class UpgradeFactory : MonoBehaviour
{
    public Factory myFactory;
    public GameObject upgradePanel;
    public GameObject noEnoughCurrencyPopup;
    public GameObject autoCollectUpgradeThumb;
    public TextMeshProUGUI autoCollectDesc;
    public TextMeshProUGUI storageDesc;
    public TextMeshProUGUI collectionSpeedDesc;
    public TextMeshProUGUI collectMultiplierDesc;

    public void OpenCloseFactoryUpgradePanel()
    {
        upgradePanel.SetActive(!upgradePanel.activeInHierarchy);
        autoCollectDesc.text = $"Auto Collect ({myFactory.upgradePrices.FirstOrDefault(x => x.upgradeType == UpgradeType.AutoCollect).price})";
        storageDesc.text = $"Storage +10 ({myFactory.upgradePrices.FirstOrDefault(x => x.upgradeType == UpgradeType.Storage).price})";
        collectionSpeedDesc.text = $"Speed +1 ({myFactory.upgradePrices.FirstOrDefault(x => x.upgradeType == UpgradeType.CollectionSpeed).price})";
        collectMultiplierDesc.text = $"Collect Output +1 ({myFactory.upgradePrices.FirstOrDefault(x => x.upgradeType == UpgradeType.ProductionOutputMultiplier).price})";
    } 
    private bool PayForUpgrade(UpgradeType upgradeType) 
    {
        var cost = myFactory.upgradePrices.FirstOrDefault(x => x.upgradeType == upgradeType);
        if (myFactory.amount >= cost.price) 
        {
            myFactory.amount -= cost.price;
            myFactory.UpdateLabels();
            return true;
        }
        noEnoughCurrencyPopup.SetActive(true);
        noEnoughCurrencyPopup.transform.GetComponentInChildren<TextMeshProUGUI>().text = $"Not enough {myFactory.currencyName}";
        Invoke(nameof(DisableNoEnoughCurrency), 2);
        return false;
    }
    private void DisableNoEnoughCurrency() => noEnoughCurrencyPopup.SetActive(false);
    public void UpgradeAutoCollect() 
    {
        if (!PayForUpgrade(UpgradeType.AutoCollect)) return;
        myFactory.autoCollect = 1;
        myFactory.autoCollectToggle.gameObject.SetActive(true);
        autoCollectUpgradeThumb.SetActive(false);
    }
    public void UpgradeStorage() 
    {
        if (!PayForUpgrade(UpgradeType.Storage)) return;
        myFactory.maxAmount += 10;
        myFactory.UpdateLabels();
    }
    public void UpgradeCollectionSpeed()
    {
        if (!PayForUpgrade(UpgradeType.CollectionSpeed)) return;
        myFactory.speed += 0.1f;
        myFactory.UpdateLabels();
    }
    public void UpgradeCollectMultiplier()
    {
        if (!PayForUpgrade(UpgradeType.ProductionOutputMultiplier)) return;
        myFactory.ProductionOutputMultiplier += 1;
        myFactory.UpdateLabels();
    }
}
