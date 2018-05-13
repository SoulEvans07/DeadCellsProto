using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;

public class MacFly : Enemy
{
    public bool DrawGizmos = false;
    public float trackingRange = 1f;
    public float followingRange = 10f;
    public float followingDistance = 0.5f;

    private Transform target;

    public float maxSpeed = 0.5f;
    public float acceleration = 0.4f;
    private Vector2 lastPos;

    public float atkLoad = 2 / 6f;
    private float atkLoadValue = 0;
    public float atkCooldown = 2f;
    private float atkCdValue = 0;
    private bool attack = false;

    public GameObject attackFx;
    public Vector2 fxOffset = new Vector2(0.3f, -0.05f);

    void Start()
    {
        InitEnemy();
        rd2d.gravityScale = 0;
    }
    
    void Update() {
        UpdateHitCooldown();
    }

    protected override void FixedUpdate()
    {
        if (Headless.instance == null || dead)
            return;
        UpdateHPBar();

        FixedUpdateTimers();

        if (IsHitCooldownUp())
        {
            rd2d.velocity -= rd2d.velocity * Time.fixedDeltaTime;
            return;
        }

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
        {
            rd2d.velocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > followingDistance)
        {
            Vector2 minVelocity = (target.position - transform.position).normalized;
            Vector2 velocity = Vector2.Lerp(minVelocity, minVelocity * maxSpeed, acceleration);
            rd2d.velocity = velocity;
        }
        else
        {
            if (!attack && atkCdValue.Equals(0))
            {
                StartAttack();
                return;
            }
            rd2d.velocity = Vector2.zero;
        }

        spriteRenderer.flipX = (transform.position.x - lastPos.x) < 0;
        if (target != null)
            spriteRenderer.flipX = (target.position.x - transform.position.x) < 0;
        lastPos = transform.position;

        if (attack && atkLoadValue.Equals(0))
        {
            Attack();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("PlayerAtk") && !other.CompareTag("PlayerAtkArrow"))
            return;

        AttackFx playerAttack = other.GetComponent<AttackFx>();
        if (other.CompareTag("PlayerAtkArrow"))
        {
            other.GetComponent<Rigidbody2D>().isKinematic = true;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            Destroy(other.gameObject);
        }
        other.tag = "Untagged";
        if (playerAttack != null)
        {
            HitBy(playerAttack, other.transform.position);
        }
    }

    void HitBy(AttackFx playerAttack, Vector3 pos)
    {
        if(IsHitCooldownUp())
            return;
        
        Vector2 force = new Vector2(transform.position.x - pos.x, transform.position.y - pos.y);
        force.Scale(new Vector2(playerAttack.force, playerAttack.force));
        rd2d.velocity += force;


        attack = false;
        atkCdValue = atkCooldown;
        atkLoadValue = 0;

        ResetHitCooldown();

        health -= playerAttack.damage;

        anim.Play("DamageTaken");

        if (health <= 0)
            Die();
    }

    private void Attack()
    {
        atkCdValue = atkCooldown;
        anim.Play("MacFlyAtk");
        Vector3 fxPos = transform.position;
        if (spriteRenderer.flipX)
            fxPos.x -= fxOffset.x;
        else
            fxPos.x += fxOffset.x;
        fxPos.y += fxOffset.y;
        GameObject fx = Instantiate(attackFx, fxPos, transform.rotation) as GameObject;
        fx.GetComponentInChildren<SpriteRenderer>().flipX = spriteRenderer.flipX;
        Destroy(fx, 1f);
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
        atkLoadValue = FixedUpdateTimer(atkLoadValue);
        atkCdValue = FixedUpdateTimer(atkCdValue);
    }

    private static float FixedUpdateTimer(float value)
    {
        if (value > 0)
            return value - Time.fixedDeltaTime;
        return 0;
    }

//	void OnDrawGizmosSelected() {
//		if(!DrawGizmos)
//			return;
//		
//		Handles.color = Color.white;
//		if (target != null)
//		{
//			UnityEditor.Handles.DrawDottedLine(transform.position, target.position, 1);
//			UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, followingDistance);
//		}
//		
//		Handles.color = Color.cyan;
//		if(rd2d != null)
//			Handles.DrawLine(transform.position, transform.position+(Vector3)rd2d.velocity);
//
//		Handles.color = Color.red;
//		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, trackingRange);
//		Handles.color = Color.yellow;
//		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, followingRange);
//	}
}