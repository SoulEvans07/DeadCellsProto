using UnityEngine;

public abstract class Skill : Equipment {
    public GameObject skillFx;
    public string skillUseAnim;
    
    public float skillCooldown = 3f;
    [ShowOnly] public float skillCooldownValue = 0f;
    
    public void Update() {
        if(inventoryIcon == null)
            return;
        skillCooldownValue = Mathf.Clamp(skillCooldownValue -  Time.fixedDeltaTime, 0, skillCooldown);
        if (skillCooldownValue > 0) {
            inventoryIcon.color = Color.gray;
        } else if(inventoryIcon.color != Color.white) {
            inventoryIcon.color = Color.white;
        }
    }
}