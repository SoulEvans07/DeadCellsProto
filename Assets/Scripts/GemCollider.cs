using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollider : Item {

	public int value = 5;

	protected override void OnPickUp(Collider2D other)
	{
		if (!other.CompareTag ("Player"))
			return;

		PlayerController.instance.PickUpGold (value);

		Destroy (gameObject);
	}

}
