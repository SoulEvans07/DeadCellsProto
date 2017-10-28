using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	// moving and colliding
	public float maxSpeed = 5.0f;
	private bool facingRight = true;
	private Rigidbody2D rd2d;
	private Animator anim;

	// ground check
	public Transform groundCheck;
	public LayerMask whatIsGround;
	private bool grounded = false;
	private float groundRadius = 0.2f;

	// jump
	public float jumpForce = 80.0f;

	void Start ()
	{
		rd2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void FixedUpdate ()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("player-ground", grounded);
		anim.SetFloat ("player-y-speed", rd2d.velocity.y);

		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("player-x-speed", Mathf.Abs (move));
		rd2d.velocity = new Vector2 (move * maxSpeed, rd2d.velocity.y);

		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 & facingRight)
			Flip ();
	}

	void Update ()
	{
		if (grounded && Input.GetButton ("Jump")) {
			anim.SetBool ("player-ground", false);
			rd2d.AddForce (new Vector2 (0, jumpForce));
		}
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
