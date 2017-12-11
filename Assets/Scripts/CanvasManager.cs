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

    private void Start()
    {
        AudioManager.instance.Stop(AudioManager.instance.playing);
        AudioManager.instance.Play("ActionIntro");
        AudioManager.instance.PlayAfter("ActionLoop");
    }
}