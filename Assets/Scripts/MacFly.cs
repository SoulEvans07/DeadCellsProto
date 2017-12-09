using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MacFly : MonoBehaviour
{
	public bool DrawGizmos = false;
	public float trackingRange = 1f;
	public float followingRange = 10f;
	public float followingDistance = 0.5f;

	private Transform target;

	public float speed = 0.01f;
	private Vector2 lastPos;
	private SpriteRenderer renderR;

	private Animator anim;
	public float atkLoad = 2/6f;
	private float atkLoadValue = 0;
	public float atkCooldown = 2f;
	private float atkCdValue = 0;
	private bool attack = false;

	public GameObject attackFx;
	public Vector2 fxOffset = new Vector2(0.3f, -0.05f);

	public float maxHealth = 10f;
	private float health;
	private bool dead = false;
	
	public GameObject spawnGem;
	public int maxGem = 3;
	private int gemNum;
	

	void Start ()
	{
		renderR = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		health = maxHealth;
		gemNum = (int) Random.Range (maxGem * 0.6f, maxGem);
	}

	void FixedUpdate()
	{
		if(dead)
			return;
		
		FixedUpdateTimers();
		
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
		else if(!attack && atkCdValue.Equals(0))
		{
			StartAttack();
			return;
		}

		if (attack && atkLoadValue.Equals(0))
		{
			Attack();
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag ("PlayerAtk") && !other.CompareTag("PlayerAtkArrow"))
			return;

		Attack playerAttack = other.GetComponent<Attack> ();
		if (other.CompareTag("PlayerAtkArrow"))
		{
			other.GetComponent<Rigidbody2D>().isKinematic = true;
			other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			Destroy(other.gameObject);
		}
		other.tag = "Untagged";
		if (playerAttack != null){	
			Hit(playerAttack);
		}
	}
	
	void Hit(Attack playerAttack)
	{
		attack = false;
		atkCdValue = atkCooldown;
		atkLoadValue = 0;

		health -= playerAttack.damage;

		anim.Update(100);
		anim.Play ("MacFlyHit");

		if (health <= 0)
			Die();
	}
	
	void Die()
	{
		dead = true;
		Destroy (gameObject, 0.5f);
		Transform playerTrans = PlayerController.instance.transform;

		while(gemNum > 0){
			Quaternion rotation = transform.rotation;
			GameObject gem = Instantiate (spawnGem, transform.position, rotation) as GameObject;
			int dir = (playerTrans.position.x - gem.transform.position.x) < 0 ? 1 : -1;
			gem.GetComponent<Rigidbody2D> ().AddForce(new Vector2( dir * Random.Range(100, 150), Random.Range(100, 150)));
			gemNum--;
		}
	}

	private void Attack()
	{
		atkCdValue = atkCooldown;
		anim.Play("MacFlyAtk");
		Vector3 fxPos = transform.position;
		if (renderR.flipX)
			fxPos.x -= fxOffset.x;
		else
			fxPos.x += fxOffset.x;
		fxPos.y += fxOffset.y;
		GameObject fx = Instantiate(attackFx, fxPos, transform.rotation) as GameObject;
		fx.GetComponentInChildren<SpriteRenderer>().flipX = renderR.flipX;
		Destroy(fx, 10f);
		attack = false;
	}

	private void StartAttack()
	{
		atkLoadValue = atkLoad;
		anim.Play("MacFlyAtkLoad");
		attack = true;
	}

	private void FixedUpdateTimers()
	{
		if (atkLoadValue > 0)
			atkLoadValue -= Time.fixedDeltaTime;
		else
			atkLoadValue = 0;
		
		if (atkCdValue > 0)
			atkCdValue -= Time.fixedDeltaTime;
		else
			atkCdValue = 0;
	}
	
	void OnDrawGizmosSelected() {
		if(!DrawGizmos)
			return;
		
		Handles.color = Color.red;
		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, trackingRange);
		Handles.color = Color.yellow;
		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, followingRange);
	}
}
