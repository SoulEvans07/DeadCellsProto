using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{

	public int damage = 20;
	
	private SpriteRenderer spriteR;
	public Sprite normal;
	public Sprite bloody;

	void Start () {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!other.CompareTag ("Player"))
			return;

		spriteR.sprite = bloody;
		Headless.instance.SteppedInSpikes(damage);
	}

//	private void OnTriggerEnter2D(Collider2D other)
//	{
//		if (!other.CompareTag ("Player"))
//			return;
//
//		spriteR.sprite = bloody;
//		PlayerController.instance.SteppedInSpikes (damage);
//	}
}
