using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdate : MonoBehaviour
{
    public TextMeshProUGUI sliderValue;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        sliderValue.text = slider.value.ToString("0.0");
    }
}