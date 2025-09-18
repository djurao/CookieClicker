using UnityEngine;
using System.Linq;
using TMPro;
using System;
using System.Collections.Generic;
public class UpgradeFactory : MonoBehaviour
{
    public Factory myFactory;
    public List<FactoryModifier> factoryModifiers;
    public List<FactoryModifierThumb> factoryModifierThumbs;
    public FactoryModifierThumb factoryModifierPrefab;
    public Transform modifiersParent;
    public GameObject upgradePanel;
    public GameObject noEnoughCurrencyPopup;
    public GameObject autoCollectUpgradeThumb;
    public GameObject autoCollectBlocker;

    void Start()
    {
        foreach (var factoryModifier in factoryModifiers)
        {
            factoryModifier.upgradeFactory = this;
            FactoryModifierThumb newFMT = Instantiate(factoryModifierPrefab, modifiersParent);
            factoryModifier.factoryModifierThumb = newFMT;
            newFMT.Init(factoryModifier);
            factoryModifierThumbs.Add(newFMT);
        }
    }
    public void OpenCloseFactoryUpgradePanel() => upgradePanel.SetActive(!upgradePanel.activeInHierarchy);
    public bool PayForUpgrade(FactoryModifier factoryModifier) 
    {
        if (myFactory.amount >= factoryModifier.upgradeCost) 
        {
            myFactory.amount -= factoryModifier.upgradeCost;
            myFactory.UpdateLabels();
            AudioManager.Instance.PlayAccepted();
            return true;
        }
        AudioManager.Instance.PlayForbiden();
        noEnoughCurrencyPopup.SetActive(true);
        noEnoughCurrencyPopup.transform.GetComponentInChildren<TextMeshProUGUI>().text = $"Not enough {myFactory.currencyName}";
        Invoke(nameof(DisableNoEnoughCurrency), 2);
        return false;
    }
    private void DisableNoEnoughCurrency() => noEnoughCurrencyPopup.SetActive(false);
}
