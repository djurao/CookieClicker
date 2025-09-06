using System;
using TMPro;
using UnityEngine;

public class Population : MonoBehaviour
{
    public static Population Instance;
    public TextMeshProUGUI eraLabel;
    public TextMeshProUGUI populationLabel;

    public PopulationGroup[] populationGroups;
    public PopulationGroup currentPopulationGroup;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SetEra(populationGroups[0]);
    }
    public void SetEra(PopulationGroup group)
    {
        currentPopulationGroup = group;
        eraLabel.text = group.name;
    }
    private void Update()
    {
        if (currentPopulationGroup != null) 
        {
            eraLabel.text = $"{currentPopulationGroup.name}: {currentPopulationGroup.amount}";
        }
    }
    public void IncreaseCurrentPopulation(int amount) 
    {
        print(amount);
        currentPopulationGroup.amount += amount;
    }
}
[Serializable]
public class PopulationGroup 
{
    public string name;
    public string description;
    public int amount;
}