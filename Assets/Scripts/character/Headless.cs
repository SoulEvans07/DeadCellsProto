using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Headless : Living {
    // Singleton
    public static Headless instance;

    void Awake() {
        if (instance != null)
            Debug.LogWarning("more than one instance");
        instance = this;
    }

    // movement
    public float speed = 4.5f;

    private float prevXSpeed = 0;

    public float airSlowness = 0.8f;

    private float nextVx = 0;
    private float nextVy = 0;

    // jump
    public int maxJumps = 2; // can be more with trinkets

    [ShowOnly] public int jumpSem; // jump semaphore
    private bool jumped = false;

    private Rigidbody2D rd2d;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public Transform probe;

    public LayerMask whatIsGround;

    // effects
    public GameObject airDash;

    // buffs and debuffs
    public List<DamageOverTime> dot;

    // inventory
    public Inventory inventory;

    // score
    private int gold = 0;

    public TextMeshProUGUI goldLabel;

    // health
    public Slider healthBar;

    public float hitCooldown = 1f;
    protected float hitCooldownValue = 0;
    public GameObject healEffect;
    public float healWait = 1f;
    private float healPress;
    public GameObject potion;

    void Start() {
        PlayerPrefs.DeleteKey("player-gold");
        goldLabel.text = gold.ToString();

        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dot = new List<DamageOverTime>();

        jumpSem = maxJumps;

        health = maxHealth;
        healthBar.value = (int) (100 * health / maxHealth);
    }

    void Update() {
        if (Time.timeScale.Equals(0))
            return;

        hitCooldownValue = Mathf.Clamp(hitCooldownValue - Time.deltaTime, 0, hitCooldown);

        float LX = Input.GetAxis(InputManager.AXIS_X);
        float LY = Input.GetAxis(InputManager.AXIS_Y);
        bool jump = Input.GetButtonDown(InputManager.JUMP);
        bool grounded = isGrounded();
        Collider2D standingOn = isGrounded();
        bool attackOne = Input.GetButton(InputManager.ATTACK1);
        bool attackTwo = Input.GetButton(InputManager.ATTACK2);
        bool skillOne = Input.GetAxisRaw(InputManager.SKILL1) > 0;
        bool skillTwo = Input.GetAxisRaw(InputManager.SKILL2) > 0;


        if (Input.GetButton(InputManager.HEAL)) {
            healPress += Time.deltaTime;
            if (healPress >= healWait) {
                healPress = 0;
                if (!health.Equals(maxHealth)) {
                    Heal(potion.GetComponent<PotionHandler>().UsePotion());
                }
            }
        }


        // moving
        if (LX.Equals(0) && grounded) {
            nextVx = 0;
        } else {
            if (!grounded) {
                // move slower (airSlowness = 0.7)
                if (hitWall() && (LX / transform.localScale.x) > 0)
                    nextVx = 0;
                else
                    nextVx = LX * speed * airSlowness;
            } else {
                // move
                nextVx = LX * speed;
            }
        }

        // jumping
        if (!jumped && jump) {
            if (grounded) {
                if (LY < 0 && standingOn.CompareTag("Platform")) {
                    standingOn.GetComponent<OneWayPlatform>().letThroughPlayer();
                } else if (jumpSem > 0) {
                    jumpSem--;
                    jumped = true;
                } 
            } else if (jumpSem > 0) {
                jumpSem--;
                spawnEffect(airDash, groundCheck, 1f);
                jumped = true;
            }
        } else {
            if (grounded) {
                // reset jump semaphore
                // on ground without wanting to jump
                jumpSem = maxJumps;
            }
        }

        if (attackOne) {
            inventory.UseX(anim);
        }
        if (attackTwo) {
            inventory.UseY(anim);
        }

        if (skillOne) {
            inventory.UseLT(anim);
        }
        if (skillTwo) {
            inventory.UseRT(anim);
        }
    }

    public void Heal(float amount) {
        if (amount <= 0)
            return;

        health += amount;
        UpdateHealthBar();
        GameObject effect = Instantiate(healEffect, transform) as GameObject;
        Destroy(effect, 0.8f);
    }

    private void FixedUpdate() {
        if (jumped) {
            rd2d.velocity = new Vector2(rd2d.velocity.x, 0);
            rd2d.AddRelativeForce(new Vector2(0, 400));
            jumped = false;
        } else {
            rd2d.velocity = new Vector2(nextVx, rd2d.velocity.y);
        }

        orientTransform();

        anim.SetBool("player-grounded", isGrounded());
        anim.SetFloat("player-x-speed", Mathf.Abs(rd2d.velocity.x));
        anim.SetFloat("player-y-speed", rd2d.velocity.y);

        UpdateHealthBar();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("EnemyAtk"))
            return;

        other.tag = "Untagged";
        AttackFx playerAttack = other.GetComponent<AttackFx>();
        if (playerAttack != null) {
            Hit(playerAttack);
        }
    }

    void Hit(AttackFx hit) {
        if (hitCooldownValue > 0) {
            return;
        }

//        attack = false;
//        shoot = false;

        hitCooldownValue = hitCooldown;
        health -= hit.damage;

//        anim.Update(100);
        anim.Play("PlayerHit");

        if (health <= 0) {
            health = 0;
            Die();
        }

        UpdateHealthBar();
    }

    public void Die() {
        PlayerPrefs.SetInt("player-gold", gold);

        health = 0;
        UpdateHealthBar();

        // Death animation
        Destroy(gameObject);

        if (AudioManager.instance != null) {
            AudioManager.instance.Stop("ActionIntro");
            AudioManager.instance.Stop("ActionLoop");
        }

        // Display highscore
        SceneManager.LoadScene("HighScore");
    }

    void UpdateHealthBar() {
        healthBar.value = (int) (100 * health / maxHealth);
    }

    public void PickUpGem(Gem gem) {
        gold += gem.value;
        goldLabel.text = gold.ToString();
        Destroy(gem.gameObject);
    }

    private void spawnEffect(GameObject fx, Transform pos, float duration) {
        GameObject fxObject = Instantiate(fx, pos.position, pos.rotation) as GameObject;
        if (duration >= 0)
            Destroy(fxObject, 2f);
    }

    private Collider2D isGrounded() {
        const float groundRadius = 0.05f;
        return Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
    }

    private bool hitWall() {
        const float wallRadius = 0.05f;
        return Physics2D.OverlapCircle(probe.position, wallRadius, whatIsGround);
    }

    private void orientTransform() {
        if (Mathf.Abs(prevXSpeed - rd2d.velocity.x) > Mathf.Abs(prevXSpeed)) {
            transform.localScale = new Vector2((rd2d.velocity.x < 0 ? -1 : 1),
                transform.localScale.y);
        }
        prevXSpeed = rd2d.velocity.x;
    }


    [ExecuteInEditMode]
    void OnDrawGizmosSelected() {
//        Handles.color = Color.cyan;
//        Vector3 center = spriteRenderer.sprite.bounds.center;
//        UnityEditor.Handles.DrawSolidDisc(transform.position + center, Vector3.back, 0.01f);
    }
}