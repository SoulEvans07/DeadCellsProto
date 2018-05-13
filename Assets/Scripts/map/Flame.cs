using UnityEngine;

public class Flame : MonoBehaviour {
    public GameObject flameDot;

    private void OnTriggerEnter2D(Collider2D other) {
        ApplyDot(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
        ApplyDot(other);
    }

    private void ApplyDot(Collider2D other) {
        if (!other.CompareTag("Enemy") && !other.CompareTag("Player"))
            return;

        if(other.GetComponent<Living>().IsHitCooldownUp())
            return;
        
        GameObject dotObject = Instantiate(flameDot, other.transform);
        DamageOverTime dot = dotObject.GetComponent<DamageOverTime>();
        dot.Apply(other.GetComponent<Living>());
        Destroy(dotObject, dot.duration);
    }
}