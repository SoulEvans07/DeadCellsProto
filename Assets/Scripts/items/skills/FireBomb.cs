using UnityEngine;

public class FireBomb : Skill {
    public float throwForce = 250f;

    private void init() {
        equipmentName = "Fire Bomb";
        spriteName = "FireBomb";
        skillUseAnim = "";
        skillCooldown = 3f;

        description = "Let them burn!";
        dps = 8;
        extraInfo = null;
    }

    public FireBomb() {
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