using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.UI;

public class Headless : Living {
    // Singleton
    public static Headless instance;

    void Awake(){
        if (instance != null)
            Debug.LogWarning ("more than one instance");
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

    void Start() {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer> ();
        dot = new List<DamageOverTime>();

        jumpSem = maxJumps;
    }

    void Update() {
        if(Time.timeScale.Equals(0))
            return;
        
        float LX = Input.GetAxis(InputManager.AXIS_X);
//        float LY = Input.GetAxis(InputManager.AXIS_Y);
        bool jump = Input.GetButtonDown(InputManager.JUMP);
        bool grounded = isGrounded();
        bool attackOne = Input.GetButton(InputManager.ATTACK1);
        bool attackTwo = Input.GetButton(InputManager.ATTACK2);
        bool skillOne = Input.GetAxisRaw(InputManager.SKILL1) > 0;
        bool skillTwo = Input.GetAxisRaw(InputManager.SKILL2) > 0;
        

        // moving
        if (LX.Equals(0) && grounded) {
            nextVx = 0;
        } else {
            if (!grounded) {
                // move slower (airSlowness = 0.7)
                if(hitWall() && (LX / transform.localScale.x) > 0)
                    nextVx = 0;
                else
                    nextVx = LX * speed * airSlowness;
            } else {
                // move
                nextVx = LX * speed;
            }
        }

        if (!jumped && jump) {
            if (grounded && jumpSem > 0) {
                jumpSem--;
                jumped = true;
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

        if(attackOne){
            inventory.UseX(anim);
        }
        if(attackTwo){
            inventory.UseY(anim);
        }

        if (skillOne) {
            inventory.UseLT(anim);
        }
        if (skillTwo) {
            inventory.UseRT(anim);
        }

//        if(attackTwo){
//            // get secondary weapon animation
//            playAnim();
//            // get secondary weapon attack
//            spawnAttack();
//        }
    }

    private HashSet<double> speeds = new HashSet<double>();
    
    private void FixedUpdate() {
        if (jumped) {
            rd2d.velocity = new Vector2(rd2d.velocity.x, 0);
            rd2d.AddRelativeForce(new Vector2(0, 400));
            jumped = false;
        } else {
            rd2d.velocity = new Vector2(nextVx, rd2d.velocity.y);
        }
        
        double vel = Math.Round(rd2d.velocity.x, 2);
        speeds.Add(vel);
        List<double> list = speeds.ToList();
        list.Sort();
        
        StringBuilder sb = new StringBuilder();
        foreach (double elem in list) {
            sb.Append(elem).Append(", ");
        }
        
        //Debug.Log(sb);

        orientTransform();
        
        anim.SetBool("player-grounded", isGrounded());
        anim.SetFloat("player-x-speed", Mathf.Abs(rd2d.velocity.x));
        anim.SetFloat("player-y-speed", rd2d.velocity.y);
    }

    private void spawnEffect(GameObject fx, Transform pos, float duration) {
        GameObject fxObject = Instantiate(fx, pos.position, pos.rotation) as GameObject;
        if (duration >= 0)
            Destroy(fxObject, 2f);
    }

    private bool isGrounded() {
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