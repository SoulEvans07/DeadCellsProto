using UnityEngine;

public class Food : Item
{
    public float heal = 10f;
    
    protected override void OnPickUp(Collider2D other)
    {
        if (!other.CompareTag ("Player"))
            return;
        
        PlayerController.instance.PickUpFood (heal);
        
        Destroy(gameObject);
    }
}