using UnityEngine;

[ExecuteInEditMode]
public class ItemDrop : MonoBehaviour {
    private static string itemIconsPath = "Assets/Sprites/GameElements/cardIcons.png";
    private SpriteRenderer renderR;

    public GameObject popUp;
    public Vector3 popOffset = new Vector3(0, 0.2f, 0);
    public GameObject popUpInst;

    public Equipment item;

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
        
        popUpInst = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
        popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
    }

    private void OnTriggerStay2D(Collider2D other) {
        popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
        if (Input.GetButtonDown(InputManager.INTERACT)) {
            PickUp(other);
            Destroy(popUpInst);
            popUpInst = null;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        Destroy(popUpInst);
        popUpInst = null;
    }
}