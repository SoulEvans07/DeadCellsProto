using UnityEngine;

public class SmallBomb : Skill {
    public float skillCooldown = 0.3f;
    [ShowOnly] public float skillCooldownValue = 0f;
    public float throwForce = 250f;

    void Start() {
        name = "Small Bomb";
        spriteName = "SmallBomb";
        skillUseAnim = "";
    }

    public override void Use() {
        if (skillCooldownValue.Equals(0f)) {
            skillCooldownValue = skillCooldown;
            
            GameObject bomb = Instantiate(skillFx, transform.position, transform.rotation);
            bomb.transform.parent = transform;
            bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForce, throwForce));
            
            Destroy(bomb, 1);
        }
    }
    
    private void Update() {
        skillCooldownValue = Mathf.Clamp(skillCooldownValue -  Time.fixedDeltaTime, 0, skillCooldown);
    }
}