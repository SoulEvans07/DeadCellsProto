using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollider : MonoBehaviour {

	public int value = 5;

	void OnTriggerEnter2D(Collider2D other){
		if (!other.CompareTag ("Player"))
			return;

		PlayerController.instance.PickUpGold (value);

		Destroy (gameObject);
	}
}
