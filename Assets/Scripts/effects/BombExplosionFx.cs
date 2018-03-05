using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionFx : MonoBehaviour
{
   
    void Start()
    {
        Destroy(this, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
    }
}