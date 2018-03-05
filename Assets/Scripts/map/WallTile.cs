using UnityEngine;

public class WallTile : MonoBehaviour
{
    private BoxCollider2D collidR;
    private SpriteRenderer renderR;

    void Start () {
        collidR = GetComponent<BoxCollider2D>();
        renderR = GetComponent<SpriteRenderer>();
		
        SetColliderSize();
    }

    private void SetColliderSize()
    {
        Vector2 size = renderR.size;
        size.y *= 0.98f;
        size.x *= 0.6f;
        collidR.size = size;
        Vector2 offset = collidR.offset;
        offset.x = collidR.size.x / 2f + 0.09f;
        offset.y = collidR.size.y / 2f;
        collidR.offset = offset;
    }
}