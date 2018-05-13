using UnityEngine;

[ExecuteInEditMode]
public class GearDrop : ItemDrop {
    public GameObject itemSelect;
    [ShowOnly] public GameObject itemSelectInst;

    public Gear item;

    private void Start() {
        SpriteLoader.loadSpritesFrom(itemIconsPath);
        if (item != null) {
            name = "Gear[" + item.name + "]";
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = SpriteLoader.getSprite(item.spriteName);
        }
    }

    public void changeItem(Gear gear) {
        if (gear != null) {
            item = gear;
            name = "Gear[" + item.name + "]";
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = SpriteLoader.getSprite(item.spriteName);
        }
    }

    protected override void PickUp(Collider2D other) {
        if (!other.CompareTag("Player"))
            return;

        Transform playerTransform = Headless.instance.transform;
        Gear equip = Instantiate(item, playerTransform.position, playerTransform.rotation);
        if (!Headless.instance.inventory.addItemToInventory(equip)) {
            itemSelectInst = Instantiate(itemSelect, CanvasManager.instance.gameObject.transform);
            ItemSwitcher switcher = itemSelectInst.GetComponent<ItemSwitcher>();
            switcher.setOnGround(equip, this);
        } else {
            Destroy(gameObject);
        }
    }
}