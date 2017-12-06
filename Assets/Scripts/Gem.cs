using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Gem : Item {

	public int value = 5;
	private SpriteRenderer spriteR;
	public Sprite[] sprites;

	private void Start()
	{
		spriteR = gameObject.GetComponent<SpriteRenderer>();
		int randomGem = Random.Range(0, sprites.Length - 1);
		spriteR.sprite = sprites[randomGem];
	}

	protected override void OnPickUp(Collider2D other)
	{
		if (!other.CompareTag ("Player"))
			return;

		PlayerController.instance.PickUpGold (value);

		Destroy (gameObject);
	}

}
