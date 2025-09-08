using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class DNA : MonoBehaviour
{
    public static DNA Instance;
    public EvolutionRequirement[] evolutionRequirements;
    public int currentEvolutionStage = 0;   
    public float dna;
    public float dnaRate;
    public GameObject firstEraPanel;
    public RequirementsAttribute dnaAttribute;
    public Texture2D dnaIcon;
    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("Era"))currentEvolutionStage = PlayerPrefs.GetInt("Era");
        if(currentEvolutionStage == 0) firstEraPanel.SetActive(true);
        dnaAttribute = evolutionRequirements[currentEvolutionStage].requirementsAttributes[0]; 
        dnaAttribute.Init(dnaAttribute.requiredAmount, dnaIcon);

    }
    private void Update()
    {
        var population = Population.Instance.currentPopulationGroup.amount;
        if (dna < dnaAttribute.requiredAmount) dna += (dnaRate * population) * Time.deltaTime;
        else dna = dnaAttribute.requiredAmount;
        if (CheckEraProgress())
        {
            evolutionRequirements[currentEvolutionStage].evolutionPanel.SetActive(true);
            Time.timeScale = 0;
            currentEvolutionStage = (currentEvolutionStage < evolutionRequirements.Length - 1) ? currentEvolutionStage + 1 : currentEvolutionStage;
            Era.Instance.ProgressToNextEra();
            dnaAttribute = evolutionRequirements[currentEvolutionStage].requirementsAttributes[0];
            dnaAttribute.Init(dnaAttribute.requiredAmount, dnaIcon); // reset dna requirement display
        }
        dnaAttribute.UpdateProgress(Mathf.FloorToInt(dna));
    }
    internal bool CheckEraProgress()
    {
        bool allRequirementsMet = false;
        foreach (var req in evolutionRequirements[currentEvolutionStage].requirementsAttributes)
        {
            allRequirementsMet = req.currentAmount >= req.requiredAmount;
            if (!allRequirementsMet) break;
        }
        return allRequirementsMet;
    }
    public void ResumeTime() => Time.timeScale = 1;
}
[Serializable]
public class EvolutionRequirement
{
    public string speciesName;
    public RequirementsAttribute[] requirementsAttributes;
    public GameObject evolutionPanel;
}
[Serializable]
public class RequirementsAttribute 
{
    public TaskUI taskUI;
    public int currentAmount;
    public int requiredAmount;
    public void Init(int requiredAmount, Texture2D icon) 
    {
        this.requiredAmount = requiredAmount;
        taskUI.Init(requiredAmount, icon);
        UpdateProgress(0);
    }
    public void SetIcon(Texture2D texture) => taskUI.icon.texture = texture;
    internal void UpdateProgress(int lifetimeAmount)
    {
        currentAmount = lifetimeAmount;
        taskUI.UpdateProgress(currentAmount);
    }
}