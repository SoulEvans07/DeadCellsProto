using UnityEngine;
using UnityEngine.EventSystems;

public class ISaidNoMouse : MonoBehaviour {
    private GameObject selectedObj;

    void Start() {
        selectedObj = EventSystem.current.currentSelectedGameObject;
    }

    void Update() {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(selectedObj);

        selectedObj = EventSystem.current.currentSelectedGameObject;
    }
}