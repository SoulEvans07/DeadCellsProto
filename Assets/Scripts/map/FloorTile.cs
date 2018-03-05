using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : MonoBehaviour {

    private BoxCollider2D collidR;
	private SpriteRenderer renderR;

	public bool placeLedge = true;

	public GameObject ledge;
	private GameObject leftLedge;
	private GameObject rightLedge;

	void Start () {
		collidR = GetComponent<BoxCollider2D>();
		renderR = GetComponent<SpriteRenderer>();
		
		SetColliderSize();
		if(placeLedge)
			CreateLedges();
	}

	private void SetColliderSize()
	{
		Vector2 size = renderR.size;
		size.y *= 0.98f;
		collidR.size = size;
		Vector2 offset = collidR.offset;
		offset.x = collidR.size.x / 2f;
		offset.y = collidR.size.y / 2f;
		collidR.offset = offset;
	}

	private void CreateLedges()
	{
		leftLedge = Instantiate(ledge, transform) as GameObject;
		rightLedge = Instantiate(ledge, transform) as GameObject;
		leftLedge.transform.localPosition = new Vector2(0, renderR.size.y);
		rightLedge.transform.localPosition = new Vector2(renderR.size.x, renderR.size.y);
	}
}
