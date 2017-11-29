using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierZombie : MonoBehaviour {
	// life
	public int health = 100;
	private bool dead = false;

	// moving and colliding
	public float maxSpeed = 0.4f;
	private bool facingRight = true;
	private Rigidbody2D rd2d;
	private Animator anim;
	private SpriteRenderer spriteRenderer;

	// attack
	public float attackCooldown = 1f;
	private float attackCooldownValue = 0f;
	public float attackDistance = 0.4f;
	public Vector3 slashFxOffset = new Vector3(0.213f, 0.056f, 0);
	private Vector3 slashFxOffsetLeft;
	public GameObject slashAttackFx;

	// hit
	public float hitCooldown = 22 / 24f;
	private float hitCooldownValue = 0;

	void Start () {
		rd2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		// attack
		slashFxOffsetLeft = slashFxOffset;
		slashFxOffsetLeft.x *= -1;
	}
	

	void FixedUpdate (){
		SetBoxCollider ();
		if (dead) {
			Die ();
			return;
		}


		// attack
		Transform playerTrans = PlayerController.instance.transform;
		float diffX = playerTrans.position.x - transform.position.x;
		float diffY = playerTrans.position.y - transform.position.y;

		attackCooldownValue -= Time.fixedDeltaTime;
		hitCooldownValue -= Time.fixedDeltaTime;

		if (0 < diffX && diffX < attackDistance && Mathf.Abs (diffY) < 0.1f) {
			rd2d.velocity = new Vector2 (0, 0);
			anim.SetFloat ("zombie-x-speed", 0);
			if (attackCooldownValue <= 0) {
				attackCooldownValue = attackCooldown;
				Attack ();
			}
		} else if (attackCooldownValue <= 0 && hitCooldownValue <= 0) {
			rd2d.velocity = new Vector2 (maxSpeed, 0);
			anim.SetFloat ("zombie-x-speed", maxSpeed);
		}


		//if (move > 0 && !facingRight)
		//	Flip ();
		//else if (move < 0 & facingRight)
		//	Flip ();
	}

	void Attack(){
		SpawnSlashFx();
		anim.Play("ZombieAtkA");
	}

	void SpawnSlashFx(){
		GameObject slash;
		if (facingRight) {
			slash = Instantiate(slashAttackFx, transform.position+slashFxOffset, transform.rotation) as GameObject;
		} else {
			slash = Instantiate(slashAttackFx, transform.position+slashFxOffsetLeft, transform.rotation) as GameObject;
			slash.transform.parent = gameObject.transform;
			Vector3 theScale = slash.transform.localScale;
			theScale.x *= -1;
			slash.transform.localScale = theScale;
		}
		if (slash != null) {
			slash.transform.parent = gameObject.transform;
			Destroy (slash, 1f);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag ("PlayerAtk"))
			return;

		other.tag = "Untagged";
		PlayerAttack playerAttack = other.GetComponent<PlayerAttack> ();
		if (playerAttack != null){	
			Hit(playerAttack);
		}
	}

	void Hit(PlayerAttack attack){
		hitCooldownValue = hitCooldown;
		health -= attack.damage;

		anim.Update(100);
		anim.Play ("ZombieHit");

		Debug.Log ("zombie[ hp: " + health + " ]");

		if (health <= 0)
			dead = true;
	}

	void Die(){
		// play animation
		anim.SetBool("zombie-dead", dead);
		Destroy (gameObject, 1f);
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void SetBoxCollider ()
	{
		BoxCollider2D itemBoxCollider2D = GetComponent<BoxCollider2D> ();
		if (itemBoxCollider2D != null) {
			itemBoxCollider2D.size = spriteRenderer.sprite.bounds.size;
			itemBoxCollider2D.size.Scale(new Vector3(0.9f, 0.9f, 0.9f));
		}
	}
}
