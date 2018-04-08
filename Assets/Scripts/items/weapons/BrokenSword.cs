using System;
using UnityEngine;

public class BrokenSword : Weapon {

    private void init() {
        equipmentName = "Broken Sword";
        spriteName = "BrokenSword";
        attackAnim = "AtkSaberA";
    }

    public BrokenSword() {
        init();
    }

    private void Start() {
        init();
    }

    public override void Use() {
        if (attackCooldownValue.Equals(0f)) {
            attackCooldownValue = attackCooldown;
            
            GameObject fx = Instantiate(attackFx, transform.position, transform.rotation);
            fx.transform.parent = transform;
            
            Destroy(fx, 1);
        }
    }

    
}