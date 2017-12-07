using UnityEngine;

public class ArrowHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("PlayerAtkArrow"))
            return;
        
        other.tag = "Untagged";
        other.GetComponent<Rigidbody2D>().isKinematic = true;
        other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
//        other.GetComponent<Rigidbody2D>().angularVelocity = 0;
//        other.GetComponent<Rigidbody2D>().freezeRotation = true;
        other.transform.parent = gameObject.transform;
    }
}