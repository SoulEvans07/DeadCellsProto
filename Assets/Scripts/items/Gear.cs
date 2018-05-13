using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class Gear : MonoBehaviour{
    public string gearName;
    public string description;
    public int dps;
    public string extraInfo;
    public string spriteName;
    public GameObject effect;
    
    protected Image inventoryIcon;
    
    public abstract bool Use();
    
    private void Start() {
        name = gearName;
    }
    
    public void addIcon(Image image) {
        inventoryIcon = image;
    }
}