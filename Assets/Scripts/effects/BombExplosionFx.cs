using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionFx : MonoBehaviour {
    public float maxForce = 200;
    [ShowOnly] public float radius;

    void Start() {
        Destroy(this, 2f);
        radius = this.GetComponent<CircleCollider2D>().radius;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
            return;

        AttackFx playerAttack = GetComponent<AttackFx>();
        if (playerAttack != null && other.GetComponent<GrenadierZombie>() != null) {
            float dist = Vector2.Distance(transform.position, other.transform.position);
            float force = maxForce * (radius - dist);

            Vector2 fVector = (other.transform.position - transform.position).normalized * force;
            if (fVector.y < 0)
                fVector.y = 0;

            other.GetComponent<Rigidbody2D>().AddForce(fVector);
            other.tag = "Untagged";
            other.GetComponent<GrenadierZombie>().Hit(playerAttack);
        }
    }

//    void OnDrawGizmosSelected() {
//        Handles.color = Color.red;
//        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, radius);
//    }
}