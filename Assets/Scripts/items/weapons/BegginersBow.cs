using UnityEngine;

public class BegginersBow : Weapon {
    public float bowForce = 10f;

    private void init() {
        equipmentName = "Begginers Bow";
        spriteName = "BegginersBow";
//        attackAnim = "AtkBow";
        attackAnim = "PlayerLongBow";
    }
    
    public BegginersBow() {
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