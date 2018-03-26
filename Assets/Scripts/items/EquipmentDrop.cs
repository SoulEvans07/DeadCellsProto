using UnityEngine;

// TODO: merge with old Item classes

[ExecuteInEditMode]
public class EquipmentDrop : MonoBehaviour {
    private static string itemIconsPath = "Assets/Sprites/GameElements/cardIcons.png";
    private SpriteRenderer renderR;

    public Equipment item;

    private void Start() {
        SpriteLoader.loadSpritesFrom(itemIconsPath);
        if (item != null) {
            name = "EquipmentDrop[" + item.name + "]";
            renderR = GetComponent<SpriteRenderer>();
            renderR.sprite = SpriteLoader.getSprite(item.spriteName);
        }
    }

    private void PickUp(Collider2D other) {
        if (!other.CompareTag("Player"))
            return;

        Transform playerTransform = Headless.instance.transform;
        Equipment equip = Instantiate(item, playerTransform.position, playerTransform.rotation);
        Headless.instance.inventory.addItemToInventory(equip);
        Destroy(gameObject);
    }

    public void Sell() {
    }

//    protected abstract void OnPickUp(Collider2D other);

    private void OnTriggerEnter2D(Collider2D other) {
        PickUp(other);
    }
}