using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSwitcher : MonoBehaviour {
    // OnGround
    [ShowOnly] public Equipment onGround;
    [ShowOnly] public ItemDrop drop;
    public GameObject onGroundSelection;
    public GameObject onGroundImg;
    private Image onGroundItemImg;
    public TextMeshProUGUI onGroundName;
    public TextMeshProUGUI onGroundDps;
    public TextMeshProUGUI onGroundDescr;
    public TextMeshProUGUI onGroundExtra;

    // OnLeft
    [ShowOnly] public Equipment onLeft;
    public GameObject onLeftSelection;
    private Image onLeftSelectionImage;
    public GameObject onLeftImg;
    private Image onLeftItemImg;
    public TextMeshProUGUI onLeftName;
    public TextMeshProUGUI onLeftDps;
    public TextMeshProUGUI onLeftDescr;
    public TextMeshProUGUI onLeftExtra;

    // OnRight
    [ShowOnly] public Equipment onRight;
    public GameObject onRightSelection;
    private Image onRightSelectionImage;
    public GameObject onRightImg;
    private Image onRightItemImg;
    public TextMeshProUGUI onRightName;
    public TextMeshProUGUI onRightDps;
    public TextMeshProUGUI onRightDescr;
    public TextMeshProUGUI onRightExtra;

    public void setOnGround(Equipment onGround, ItemDrop drop) {
        this.onGround = onGround;
        this.drop = drop;
        
        onGroundItemImg = onGroundImg.GetComponent<Image>();
        onGroundItemImg.sprite = SpriteLoader.getSprite(this.onGround.spriteName);
        onGroundName.text = this.onGround.equipmentName;
        onGroundDps.text = this.onGround.dps + " dps/s";
        onGroundDescr.text = this.onGround.description;
        onGroundExtra.text = this.onGround.extraInfo;
        onGroundExtra.enabled = !string.IsNullOrEmpty(onGroundExtra.text);
    }

    private void setOnLeft() {
        onLeftItemImg = onLeftImg.GetComponent<Image>();
        onLeftItemImg.sprite = SpriteLoader.getSprite(this.onLeft.spriteName);
        onLeftName.text = this.onLeft.equipmentName;
        onLeftDps.text = this.onLeft.dps + " dps/s";
        onLeftDescr.text = this.onLeft.description;
        onLeftExtra.text = this.onLeft.extraInfo;
        onLeftExtra.enabled = !string.IsNullOrEmpty(onLeftExtra.text);
    }

    private void setOnRight() {
        onRightItemImg = onRightImg.GetComponent<Image>();
        onRightItemImg.sprite = SpriteLoader.getSprite(this.onRight.spriteName);
        onRightName.text = this.onRight.equipmentName;
        onRightDps.text = this.onRight.dps + " dps/s";
        onRightDescr.text = this.onRight.description;
        onRightExtra.text = this.onRight.extraInfo;
        onRightExtra.enabled = !string.IsNullOrEmpty(onRightExtra.text);
    }

    void Start() {
        Time.timeScale = 0f;
        
        onGroundSelection.gameObject.GetComponent<Image>().enabled = false;
        onLeftSelectionImage = onLeftSelection.gameObject.GetComponent<Image>();
        onRightSelectionImage = onRightSelection.gameObject.GetComponent<Image>();
        onLeftSelectionImage.enabled = true;
        onRightSelectionImage.enabled = false;

        if (onGround is Weapon) {
            onLeft = Headless.instance.inventory.GetWeaponX();
            onRight = Headless.instance.inventory.GetWeaponY();
        } else if (onGround is Skill) {
            onLeft = Headless.instance.inventory.GetSkillLT();
            onRight = Headless.instance.inventory.GetSkillRT();
        }
        
        setOnLeft();
        setOnRight();
    }

    void Update() {
        if (Input.GetButtonUp(InputManager.BACK))
            Back();

        if (Input.GetButtonUp(InputManager.OK))
            PickUp();
        
        float LX = Input.GetAxis(InputManager.AXIS_X);

        bool left = onLeftSelectionImage.enabled;
        bool right = onRightSelectionImage.enabled;

        if (left && LX > 0) {
            onLeftSelectionImage.enabled = false;
            onRightSelectionImage.enabled = true;
        }

        if (right && LX < 0) {
            onRightSelectionImage.enabled = false;
            onLeftSelectionImage.enabled = true;
        }
    }

    private void Back() {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    private void PickUp() {
        bool left = onLeftSelectionImage.enabled;
        bool right = onRightSelectionImage.enabled;

        if (onGround is Weapon) {
            if (left) {
                drop.changeItem(Headless.instance.inventory.GetWeaponX());
                Headless.instance.inventory.placeX(onGround);
            } else if (right) {
                drop.changeItem(Headless.instance.inventory.GetWeaponY());
                Headless.instance.inventory.placeY(onGround);
            }
        } else if (onGround is Skill) {
            if (left) {
                drop.changeItem(Headless.instance.inventory.GetSkillLT());
                Headless.instance.inventory.placeLT(onGround);
            } else if (right) {
                drop.changeItem(Headless.instance.inventory.GetSkillRT());
                Headless.instance.inventory.placeRT(onGround);
            }
        }
        
        Back();
    }

}