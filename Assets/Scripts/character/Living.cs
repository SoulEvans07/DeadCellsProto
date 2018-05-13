using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Living : MonoBehaviour {
    // basic components
    protected Rigidbody2D rd2d;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    
    public int maxHealth = 30;
    protected int health;
    protected bool dead = false;
    
    // buffs and debuffs
    public List<DamageOverTime> dotList;

    protected void InitLiving() {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        health = maxHealth;
        dotList = new List<DamageOverTime>();
    }

    public void DotAffect(int damage) {
        health -= damage;
    }

    public void AddDot(DamageOverTime dot) {
        dot.transform.parent = this.transform;
        dotList.Add(dot);
        ReArrangeDotIcons();
    }

    private void ReArrangeDotIcons() {
        int size = dotList.Count;
        for (int i = 0; i < size; i++) {
            float off = (-Mathf.Floor(size / 2f) + i) * (0.2f / size);
            DamageOverTime dot = dotList[i];
            if(dot != null)
                dot.transform.localPosition = new Vector3(off, 0.38f);
        }
    }
}