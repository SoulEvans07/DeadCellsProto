using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class MacFly : MonoBehaviour
{
    public bool DrawGizmos = false;
    public float trackingRange = 1f;
    public float followingRange = 10f;
    public float followingDistance = 0.5f;

    private Transform target;

    public float maxSpeed = 0.5f;
    public float acceleration = 0.4f;
    private Vector2 lastPos;
    private SpriteRenderer renderR;
    private Rigidbody2D rgbody;

    private Animator anim;
    public float atkLoad = 2 / 6f;
    private float atkLoadValue = 0;
    public float atkCooldown = 2f;
    private float atkCdValue = 0;
    private bool attack = false;

    public float hitCooldown = 1f;
    private float hitCdValue = 0;

    public GameObject attackFx;
    public Vector2 fxOffset = new Vector2(0.3f, -0.05f);

    public float maxHealth = 10f;
    private float health;
    private bool dead = false;

    public GameObject hpBarPref;
    public Vector2 healthBarOffset = new Vector2(0, 0.01f);
    private GameObject healthBarObject;
    private Slider healthBar;

    public GameObject spawnGem;
    public int maxGem = 3;
    private int gemNum;


    void Start()
    {
        renderR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        health = maxHealth;
        gemNum = (int) Random.Range(maxGem * 0.6f, maxGem);

        rgbody = GetComponent<Rigidbody2D>();
        rgbody.gravityScale = 0;
    }

    void FixedUpdate()
    {
        if (dead)
            return;

        if (healthBarObject == null && health < maxHealth)
        {
            CreateHealthBar();
        }

        UpdateHPBar();

        FixedUpdateTimers();

        if (hitCdValue > 0)
        {
            rgbody.velocity -= rgbody.velocity * Time.fixedDeltaTime;
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
            rgbody.velocity = Vector2.zero;
            return;
        }

        if (Vector2.Distance(transform.position, target.position) > followingDistance)
        {
            Vector2 minVelocity = (target.position - transform.position).normalized;
            Vector2 velocity = Vector2.Lerp(minVelocity, minVelocity * maxSpeed, acceleration);
            rgbody.velocity = velocity;
        }
        else
        {
            if (!attack && atkCdValue.Equals(0))
            {
                StartAttack();
                return;
            }
            rgbody.velocity = Vector2.zero;
        }

        renderR.flipX = (transform.position.x - lastPos.x) < 0;
        if (target != null)
            renderR.flipX = (target.position.x - transform.position.x) < 0;
        lastPos = transform.position;

        if (attack && atkLoadValue.Equals(0))
        {
            Attack();
        }
    }

    void CreateHealthBar()
    {
        healthBarObject = Instantiate(hpBarPref, CanvasManager.instance.gameObject.transform) as GameObject;
        SliderFollowObject followObject = healthBarObject.GetComponent<SliderFollowObject>();
        followObject.target = transform;
        followObject.offset = healthBarOffset;
        followObject.UpdatePos();
        healthBar = healthBarObject.GetComponent<Slider>();
    }

    void UpdateHPBar()
    {
        if (healthBar != null)
            healthBar.value = health / maxHealth;
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
        Vector2 force = new Vector2(transform.position.x - pos.x, transform.position.y - pos.y);
        force.Scale(new Vector2(playerAttack.force, playerAttack.force));
        rgbody.velocity += force;


        attack = false;
        atkCdValue = atkCooldown;
        atkLoadValue = 0;

        hitCdValue = hitCooldown;

        health -= playerAttack.damage;

        anim.Update(100);
        anim.Play("MacFlyHit");

        if (health <= 0)
            Die();
    }

    void Die()
    {
        dead = true;
        Destroy(gameObject, 0.5f);
        Destroy(healthBarObject);
        Transform playerTrans = PlayerController.instance.transform;

        while (gemNum > 0)
        {
            Quaternion rotation = transform.rotation;
            GameObject gem = Instantiate(spawnGem, transform.position, rotation) as GameObject;
            int dir = (playerTrans.position.x - gem.transform.position.x) < 0 ? 1 : -1;
            gem.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(dir * Random.Range(100, 150), Random.Range(100, 150)));
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
        hitCdValue = FixedUpdateTimer(hitCdValue);
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
//		if(rgbody != null)
//			Handles.DrawLine(transform.position, transform.position+(Vector3)rgbody.velocity);
//
//		Handles.color = Color.red;
//		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, trackingRange);
//		Handles.color = Color.yellow;
//		UnityEditor.Handles.DrawWireDisc(transform.position ,Vector3.back, followingRange);
//	}
}