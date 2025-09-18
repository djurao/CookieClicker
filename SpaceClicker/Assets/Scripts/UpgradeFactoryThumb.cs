using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryModifierThumb : MonoBehaviour
{
    public FactoryModifier factoryModifier;
    public RawImage image;
    public TextMeshProUGUI descriptionLabel;
    public void Init(FactoryModifier fm)
    {
        factoryModifier = fm;
        image.texture = fm.icon;
        descriptionLabel.text = fm.description;
    }
    public void TryUpgrade() => factoryModifier.TryUpgrade();
}
