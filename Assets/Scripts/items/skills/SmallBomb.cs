using UnityEngine;

public class SmallBomb : Skill {
    public float throwForce = 250f;

    private void init() {
        gearName = "Small Bomb";
        spriteName = "SmallBomb";
        skillUseAnim = "";
        skillCooldown = 3f;
        
        description = "We all like explosions.";
        dps = 10;
        extraInfo = null;
    }

    public SmallBomb() {
        init();
    }

    private void Start() {
        init();
    }

    public override bool Use() {
        if (skillCooldownValue > 0f)
            return false;
        skillCooldownValue = skillCooldown;
        
        GameObject bomb = Instantiate(skillFx, transform.position, transform.rotation);
        bomb.transform.parent = null;
        bomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(Headless.instance.transform.localScale.x * throwForce, throwForce));
        
        Destroy(bomb, 1);
        return true;
    }
    
}