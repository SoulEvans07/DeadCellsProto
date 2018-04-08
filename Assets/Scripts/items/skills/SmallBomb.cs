using UnityEngine;

public class SmallBomb : Skill {
    public float throwForce = 250f;

    private void init() {
        equipmentName = "Small Bomb";
        spriteName = "SmallBomb";
        skillUseAnim = "";
        skillCooldown = 3f;
    }

    public SmallBomb() {
        init();
    }

    private void Start() {
        init();
    }

    public override void Use() {
        if (skillCooldownValue.Equals(0f)) {
            skillCooldownValue = skillCooldown;
            
            GameObject bomb = Instantiate(skillFx, transform.position, transform.rotation);
            bomb.transform.parent = null;
            bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(throwForce, throwForce));
            
            Destroy(bomb, 1);
        }
    }
    
}