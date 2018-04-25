using System.Collections.Generic;
using UnityEngine;

public class Living : MonoBehaviour {
    public float maxHealth = 30f;
    protected float health;
    protected bool dead = false;
    
    // buffs and debuffs
    public List<DamageOverTime> dotList;

    public void DotAffect(float damage) {
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
            
            dot.transform.localPosition = new Vector3(off, 0.38f);
        }
    }
}