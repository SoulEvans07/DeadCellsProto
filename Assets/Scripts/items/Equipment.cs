using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public abstract class Equipment : MonoBehaviour{
    //public string description;
    public string equipmentName;
    public string spriteName;
    
    protected Image inventoryIcon;
    
    public abstract void Use();
    
    private void Start() {
        name = equipmentName;
    }
    
    public void addIcon(Image image) {
        inventoryIcon = image;
    }
}