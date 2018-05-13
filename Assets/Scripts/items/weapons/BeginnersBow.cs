using UnityEngine;

public class BeginnersBow : Weapon {
    public float bowForce = 50f;

    private void init() {
        gearName = "Begginers Bow";
        spriteName = "BeginnersBow";
//        attackAnim = "AtkBow";
        attackAnim = "PlayerLongBow";
        
        description = "";
        dps = 100;
        extraInfo = "Can not be sold.";
    }
    
    public BeginnersBow() {
        init();
    }

    private void Start() {
        init();
    }

    public override bool Use() {
        if (attackCooldownValue > 0f)
            return false;
        
        attackCooldownValue = attackCooldown;

        GameObject arrow = Instantiate(attackFx, transform.position, transform.rotation);
        arrow.transform.parent = null;
        Vector3 preScale = arrow.transform.localScale;
        arrow.transform.localScale = new Vector3(Headless.instance.transform.localScale.x * preScale.x, preScale.y, preScale.z);
        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(Headless.instance.transform.localScale.x * bowForce, 0));
        arrow.GetComponent<AttackFx>().damage =  (int) (dps * attackCooldown);
        
        Destroy(arrow, 1f);

        return true;
    }
}