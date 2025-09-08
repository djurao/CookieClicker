using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
public class Settings : MonoBehaviour
{
    public float audioVolume = 1.0f;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValue;
    public List<AudioSource> audioSources;

    private void Start()
    {
        audioVolume = PlayerPrefs.GetFloat("audioVolume", 1.0f);
        volumeSlider.value = audioVolume;
        audioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None).ToList();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = audioVolume;
        }
    }

    public void OnVolumeSlided()
    {
        audioVolume = volumeSlider.value;
        volumeValue.text = (volumeSlider.value * 100).ToString("0");
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = audioVolume;
        }
        PlayerPrefs.SetFloat("audioVolume", audioVolume);
    }
}
