using UnityEngine;

public class WallFill : MonoBehaviour
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
        collidR.size = renderR.size;
    }
}