using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    public bool watching = false;
    
    private Weapon weaponX;
    public Image weaponXImage;

    private Weapon weaponY;
    public Image weaponYImage;

    private Skill skillLT;
    public Image skillLTImage;

    private Skill skillRT;
    public Image skillRTImage;


    public bool addItemToInventory(Gear gear) {
        if (gear is Weapon) {
            if (weaponX == null) {
                placeX(gear);
            } else if (weaponY == null) {
                placeY(gear);
            } else {
                return false;
            }
        }

        if (gear is Skill) {
            if (skillLT == null) {
                placeLT(gear);
            } else if (skillRT == null) {
                placeRT(gear);
            } else {
                return false;
            }
        }

        return true;
    }

    public void placeX(Gear gear) {
        weaponX = (Weapon) gear;
        weaponX.transform.parent = transform;
        weaponXImage.sprite = SpriteLoader.getSprite(gear.spriteName);
        weaponXImage.color = Color.white;
        gear.addIcon(weaponXImage);
    }

    public void placeY(Gear gear) {
        weaponY = (Weapon) gear;
        weaponY.transform.parent = transform;
        weaponYImage.sprite = SpriteLoader.getSprite(gear.spriteName);
        weaponYImage.color = Color.white;
        gear.addIcon(weaponYImage);
    }

    public void placeLT(Gear gear) {
        skillLT = (Skill) gear;
        skillLT.transform.parent = transform;
        skillLTImage.sprite = SpriteLoader.getSprite(skillLT.spriteName);
        skillLTImage.color = Color.white;
        gear.addIcon(skillLTImage);
    }

    public void placeRT(Gear gear) {
        skillRT = (Skill) gear;
        skillRT.transform.parent = transform;
        skillRTImage.sprite = SpriteLoader.getSprite(skillRT.spriteName);
        skillRTImage.color = Color.white;
        gear.addIcon(skillRTImage);
    }
    
    public void UseX(Animator anim) {
        if (weaponX != null && weaponX.Use()) {
            anim.Play(weaponX.attackAnim);
        }
    }
    
    public void UseY(Animator anim) {
        if (weaponY != null && weaponY.Use()) {
            anim.Play(weaponY.attackAnim);
        }
    }

    public void UseLT(Animator anim) {
        if (skillLT != null && skillLT.Use()) {
            anim.Play(skillLT.skillUseAnim);
        }
    }
    
    public void UseRT(Animator anim) {
        if (skillRT != null && skillRT.Use()) {
            anim.Play(skillRT.skillUseAnim);
        }
    }

    public Weapon GetWeaponX() {
        return weaponX;
    }
    
    public Weapon GetWeaponY() {
        return weaponY;
    }
    
    public Skill GetSkillLT() {
        return skillLT;
    }
    
    public Skill GetSkillRT() {
        return skillRT;
    }
}