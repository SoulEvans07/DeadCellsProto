using UnityEngine;
using UnityEngine.UI;

public class PotionHandler : MonoBehaviour
{
    public int maxVolume = 0;
    public int volume;
    public float healMultiplier = 10f; 

    private Image image;
    public Sprite[] sprites;

    private void Start()
    {
        volume = maxVolume;
        image = GetComponent<Image>();
        image.sprite = sprites[getId()];
    }

    private int getId()
    {
        return maxVolume * (maxVolume + 1) / 2 + volume;
    }

    public float UsePotion()
    {
        if (volume == 0)
            return 0;

        volume--;
        image.sprite = sprites[getId()];
        return maxVolume * healMultiplier;
    }
    
}