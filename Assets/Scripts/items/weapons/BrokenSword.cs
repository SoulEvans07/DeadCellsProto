using System;
using UnityEngine;

public class BrokenSword : Weapon {

    [ShowOnly] public float offsetX = 0.2f;
    [ShowOnly] public float offsetY = 0f;

    private void init() {
        equipmentName = "Broken Sword";
        spriteName = "BrokenSword";
        attackAnim = "AtkSaberA";
        
        description = "";
        dps = 10;
        extraInfo = "Can not be sold.";
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
            
            Vector3 offset = new Vector3(Headless.instance.transform.localScale.x * offsetX, offsetY, 0);
            GameObject fx = Instantiate(attackFx, transform.position + offset, transform.rotation);
            fx.transform.parent = transform;
            Vector3 preScale = fx.transform.localScale;
            fx.transform.localScale = new Vector3(Headless.instance.transform.localScale.x * preScale.x, preScale.y, preScale.z);
            
            Destroy(fx, 1);
        }
    }

    
}