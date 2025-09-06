using System;
using TMPro;
using UnityEngine;

public class Era : MonoBehaviour
{
    public TextMeshProUGUI eraLabel;
    public EraDefinition[] eraDefinitions;
    public EraDefinition currentEra;
    public int currentEraIndex;
    public float currentEraProgress;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Era")) currentEraIndex = PlayerPrefs.GetInt("Era");
        SetEra(currentEraIndex);
    }
    public void SetEra(int index) 
    {
        currentEra = eraDefinitions[index];
        eraLabel.text = currentEra.Name;
        currentEra.factoies.SetActive(true);
    }
    public void DisableEra() 
    {
        currentEra.factoies.SetActive(false);
    }
    private void Update()
    {
        if (currentEraIndex == 0) 
        {
            currentEraProgress = Population.Instance.currentPopulationGroup.amount;
            if (currentEraProgress >= currentEra.eraCost.cost) 
            {
                ProgressToNextEra();
            }
        }
    }
    public void ProgressToNextEra() 
    {
        DisableEra();
        currentEraIndex++;
        SetEra(currentEraIndex);
        PlayerPrefs.SetInt("Era", currentEraIndex);
    }
}

[Serializable]
public class EraDefinition 
{
    public string Name;
    public string Description;
    public EraCost eraCost;
    public GameObject factoies;
}

[Serializable]
public class EraCost
{
    public string Name;
    public string Description;
    public int cost;
}