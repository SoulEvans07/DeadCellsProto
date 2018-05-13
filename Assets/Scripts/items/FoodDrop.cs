using UnityEngine;

[ExecuteInEditMode]
public class FoodDrop : ItemDrop {

    public Food food;
    
    private void Start() {
        SpriteLoader.loadSpritesFrom(itemIconsPath);
        if (food != null) {
            name = "Food[" + food.name + "]";
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = SpriteLoader.getSprite(food.spriteName);
        }
    }
    
    protected override void PickUp(Collider2D other) {
        if (!other.CompareTag("Player"))
            return;

        if (Headless.instance.Heal(food.healValue)) {
            Destroy(gameObject);
        }
    }
}