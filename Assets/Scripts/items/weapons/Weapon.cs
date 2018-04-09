using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : Equipment{
    public GameObject attackFx;
    public string attackAnim;
    
    public float attackCooldown = 0.3f;
    [ShowOnly] public float attackCooldownValue = 0f;

    protected void Update() {
        if(inventoryIcon == null)
            return;
        attackCooldownValue = Mathf.Clamp(attackCooldownValue -  Time.fixedDeltaTime, 0, attackCooldown);
        if (attackCooldownValue > 0) {
            inventoryIcon.color = Color.gray;
        } else if(inventoryIcon.color != Color.white) {
            inventoryIcon.color = Color.white;
        }
    }

}