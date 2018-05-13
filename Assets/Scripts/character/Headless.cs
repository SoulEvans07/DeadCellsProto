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
    public Transform groundCheck;
    public Transform probeUp;
    public Transform probeDown;
    public LayerMask whatIsGround;

    // effects
    public GameObject airDash;

    // buffs and debuffs
    public List<DamageOverTime> dot;

    // inventory
    public Inventory inventory;

    // score
    public int gold = 0;
    public TextMeshProUGUI goldLabel;

    // health
    public Slider healthBar;
    public GameObject healEffect;
    public float healWait = 1f;
    private float healPress;
    public GameObject potion;

    void Start() {
        InitLiving();

        PlayerPrefs.DeleteKey("player-gold");
        goldLabel.text = gold.ToString();

        jumpSem = maxJumps;

        UpdateHealthBar();
    }

    void Update() {
        if (Time.timeScale.Equals(0))
            return;

        UpdateHitCooldown();

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

    public void Heal(int amount) {
        if (amount <= 0)
            return;

        health += amount;
        UpdateHealthBar();
        GameObject effect = Instantiate(healEffect, transform) as GameObject;
        Destroy(effect, 0.8f);
    }

    private void FixedUpdate() {
        if (jumped) {
            rd2d.velocity = new Vector3(rd2d.velocity.x, 0, -1);
            rd2d.AddRelativeForce(new Vector3(0, 400, -1));
            jumped = false;
        } else {
            rd2d.velocity = new Vector3(nextVx, rd2d.velocity.y, -1);
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

    public void SteppedInSpikes(int damage) {
        if (IsHitCooldownUp())
            return;

        ResetHitCooldown();
        health -= damage;

        anim.Play("PlayerHit");

        UpdateHealthBar();
    }

    void Hit(AttackFx hit) {
        if (IsHitCooldownUp())
            return;

//        attack = false;
//        shoot = false;

        ResetHitCooldown();
        TakeDamage(hit.damage);

        UpdateHealthBar();
    }
    
    public void TakeDamage(int damage) {
        health -= damage;
        anim.Play("DamageTaken");
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
        if (dead)
            return;
        if (health <= 0) {
            health = 0;
            dead = true;
            Die();
        }

        healthBar.GetComponent<SliderUpdate>().SetValue(health, maxHealth);
    }

    public void PickUpGem(Gem gem) {
        gold += gem.value;
        goldLabel.text = gold.ToString();
        Destroy(gem.gameObject);
    }

    public bool Buy(int price, Equipment item) {
        if (price > gold)
            return false;

        gold -= price;
        goldLabel.text = gold.ToString();
        
        if (item != null)
            inventory.addItemToInventory(item);
        
        return true;
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
        return Physics2D.OverlapBox(probeDown.position, probeUp.position - probeDown.position, 0, whatIsGround);
    }

    private void orientTransform() {
        if (Mathf.Abs(prevXSpeed - rd2d.velocity.x) > Mathf.Abs(prevXSpeed)) {
            transform.localScale = new Vector3((rd2d.velocity.x < 0 ? -1 : 1),
                transform.localScale.y, 1);
        }
        prevXSpeed = rd2d.velocity.x;
    }


//    [ExecuteInEditMode]
//    void OnDrawGizmosSelected() {
//        UnityEditor.Handles.DrawSolidRectangleWithOutline(
//            new Rect(probeDown.position, probeUp.position - probeDown.position), Color.green, Color.green);
//    }
}