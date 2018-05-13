using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Singleton
    public static CanvasManager instance;
    
    void Awake(){
        if (instance != null)
            Debug.LogWarning ("more than one instance");
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if(AudioManager.instance != null){
            AudioManager.instance.Stop("Main");
            AudioManager.instance.Play("ActionIntro");
            AudioManager.instance.PlayAfter("ActionLoop");
        }
    }
}