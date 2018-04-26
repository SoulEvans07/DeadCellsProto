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
            Vector3 preScale = arrow.transform.localScale;
            arrow.transform.localScale = new Vector3(Headless.instance.transform.localScale.x * preScale.x, preScale.y, preScale.z);
            arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(Headless.instance.transform.localScale.x * bowForce, 0));
            
            //Destroy(arrow, 1);
        }
    }
}