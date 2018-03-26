using System;
using UnityEngine;

public class BrokenSword : Weapon {
    public float attackCooldown = 0.3f;
    [ShowOnly] public float attackCooldownValue = 0f;

    void Start() {
        name = "Broken Sword";
        spriteName = "BrokenSword";
        attackAnim = "AtkSaberA";
    }

    public override void Use() {
        if (attackCooldownValue.Equals(0f)) {
            attackCooldownValue = attackCooldown;
            
            GameObject fx = Instantiate(attackFx, transform.position, transform.rotation);
            fx.transform.parent = transform;
            
            Destroy(fx, 1);
        }
    }

    private void Update() {
        attackCooldownValue = Mathf.Clamp(attackCooldownValue -  Time.fixedDeltaTime, 0, attackCooldown);
    }
}