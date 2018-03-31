using UnityEngine;

[ExecuteInEditMode]
public class ItemDrop : MonoBehaviour {
    private static string itemIconsPath = "Assets/Sprites/GameElements/cardIcons.png";
    private SpriteRenderer renderR;

    public GameObject popUp;
    public Vector3 popOffset = new Vector3(0, 0.2f, 0);
    public GameObject popUpInst;

    public Equipment item;

    public Collider2D inside = null;

    private void Start() {
        SpriteLoader.loadSpritesFrom(itemIconsPath);
        if (item != null) {
            name = "Item[" + item.name + "]";
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
        if(!other.CompareTag("Player") || CanvasManager.instance == null || popUpInst != null)
            return;
        inside = other;
        popUpInst = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
        popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
    }

    private void Update() {
        if (inside != null && Input.GetButtonDown(InputManager.INTERACT)) {
            PickUp(inside);
            Destroy(popUpInst);
            popUpInst = null;
            inside = null;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(!other.CompareTag("Player") || popUpInst == null)
            return;
        inside = null;
        Destroy(popUpInst);
        popUpInst = null;
    }
}