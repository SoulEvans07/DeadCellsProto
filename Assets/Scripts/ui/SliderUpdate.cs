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

    public void SetValue(int value, int max) {
        slider.value = 100 * (float) value / max;
        sliderValue.text = value + " / " + max;
    }
}