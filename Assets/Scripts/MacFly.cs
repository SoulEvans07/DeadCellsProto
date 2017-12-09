﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MacFly : MonoBehaviour
{
	public bool DrawGizmos = false;
	public float trackingRange = 1f;
	public float followingRange = 10f;
	public float followingDistance = 0.5f;

	public Transform target;

	public float speed = 0.01f;
	private Vector2 lastPos;
	private SpriteRenderer renderR;
	
	void OnDrawGizmosSelected() {
		if(!DrawGizmos)
			return;
		
		Handles.color = Color.red;
		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, trackingRange);
		Handles.color = Color.yellow;
		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, followingRange);
	}

	void Start ()
	{
		renderR = GetComponent<SpriteRenderer>();
	}

	void FixedUpdate()
	{
		if (target != null && Vector2.Distance(transform.position, target.position) > followingRange)
		{
			target = null;
			return;
		}

		Collider2D[] inRange = Physics2D.OverlapCircleAll(transform.position, trackingRange);
		foreach (Collider2D coll in inRange)
		{
			if (coll.CompareTag("Player"))
			{
				target = coll.gameObject.transform;
				break;
			}
		}

		if (target == null)
			return;

		if (Vector2.Distance(transform.position, target.position) > followingDistance)
		{
			Vector2 nextPos = Vector2.Lerp(transform.position, target.position, speed);
			renderR.flipX = (nextPos.x - lastPos.x) < 0;
			transform.position = nextPos;
			lastPos = nextPos;
		}
		else

		{
			// attack
		}
	}
}
