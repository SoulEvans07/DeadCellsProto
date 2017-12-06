using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected abstract void OnPickUp(Collider2D other);

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnPickUp(other);
    }
}