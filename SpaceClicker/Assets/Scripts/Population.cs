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
    private DNA dna;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        dna = DNA.Instance;
        SetEra(dna.currentEvolutionStage);
    }
    public void SetEra(int index)
    {
        currentPopulationGroup = populationGroups[index];
        eraLabel.text = currentPopulationGroup.name;
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