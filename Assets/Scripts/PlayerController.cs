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
	public float jumpForce = 400.0f;
	public GameObject airDash;
	private bool doubleJumped = false;
	private bool stomped = false;

	// crouch
	private bool crouch;
	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		rd2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		anim.SetBool ("player-doublejump", doubleJumped);
	}

	void FixedUpdate ()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		if (grounded && doubleJumped) {
			doubleJumped = false;
			stomped = false;
			anim.SetBool ("player-doublejump", doubleJumped);
			anim.SetBool ("player-stomp", doubleJumped);
		}
		
		anim.SetBool ("player-ground", grounded);
		anim.SetFloat ("player-y-speed", rd2d.velocity.y);

		float vertical = Input.GetAxis ("Vertical");
		crouch = grounded && (vertical < 0);
		anim.SetBool ("player-crouch", crouch);

		float move = Input.GetAxis ("Horizontal");
		if (!crouch) {
			anim.SetFloat ("player-x-speed", Mathf.Abs (move));
			rd2d.velocity = new Vector2 (move * maxSpeed, rd2d.velocity.y);
		}

		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 & facingRight)
			Flip ();

		SetBoxCollider ();
	}

	void Update ()
	{
		if (grounded && Input.GetButtonDown ("Jump")) {
			anim.SetBool ("player-ground", false);
			rd2d.AddForce (new Vector2 (0, jumpForce));
		}
		if (!grounded && !doubleJumped && !stomped && Input.GetButtonDown ("Jump") && Input.GetAxis ("Vertical") >= 0) {
			rd2d.velocity = new Vector2 (rd2d.velocity.x, 0);
			rd2d.AddRelativeForce (new Vector2 (0, jumpForce));
			doubleJumped = true;
			Instantiate(airDash, groundCheck.position, groundCheck.rotation);
			anim.SetBool ("player-doublejump", doubleJumped);
		}
		if (!grounded && !stomped && Input.GetButtonDown ("Jump") && Input.GetAxis ("Vertical") < 0) {
			rd2d.velocity = new Vector2 (rd2d.velocity.x, 0);
			rd2d.AddRelativeForce (new Vector2 (0, -1 * jumpForce));
			stomped = true;
			anim.SetBool ("player-stomp", stomped);
		}
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
