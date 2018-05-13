using System.Collections;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public void letThroughPlayer()
    {
        StartCoroutine(letThrough());
    }
    
    private IEnumerator letThrough()
    {
        PlatformEffector2D effector = GetComponent<PlatformEffector2D>();
        effector.colliderMask = -257;
        yield return new WaitForSeconds(0.2f); 
        effector.colliderMask = -1;
    }
}