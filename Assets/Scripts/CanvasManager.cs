using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Singleton
    public static CanvasManager instance;
    
    void Awake(){
        if (instance != null)
            Debug.LogWarning ("more than one instance");
        instance = this;
    }
}