using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
	// Singleton
	public static PlayerController instance;

	// health
	public float maxHealth = 30f;
	private float health;
	public float hitCooldown = 1f;
	private float hitCooldownValue = 0;
	private bool dead = false;
	public Slider healthBar;

	// moving and colliding
	public float maxSpeed = 2.8f;
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

	// attack
	private bool attack = false;
	public Vector3 saberFxOffset = new Vector3(0.213f, 0.056f, 0);
	private Vector3 saberFxOffsetLeft;
	public GameObject saberAttackFx;
	public float attackCooldown = 0.3f;
	private float attackCooldownValue = 0f;

	// score
	private int gold = 0;
	//private int cells = 0;
	public TextMeshProUGUI goldLabel;


	public PlayerController (){
		health = maxHealth;
	}

	public PlayerController(Vector3 startingPos){
		health = maxHealth;
		transform.position = startingPos;
	}

	void Awake(){
		if (instance != null)
			Debug.LogWarning ("more than one instance");
		instance = this;
	}

	void Start ()
	{
		PlayerPrefs.DeleteKey ("player-gold");;

		rd2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		anim.SetBool ("player-doublejump", doubleJumped);
		// attack
		saberFxOffsetLeft = saberFxOffset;
		saberFxOffsetLeft.x *= -1;

		healthBar.value = (int)(100 * health / maxHealth);
		goldLabel.text = gold.ToString();
	}

	public void PickUpFood(float value)
	{
		health += value;
		UpdateHealthBar();
	}

	public void PickUpGold(int value){
		gold += value;
		goldLabel.text = gold.ToString();
	}

	public void Die(){
		PlayerPrefs.SetInt ("player-gold", gold);

		health = 0;
		UpdateHealthBar();

		// Death animation
		Destroy(gameObject);

		// Display highscore
		SceneManager.LoadScene("HighScore");
	}

	void FixedUpdate ()
	{
		if (dead) {
			Die ();
		}
			
		attackCooldownValue -= Time.fixedDeltaTime;
		hitCooldownValue -= Time.fixedDeltaTime;

		if (attack) {
			anim.SetTrigger ("player-attackX");
			GameObject slash;
			if (facingRight) {
				slash = Instantiate(saberAttackFx, transform.position+saberFxOffset, transform.rotation) as GameObject;
			} else {
				slash = Instantiate(saberAttackFx, transform.position+saberFxOffsetLeft, transform.rotation) as GameObject;
				slash.transform.parent = gameObject.transform;
				Vector3 theScale = slash.transform.localScale;
				theScale.x *= -1;
				slash.transform.localScale = theScale;
			}
			if (slash != null) {
				slash.transform.parent = gameObject.transform;
				Destroy (slash, 1f);
			}
			//Debug.Log ("attack: " + attack);
			//WaitForAnim("AtkBackStabberB");
			attack = false;
			//Debug.Log ("attack: " + attack);
		} else {

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
			else if (move < 0 && facingRight)
				Flip ();

			SetBoxCollider ();
		}
	}

	void Update ()
	{
		if (attackCooldownValue <= 0 && Input.GetButtonDown ("Fire2")) {
			attackCooldownValue = attackCooldown;
			attack = true;
			return;
		}

		if (grounded && Input.GetButtonDown ("Jump")) {
			anim.SetBool ("player-ground", false);
			rd2d.AddForce (new Vector2 (0, jumpForce));
		}
		if (!grounded && !doubleJumped && !stomped && Input.GetButtonDown ("Jump") && Input.GetAxis ("Vertical") >= 0) {
			rd2d.velocity = new Vector2 (rd2d.velocity.x, 0);
			rd2d.AddRelativeForce (new Vector2 (0, jumpForce));
			doubleJumped = true;
			GameObject dashFx = Instantiate(airDash, groundCheck.position, groundCheck.rotation) as GameObject;
			Destroy (dashFx, 2f);
			anim.SetBool ("player-doublejump", doubleJumped);
		}
		if (!grounded && !stomped && Input.GetButtonDown ("Jump") && Input.GetAxis ("Vertical") < 0) {
			rd2d.velocity = new Vector2 (rd2d.velocity.x, 0);
			rd2d.AddRelativeForce (new Vector2 (0, -1 * jumpForce));
			stomped = true;
			anim.SetBool ("player-stomp", stomped);
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (!other.CompareTag ("EnemyAtk"))
			return;

		other.tag = "Untagged";
		Attack playerAttack = other.GetComponent<Attack> ();
		if (playerAttack != null){	
			Hit(playerAttack);
		}
	}

	void Hit(Attack attack){
		hitCooldownValue = hitCooldown;

		health -= attack.damage;

		anim.Update(100);
		anim.Play ("PlayerHit");

		Debug.Log ("player[ hp: " + health + " ]");

		if (health <= 0) {
			health = 0;
			dead = true;
		}

		Debug.Log("full: " + (100 * health / maxHealth) + ", int: " + (int)(100 * health / maxHealth));
		UpdateHealthBar();
	}

	void UpdateHealthBar()
	{
		healthBar.value = (int)(100 * health / maxHealth);	
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
