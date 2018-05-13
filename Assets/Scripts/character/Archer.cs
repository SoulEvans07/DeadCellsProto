using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : Enemy {

    // moving and colliding
    public float maxSpeed = 0.4f;
    private float move = 0f;
    private bool facingRight = true;
    
    public Transform probe;
    public Transform groundCheck;
    public Transform ledgeCheck;
    public float edgeRadius = 0.01f;
    public LayerMask whatIsGround;

    // attack
    public float attackSignalTime = 1f;
    private float attackSignalTimeValue;
    public float attackCooldown = 1f;
    private float attackCooldownValue = 0f;
    public float attackDistance = 0.4f;
//    public Vector3 slashFxOffset = new Vector3(0.213f, 0.056f, 0);
//    private Vector3 slashFxOffsetLeft;
//    public GameObject slashAttackFx;
    private bool attackStarted = false;


    void Start() {
        InitEnemy();
        // attack
//        slashFxOffsetLeft = slashFxOffset;
//        slashFxOffsetLeft.x *= -1;
//
//        attackSignalTimeValue = attackSignalTime;
    }

    void Update() {
        UpdateHitCooldown();
    }

    protected override void FixedUpdate() {
        if (Headless.instance == null || dead)
            return;
        UpdateHPBar();
        
        // attack
        Transform playerTrans = Headless.instance.transform;
        float diffX = (facingRight ? (playerTrans.position.x - transform.position.x) : (transform.position.x - playerTrans.position.x));
        float diffY = playerTrans.position.y - transform.position.y;

        attackCooldownValue -= Time.fixedDeltaTime;
        
        if (0 < diffX && diffX < attackDistance && Mathf.Abs (diffY) < 0.1f) {
            move = 0;
            if (attackCooldownValue <= 0) {
                attackStarted = true;
            }
        } else if (!attackStarted) {
            Collider2D coll = Physics2D.OverlapCircle(ledgeCheck.position, edgeRadius, whatIsGround);
            if (!coll || Physics2D.OverlapCircle(probe.position, 0.1f, whatIsGround)) {
                Flip ();
            }

            move = (facingRight ? 1 : -1) * maxSpeed;
        }
        
        anim.SetFloat ("x-speed", Mathf.Abs (move));
        rd2d.velocity = new Vector2 (move, rd2d.velocity.y);
        
        if (attackStarted) {
            if (attackSignalTimeValue <= 0) {
                attackSignalTimeValue = attackSignalTime;
                attackCooldownValue = attackCooldown;
                attackStarted = false;
//                Attack ();
            }
            attackSignalTimeValue -= Time.fixedDeltaTime;
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
    
    public void Hit(AttackFx attack) {
        if (IsHitCooldownUp())
            return;
        
        ResetAttackCooldown();
        ResetHitCooldown();

        TakeDamage(attack.damage);

        if (attack.dot != null) {
            attack.dot.Apply(this);
        }
    }

    public void TakeDamage(int damage) {
        health -= damage;
        anim.Play("DamageTaken");
    }

    void ResetAttackCooldown() {
        attackSignalTimeValue = attackSignalTime;
        attackCooldownValue = attackCooldown;
        attackStarted = false;
    }

    void Flip() {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        theScale = probe.localScale;
        theScale.x *= -1;
        probe.localScale = theScale;

        healthBarOffset.x *= -1; 
        if(healthBarObject != null)
            healthBarObject.GetComponent<SliderFollowObject>().Flip();
    }
}