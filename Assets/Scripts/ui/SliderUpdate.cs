using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUpdate : MonoBehaviour
{
    public TextMeshProUGUI sliderValue;
    private Slider slider;

    public void SetValue(int value, int max) {
        slider = GetComponent<Slider>();
        slider.value = 100 * (float) value / max;
        sliderValue.text = value + " / " + max;
    }
}