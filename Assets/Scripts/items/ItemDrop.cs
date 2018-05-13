using UnityEngine;

public class ItemDrop : MonoBehaviour {
    protected static string itemIconsPath = "Assets/Sprites/GameElements/cardIcons.png";
    protected SpriteRenderer spriteRenderer;
    
    public GameObject popUp;
    public Vector3 popOffset = new Vector3(0, 0.2f, -4.5f);
    [ShowOnly] public GameObject popUpInst;
    
    [ShowOnly] public Collider2D inside = null;

    protected virtual void PickUp(Collider2D other) {}

    protected void OnTriggerEnter2D(Collider2D other) {
        if(!other.CompareTag("Player") || Headless.instance.inventory.watching || CanvasManager.instance == null || popUpInst != null)
            return;
        inside = other;
        popUpInst = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
        Headless.instance.inventory.watching = true;
        popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
    }

    protected void Update() {
        if (inside != null && Input.GetButtonDown(InputManager.INTERACT)) {
            PickUp(inside);
            Headless.instance.inventory.watching = false;
            Destroy(popUpInst);
            popUpInst = null;
            inside = null;
        }
    }

    protected void OnTriggerStay2D(Collider2D other) {
        if(!other.CompareTag("Player") || CanvasManager.instance == null)
            return;
        if (popUpInst == null && !Headless.instance.inventory.watching) {
            inside = other;
            popUpInst = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
            Headless.instance.inventory.watching = true;
        }
        if (popUpInst != null) {
            popUpInst.GetComponent<RectTransform>().position = transform.position + popOffset;
        }
    }

    protected void OnTriggerExit2D(Collider2D other) {
        if(!other.CompareTag("Player") || popUpInst == null)
            return;
        inside = null;
        Headless.instance.inventory.watching = false;
        Destroy(popUpInst);
        popUpInst = null;
    }
    
    
    protected virtual void Sell() {
    }
}