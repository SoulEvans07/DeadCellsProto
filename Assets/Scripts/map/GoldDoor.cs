using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldDoor : MonoBehaviour {
    private static string prisonElementPath = "Assets/Sprites/Map/prison.png";
    private SpriteRenderer spriteRenderer;
    public GameObject spriteObject;
    [ShowOnly] public String openSpriteName = "goldDoor";
    [ShowOnly] public bool open = false;

    public int price;
    public Vector2 priceTagOffset = new Vector2(0, -0.65f);
    public GameObject priceTagPref;
    [ShowOnly] public GameObject priceTagObject;
    
    public Vector3 popOffset = new Vector2(0, 0.65f);
    public GameObject popUp;
    [ShowOnly] public GameObject popUpObject;
    
    [ShowOnly] public Collider2D inside = null;

    void Start() {
        if (CanvasManager.instance == null)
            return;
        
        priceTagObject = Instantiate(priceTagPref, CanvasManager.instance.gameObject.transform);
        PriceTag priceTag = priceTagObject.GetComponent<PriceTag>();
        priceTag.target = transform;
        priceTag.offset = priceTagOffset;
        priceTag.SetValue(price);
        priceTag.UpdatePos();

        spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        SpriteLoader.loadSpritesFrom(prisonElementPath);
    }

    void Update() {
        if(open)
            return;
        
        
        
        if (inside != null && Input.GetButtonDown(InputManager.INTERACT)
            && inside.GetComponent<Headless>().Buy(price, null)) {
            Open(inside);
            Headless.instance.inventory.watching = false;
            Destroy(popUpObject);
            popUpObject = null;
            inside = null;
        }
    }

    void Open(Collider2D other) {
        open = true;
        spriteRenderer.sprite = SpriteLoader.getSprite(openSpriteName);
        spriteObject.transform.localPosition = new Vector2(0.24f, 0f);
        Destroy(priceTagObject);
        
        BoxCollider2D[] colliders = this.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D box in colliders) {
            Destroy(box);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
        if(open || !other.CompareTag("Player") || Headless.instance.inventory.watching || CanvasManager.instance == null || popUpObject != null)
            return;
        inside = other;
        popUpObject = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
        Headless.instance.inventory.watching = true;
        popUpObject.GetComponent<RectTransform>().position = transform.position + popOffset;
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if(open || !other.CompareTag("Player") || CanvasManager.instance == null)
            return;
        if (popUpObject == null && !Headless.instance.inventory.watching) {
            inside = other;
            popUpObject = Instantiate(popUp, CanvasManager.instance.gameObject.transform);
            Headless.instance.inventory.watching = true;
        }
        if(popUpObject != null)    
            popUpObject.GetComponent<RectTransform>().position = transform.position + popOffset;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(open || !other.CompareTag("Player") || popUpObject == null)
            return;
        inside = null;
        Headless.instance.inventory.watching = false;
        Destroy(popUpObject);
        popUpObject = null;
    }
}