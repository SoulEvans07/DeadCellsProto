using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : Living {
    // life
    public GameObject hpBarPref;
    [ShowOnly] public Vector2 healthBarOffset = new Vector2(0.13f, 0.35f);
    protected GameObject healthBarObject;
    private Slider healthBar;
    protected String deathAnim;
    
    // gems
    public GameObject spawnGem;
    public int maxGem = 5;
    private int gemNum;
    
    protected void InitEnemy() {
        InitLiving();
        gemNum = (int) Random.Range(maxGem * 0.6f, maxGem);
    }

    protected void UpdateHPBar() {
        if (healthBarObject == null && health < maxHealth) {
            CreateHealthBar();
        }

        if (health <= 0) {
            health = 0;
            Die();
        }
        
        if (healthBar != null)
            healthBar.value = (float) health / maxHealth;
    }
    
    protected void CreateHealthBar() {
        if (CanvasManager.instance == null)
            return;
        healthBarObject = Instantiate(hpBarPref, CanvasManager.instance.gameObject.transform) as GameObject;
        SliderFollowObject followObject = healthBarObject.GetComponent<SliderFollowObject>();
        followObject.target = transform;
        followObject.offset = healthBarOffset;
        followObject.UpdatePos();
        healthBar = healthBarObject.GetComponent<Slider>();
    }
    
    protected void Die() {
        dead = true;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());
        
        anim.Play("Death");

        Destroy(gameObject, 1f);
        Destroy(healthBarObject);

        DropGems();        
    }

    protected void DropGems() {
        Transform playerTrans = Headless.instance.transform;
        while (gemNum > 0) {
            Quaternion rotation = transform.rotation;
            Vector3 gemPos = new Vector3(transform.position.x, transform.position.y, -1);
            GameObject gem = Instantiate (spawnGem, gemPos, rotation) as GameObject;
            int dir = (playerTrans.position.x - gem.transform.position.x) < 0 ? 1 : -1;
            gem.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir * Random.Range(100, 150), Random.Range(100, 150)));
            gemNum--;
        }
    }
    
}