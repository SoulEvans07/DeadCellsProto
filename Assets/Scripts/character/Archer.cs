using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    public float attackDistance = 2.5f;
//    public Vector3 slashFxOffset = new Vector3(0.213f, 0.056f, 0);
//    private Vector3 slashFxOffsetLeft;
    public GameObject arrowPref;
    public float bowForce = 100f;
    public int dps = 30;
    private bool attack = false;
    private bool attackPrep = false;


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

    private bool LineOfSight() {
        return Physics2D.Linecast(transform.position, Headless.instance.transform.position, whatIsGround);
    }
    
    protected void FixedUpdate() {
        if (Headless.instance == null || dead)
            return;
        UpdateHPBar();
        
        // attack
        Transform playerTrans = Headless.instance.transform;
        float diffX = (facingRight ? (playerTrans.position.x - transform.position.x) : (transform.position.x - playerTrans.position.x));
        float diffY =  Mathf.Abs(playerTrans.position.y - transform.position.y);

        attackCooldownValue = Mathf.Clamp(attackCooldownValue - Time.fixedDeltaTime, 0, attackCooldown);;
        
        if (0 < diffX && diffX < attackDistance && diffY < 0.1f && !LineOfSight()) {
            move = 0;
            if (attackCooldownValue <= 0) {
                attack = true;
            }
        } else {
            if (attackPrep) {
                attackPrep = false;
                attack = false;
                StopCoroutine(Attack());
            }
            if (!attack) {
                Collider2D coll = Physics2D.OverlapCircle(ledgeCheck.position, edgeRadius, whatIsGround);
                if (!coll || Physics2D.OverlapCircle(probe.position, 0.1f, whatIsGround)) {
                    Flip();
                }

                move = (facingRight ? 1 : -1) * maxSpeed;
            }
        }
        
        anim.SetFloat ("x-speed", Mathf.Abs (move));
        rd2d.velocity = new Vector2 (move, rd2d.velocity.y);
        
        if (attack && !attackPrep) {
            attackCooldownValue = attackCooldown;
            StartCoroutine(Attack ());
        }
    }

    IEnumerator Attack() {
        anim.Play("ArcherShoot");
        attackPrep = true;
        yield return new WaitForSeconds(15f / 36f);
        attackPrep = false;
        if (attack) {
            ShootArrow();
            attack = false;
        }
    }

    void ShootArrow() {
        GameObject arrow = Instantiate(arrowPref, probe.transform.position, transform.rotation);
        arrow.transform.parent = null;
        arrow.layer = LayerMask.NameToLayer("EnemyAttackFx");
        arrow.tag = "EnemyAtk";
        Vector3 preScale = arrow.transform.localScale;
        arrow.transform.localScale = new Vector3(transform.localScale.x * preScale.x, preScale.y, preScale.z);
        arrow.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.localScale.x * bowForce, 0));
        arrow.GetComponent<AttackFx>().damage =  (int) (dps * attackCooldown);
        Destroy(arrow, 1f);
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
    
    public void Hit(AttackFx playerAttack) {
        if (IsHitCooldownUp())
            return;
        
        TakeDamage(playerAttack.damage);

        if (playerAttack.dot != null) {
            playerAttack.dot.Apply(this);
        }

        ResetAttackCooldown();
        ResetHitCooldown();
    }

    public void TakeDamage(int damage) {
        health -= damage;
        anim.Play("DamageTaken");
    }

    void ResetAttackCooldown() {
        attackSignalTimeValue = attackSignalTime;
        attackCooldownValue = attackCooldown;
        StopCoroutine(Attack());
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
    
//    [ExecuteInEditMode]
//    void OnDrawGizmosSelected() {
//        Handles.color = Color.cyan;
//        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, attackDistance);
//        if (Headless.instance != null && Vector2.Distance(transform.position, Headless.instance.transform.position) < attackDistance) {
//            Handles.color = Color.white;
//            if(!LineOfSight())
//                UnityEditor.Handles.DrawLine(transform.position, Headless.instance.transform.position);
//            else
//                UnityEditor.Handles.DrawDottedLine(transform.position, Headless.instance.transform.position, 1f);
//        }
//    }
}