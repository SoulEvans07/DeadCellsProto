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


    public bool addItemToInventory(Equipment equipment) {
        if (equipment is Weapon) {
            if (weaponX == null) {
                placeX(equipment);
            } else if (weaponY == null) {
                placeY(equipment);
            } else {
                return false;
            }
        }

        if (equipment is Skill) {
            if (skillLT == null) {
                placeLT(equipment);
            } else if (skillRT == null) {
                placeRT(equipment);
            } else {
                return false;
            }
        }

        return true;
    }

    public void placeX(Equipment equipment) {
        weaponX = (Weapon) equipment;
        weaponX.transform.parent = transform;
        weaponXImage.sprite = SpriteLoader.getSprite(equipment.spriteName);
        weaponXImage.color = Color.white;
        equipment.addIcon(weaponXImage);
    }

    public void placeY(Equipment equipment) {
        weaponY = (Weapon) equipment;
        weaponY.transform.parent = transform;
        weaponYImage.sprite = SpriteLoader.getSprite(equipment.spriteName);
        weaponYImage.color = Color.white;
        equipment.addIcon(weaponYImage);
    }

    public void placeLT(Equipment equipment) {
        skillLT = (Skill) equipment;
        skillLT.transform.parent = transform;
        skillLTImage.sprite = SpriteLoader.getSprite(skillLT.spriteName);
        skillLTImage.color = Color.white;
        equipment.addIcon(skillLTImage);
    }

    public void placeRT(Equipment equipment) {
        skillRT = (Skill) equipment;
        skillRT.transform.parent = transform;
        skillRTImage.sprite = SpriteLoader.getSprite(skillRT.spriteName);
        skillRTImage.color = Color.white;
        equipment.addIcon(skillRTImage);
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