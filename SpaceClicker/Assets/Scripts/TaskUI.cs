using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TaskUI : MonoBehaviour
{
    public RawImage icon;
    public Image fillBar;
    public TextMeshProUGUI progressLabel;
    public float currentProgress = 0f;
    public float maxProgress = 100f;

    public void Init(float neededAmount, Texture2D icon)
    {
        maxProgress = neededAmount;
        this.icon.texture = icon;
        progressLabel.text = $"{currentProgress}/{maxProgress}";
    }
    public void UpdateProgress(float newProgress)
    {
        currentProgress = newProgress;
        fillBar.fillAmount = currentProgress / maxProgress;
        progressLabel.text = $"{currentProgress}/{maxProgress}";
    }
}
