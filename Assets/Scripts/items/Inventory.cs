using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    private Weapon weaponX;
    public Image weaponXImage;

    private Weapon weaponY;
    public Image weaponYImage;

    private Skill skillLT;
    public Image skillLTImage;

    private Skill skillRT;
    public Image skillRTImage;


    public void addItemToInventory(Equipment equipment) {
        if (equipment is Weapon) {
            weaponX = (Weapon) equipment;
            weaponX.transform.parent = transform;
            weaponXImage.sprite = SpriteLoader.getSprite(weaponX.spriteName);
            weaponXImage.color = Color.white;
            equipment.addIcon(weaponXImage);
        }

        if (equipment is Skill) {
            skillLT = (Skill) equipment;
            skillLT.transform.parent = transform;
            skillLTImage.sprite = SpriteLoader.getSprite(skillLT.spriteName);
            skillLTImage.color = Color.white;
            equipment.addIcon(skillLTImage);
        }
    }

    public void UseX(Animator anim) {
        if (weaponX != null) {
            anim.Play(weaponX.attackAnim);
            weaponX.Use();
        }
    }

    public void UseLT(Animator anim) {
        if (skillLT != null) {
            anim.Play(skillLT.skillUseAnim);
            skillLT.Use();
        }
    }
}