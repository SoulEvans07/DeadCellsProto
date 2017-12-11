using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume");
        }
            
    }
}