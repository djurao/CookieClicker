using System;
using TMPro;
using UnityEngine;

public class Era : MonoBehaviour
{
    public static Era Instance;
    public TextMeshProUGUI eraLabel;
    public EraDefinition[] eraDefinitions;
    public EraDefinition currentEra;
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
        currentEra = eraDefinitions[index];
        eraLabel.text = currentEra.Name;
        Population.Instance.SetEra(index);
        currentEra.factories.SetActive(true);
    }
    public void DisableEra() 
    {
        currentEra.factories.SetActive(false);
    }
    private void Update()
    {
        if (dna.currentEvolutionStage == 0) 
        {
            if (DNA.Instance.CheckEraProgress()) 
            {
                ProgressToNextEra();
            }
        }
    }
    public void ProgressToNextEra() 
    {
        DisableEra();
        SetEra(dna.currentEvolutionStage);
        PlayerPrefs.SetInt("Era", dna.currentEvolutionStage);
    }
}

[Serializable]
public class EraDefinition 
{
    public string Name;
    public string Description;
    public GameObject factories;
}