using UnityEngine;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    
    public void SetVolume(float value)
    {
        mixer.SetFloat("MainVolume", value);
        PlayerPrefs.SetFloat("volume", value);
    }
}