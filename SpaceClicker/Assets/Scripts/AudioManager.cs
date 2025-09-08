using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource genericAS;
    public AudioClip buttonClick;
    public AudioClip accepted;
    public AudioClip restricted;
    private void Awake()
    {
        Instance = this;
    }
    public void PlaySound(AudioClip clip) => genericAS.PlayOneShot(clip);
    public void PlayGenericButtonClick() => genericAS.PlayOneShot(buttonClick);

    internal void PlayAccepted() => genericAS.PlayOneShot(accepted);

    internal void PlayForbiden() => genericAS.PlayOneShot(restricted);
}
