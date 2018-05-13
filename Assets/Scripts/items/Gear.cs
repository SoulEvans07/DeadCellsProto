using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class Gear : MonoBehaviour{
    public string equipmentName;
    public string description;
    public int dps;
    public string extraInfo;
    public string spriteName;
    public GameObject effect;
    
    protected Image inventoryIcon;
    
    public abstract bool Use();
    
    private void Start() {
        name = equipmentName;
    }
    
    public void addIcon(Image image) {
        inventoryIcon = image;
    }
}