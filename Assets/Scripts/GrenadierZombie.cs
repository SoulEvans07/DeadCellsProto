using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadierZombie : MonoBehaviour {
	// life
	public int health = 100;
	private bool dead = false;

	// moving and colliding
	public float maxSpeed = 2.8f;
	private bool facingRight = true;
	private Rigidbody2D rd2d;
	private Animator anim;
	private SpriteRenderer spriteRenderer;

	// attack
	private bool attack = false;
	public float attackDistance = 0.4f;
	public Vector3 slashFxOffset = new Vector3(0.213f, 0.056f, 0);
	private Vector3 slashFxOffsetLeft;
	public GameObject slashAttackFx;

	void Start () {
		rd2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		// attack
		slashFxOffsetLeft = slashFxOffset;
		slashFxOffsetLeft.x *= -1;
	}
	

	void FixedUpdate (){
		if (attack) {
			return;
		}

		SetBoxCollider ();
		if (dead) {
			Die ();
			return;
		}


		// live
		Transform playerTrans = PlayerController.instance.transform;
		float diffX = playerTrans.position.x - transform.position.x;
		float diffY = playerTrans.position.y - transform.position.y;
		if(0 < diffX && diffX < attackDistance && Mathf.Abs(diffY) < 0.1f){
			StartCoroutine(Attack());
		}


		//if (move > 0 && !facingRight)
		//	Flip ();
		//else if (move < 0 & facingRight)
		//	Flip ();
	}

	IEnumerator Attack(){
		attack = true;

		SpawnSlashFx();
		anim.SetBool ("zombie-attack", true);
		yield return new WaitForSeconds (0.8f);
		anim.SetBool ("zombie-attack", false);

		attack = false;
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
		health -= 10;
		if (!other.CompareTag ("PlayerAtk"))
			return;
		anim.SetTrigger ("zombie-hit");

		if (health <= 0)
			dead = true;

		Debug.Log ("zombie[ hp: " + health + " ]");
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
