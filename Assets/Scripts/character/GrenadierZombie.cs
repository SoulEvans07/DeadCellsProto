using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadierZombie : Living {
	// life
	public GameObject hpBarPref;
	public Vector2 healthBarOffset = new Vector2(0, 0.3f);
	private GameObject healthBarObject;
	private Slider healthBar;

	// moving and colliding
	public float maxSpeed = 0.4f;
	private float move = 0f;
	private bool facingRight = true;
	private Rigidbody2D rd2d;
	private Animator anim;
	private SpriteRenderer spriteRenderer;
	public Transform probe;
	public Transform groundCheck;
	public Transform ledgeCheck;
	public float edgeRadius = 0.01f;
	public LayerMask whatIsGround;
	public LayerMask whatIsLedge;

	// attack
	public float attackSignalTime = 1f;
	private float attackSignalTimeValue;
	public float attackCooldown = 1f;
	private float attackCooldownValue = 0f;
	public float attackDistance = 0.4f;
	public Vector3 slashFxOffset = new Vector3(0.213f, 0.056f, 0);
	private Vector3 slashFxOffsetLeft;
	public GameObject slashAttackFx;
	private bool attackStarted = false;

	// hit
	public float hitCooldown = 22 / 24f;
	private float hitCooldownValue = 0;

	// gems
	public GameObject spawnGem;
	public int maxGem = 5;
	private int gemNum;

	void Start ()
	{
		health = maxHealth;
		
		rd2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		// attack
		slashFxOffsetLeft = slashFxOffset;
		slashFxOffsetLeft.x *= -1;

		attackSignalTimeValue = attackSignalTime;

		gemNum = (int) Random.Range (maxGem * 0.6f, maxGem);
		dotList = new List<DamageOverTime>();
	}
	

	void FixedUpdate (){
		if(Headless.instance == null)
			return;
		SetBoxCollider ();
		if (health <= 0)
			dead = true;
		
		if (dead) {
			Die();
			return;
		}

		if (healthBarObject == null && health < maxHealth)
		{
			CreateHealthBar();
		}

		UpdateHPBar();
		
		// attack
		Transform playerTrans = Headless.instance.transform;
		float diffX = (facingRight ? (playerTrans.position.x - transform.position.x) : (transform.position.x - playerTrans.position.x));
		float diffY = playerTrans.position.y - transform.position.y;

		attackCooldownValue -= Time.fixedDeltaTime;
		hitCooldownValue -= Time.fixedDeltaTime;


		if (0 < diffX && diffX < attackDistance && Mathf.Abs (diffY) < 0.1f) {
			move = 0;
			if (attackCooldownValue <= 0) {
				attackStarted = true;
			}
		} else if (!attackStarted) { // attackCooldownValue <= 0 && attackSignalTimeValue == attackSignalTime && hitCooldownValue <= 0
			Collider2D coll = Physics2D.OverlapCircle(ledgeCheck.position, edgeRadius, whatIsGround);
			if (!coll || Physics2D.OverlapCircle(probe.position, 0.1f, whatIsGround)) {
				Flip ();
			}

			move = (facingRight ? 1 : -1) * maxSpeed;
		}

		anim.SetFloat ("zombie-x-speed", Mathf.Abs (move));
		rd2d.velocity = new Vector2 (move, rd2d.velocity.y);

		if (attackStarted) {
			if (attackSignalTimeValue <= 0) {
				attackSignalTimeValue = attackSignalTime;
				attackCooldownValue = attackCooldown;
				attackStarted = false;
				Attack ();
			}
			attackSignalTimeValue -= Time.fixedDeltaTime;
		}
	}

	void CreateHealthBar()
	{
		if(CanvasManager.instance == null)
			return;
		healthBarObject = Instantiate(hpBarPref, CanvasManager.instance.gameObject.transform) as GameObject;
		SliderFollowObject followObject = healthBarObject.GetComponent<SliderFollowObject>();
		followObject.target = transform;
		followObject.offset = healthBarOffset;
		followObject.UpdatePos();
		healthBar = healthBarObject.GetComponent<Slider>();
	}
	
	void UpdateHPBar()
	{
		if(healthBar != null)
			healthBar.value = health / maxHealth;
	}

	void Attack(){
		anim.Play("ZombieAtkA");
		SpawnSlashFx();
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
		if (!other.CompareTag ("PlayerAtk") && !other.CompareTag("PlayerAtkArrow"))
			return;

		AttackFx playerAttack = other.GetComponent<AttackFx> ();
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

	public void Hit(AttackFx attack){
		attackSignalTimeValue = attackSignalTime;
		attackCooldownValue = attackCooldown;
		attackStarted = false;

		hitCooldownValue = hitCooldown;

		health -= attack.damage;

		anim.Update(100);
		anim.Play ("ZombieHit");

		if (attack.dot != null) {
			attack.dot.Apply(this);
		}

//		Debug.Log(health);
		if (health <= 0)
			dead = true;
	}

	void Die(){
		Destroy(GetComponent<Rigidbody2D>());
		Destroy(GetComponent<Collider2D>());
		// play animation
		anim.SetBool("zombie-dead", dead);
			
		Destroy (gameObject, 1f);
		Destroy(healthBarObject);

		Transform playerTrans = Headless.instance.transform;

		while(gemNum > 0){
			Quaternion rotation = transform.rotation;
			GameObject gem = Instantiate (spawnGem, transform.position, rotation) as GameObject;
			int dir = (playerTrans.position.x - gem.transform.position.x) < 0 ? 1 : -1;
			gem.GetComponent<Rigidbody2D> ().AddForce(new Vector2( dir * Random.Range(100, 150), Random.Range(100, 150)));
			gemNum--;
		}
	}

	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

		theScale = probe.localScale;
		theScale.x *= -1;
		probe.localScale = theScale;
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
