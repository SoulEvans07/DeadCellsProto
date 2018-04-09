using UnityEngine;

public class BeginnersBow : Weapon {
    public float bowForce = 10f;

    private void init() {
        equipmentName = "Begginers Bow";
        spriteName = "BeginnersBow";
//        attackAnim = "AtkBow";
        attackAnim = "PlayerLongBow";
        
        description = "";
        dps = 5;
        extraInfo = "Can not be sold.";
    }
    
    public BeginnersBow() {
        init();
    }

    private void Start() {
        init();
    }

    public override void Use() {
        if (attackCooldownValue.Equals(0f)) {
            attackCooldownValue = attackCooldown;

            GameObject arrow = Instantiate(attackFx, transform.position, transform.rotation);
            arrow.transform.parent = null;
            arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(bowForce, 0));
            
            //Destroy(arrow, 1);
        }
    }
}