using System;
using UnityEngine;

public class BrokenSword : Weapon {

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
            
            GameObject fx = Instantiate(attackFx, transform.position, transform.rotation);
            fx.transform.parent = transform;
            Vector3 preScale = fx.transform.localScale;
            fx.transform.localScale = new Vector3(Headless.instance.transform.localScale.x * preScale.x, preScale.y, preScale.z);
            
            Destroy(fx, 1);
        }
    }

    
}