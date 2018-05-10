using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Living {
    // life
    public GameObject hpBarPref;
    [ShowOnly] public Vector2 healthBarOffset = new Vector2(0.13f, 0.35f);
    protected GameObject healthBarObject;
    private Slider healthBar;
    
    // gems
    public GameObject spawnGem;
    public int maxGem = 5;
    private int gemNum;
    
    // basic components
    protected Rigidbody2D rd2d;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;

    protected void InitEnemy() {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        health = maxHealth;
        
        gemNum = (int) Random.Range(maxGem * 0.6f, maxGem);
        dotList = new List<DamageOverTime>();
    }
    
    protected void UpdateHPBar() {
        if (healthBarObject == null && health < maxHealth) {
            CreateHealthBar();
        }

        if (health <= 0) {
            health = 0;
            dead = true;
            Die();
        }
        
        if (healthBar != null)
            healthBar.value = health / maxHealth;
    }
    
    void CreateHealthBar() {
        if (CanvasManager.instance == null)
            return;
        healthBarObject = Instantiate(hpBarPref, CanvasManager.instance.gameObject.transform) as GameObject;
        SliderFollowObject followObject = healthBarObject.GetComponent<SliderFollowObject>();
        followObject.target = transform;
        followObject.offset = healthBarOffset;
        followObject.UpdatePos();
        healthBar = healthBarObject.GetComponent<Slider>();
    }
    
    void Die() {
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
        
        // TODO: play animation

        Destroy(gameObject, 1f);
        Destroy(healthBarObject);

        DropGems();        
    }

    void DropGems() {
        Transform playerTrans = Headless.instance.transform;
        while (gemNum > 0) {
            Quaternion rotation = transform.rotation;
            GameObject gem = Instantiate(spawnGem, transform.position, rotation) as GameObject;
            int dir = (playerTrans.position.x - gem.transform.position.x) < 0 ? 1 : -1;
            gem.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir * Random.Range(100, 150), Random.Range(100, 150)));
            gemNum--;
        }
    }
    
}