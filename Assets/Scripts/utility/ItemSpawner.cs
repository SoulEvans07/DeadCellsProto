using UnityEngine;

public class ItemSpawner : MonoBehaviour {
    
    public GameObject itemDrop;
    public Camera cameraloc;

    void Update() {
        
        if (Input.GetKeyDown(KeyCode.I)) {
            Vector2 pos = cameraloc.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(itemDrop, pos, Quaternion.identity);
        }
    }
}