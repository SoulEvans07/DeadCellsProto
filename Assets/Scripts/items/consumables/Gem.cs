﻿using UnityEngine;

public class Gem : Item {
    public int value = 5;
    private SpriteRenderer spriteR;
    public Sprite[] sprites;

    private void Start() {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        int randomGem = Random.Range(0, sprites.Length - 1);
        spriteR.sprite = sprites[randomGem];
        value += (int) Random.Range(value * -0.1f, value * 0.1f);
    }

    protected override void OnPickUp(Collider2D other) {
        if (!other.CompareTag("Player"))
            return;

        Headless.instance.PickUpGem(this);
    }
}