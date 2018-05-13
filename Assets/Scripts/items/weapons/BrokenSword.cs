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
        dps = 119;
        extraInfo = "Can not be sold.";
    }

    public BrokenSword() {
        init();
    }

    private void Start() {
        init();
    }

    public override bool Use() {
        if (attackCooldownValue > 0f)
            return false;
        
        attackCooldownValue = attackCooldown;
        
        Vector3 offset = new Vector3(Headless.instance.transform.localScale.x * offsetX, offsetY, 0);
        GameObject fx = Instantiate(attackFx, transform.position + offset, transform.rotation);
        fx.transform.parent = transform;
        Vector3 preScale = fx.transform.localScale;
        fx.transform.localScale = new Vector3(Headless.instance.transform.localScale.x * preScale.x, preScale.y, preScale.z);
        fx.GetComponent<AttackFx>().damage =  (int) (dps * attackCooldown);
        
        Destroy(fx, 1);
        return true;
    }

    
}