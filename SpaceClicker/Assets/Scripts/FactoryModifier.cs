using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class FactoryModifier
{
    [HideInInspector]internal UpgradeFactory upgradeFactory;
    [HideInInspector]public FactoryModifierThumb factoryModifierThumb;
    public ModifierType modifierType;
    public string name;
    public string description;
    public float value;
    public int upgradeCost;
    public float upgradeAmountToIncrease;
    public Texture2D icon;

     public void TryUpgrade()
    {
        if (!upgradeFactory.PayForUpgrade(this)) return;

        value += upgradeAmountToIncrease;

        if (modifierType == ModifierType.AutoCollect)
        {
            factoryModifierThumb.gameObject.SetActive(false);
            upgradeFactory.myFactory.autoCollectToggle.gameObject.SetActive(true);
        }
    }
}