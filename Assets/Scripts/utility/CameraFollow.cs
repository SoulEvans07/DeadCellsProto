﻿using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    public float offsetX;
    public float offsetY;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    private void LateUpdate()
    {
        if(target == null)
            return;
        
        Vector3 desired = transform.position;
        desired.x = target.position.x + offsetX;
        desired.y = target.position.y + offsetY;

        Vector3 smoothed = Vector3.Lerp(transform.position, desired, smoothSpeed);
        
        transform.position = smoothed;
    }
}