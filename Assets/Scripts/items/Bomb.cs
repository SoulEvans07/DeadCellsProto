using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

	public GameObject explosionFx; 
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("explode!");
		GameObject explosion = Instantiate(explosionFx, transform.position, transform.rotation) as GameObject;
		Destroy(explosion, 6f/12f);
		Destroy(this.gameObject);
	}
}
