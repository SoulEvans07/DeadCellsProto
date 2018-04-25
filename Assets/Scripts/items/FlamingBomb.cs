using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamingBomb : MonoBehaviour {
    public GameObject explosionFx;
    public GameObject effect;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("PlayerAtk") || other.CompareTag("Player"))
            return;

        GameObject explosion = Instantiate(explosionFx, transform.position, transform.rotation) as GameObject;
        GameObject ef = Instantiate(effect);
        ef.SetActive(false);
        explosion.GetComponent<AttackFx>().dot = ef.GetComponent<DamageOverTime>();
        Destroy(explosion, 6f / 12f);
        Destroy(this.gameObject);
    }
}