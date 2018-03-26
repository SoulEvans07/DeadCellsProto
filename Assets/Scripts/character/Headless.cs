using UnityEditor;
using UnityEngine;
using UnityEngine.Collections;
using UnityEngine.UI;

public class Headless : MonoBehaviour {
    // Singleton
    public static Headless instance;

    void Awake(){
        if (instance != null)
            Debug.LogWarning ("more than one instance");
        instance = this;
    }
    
    // movement
    public float speed = 4.5f;

    public float airSlowness = 0.8f;

    private float nextVx = 0;
    private float nextVy = 0;

    // jump
    public int maxJumps = 2; // can be more with trinkets

    [ShowOnly] public int jumpSem; // jump semaphore
    private bool jumped = false;

    private Rigidbody2D rd2d;
    private Animator anim;
    public Transform groundCheck;
    public Transform probe;

    public LayerMask whatIsGround;

    // effects
    public GameObject airDash;
    
    // inventory
    public Inventory inventory;

    void Start() {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        jumpSem = maxJumps;
    }

    void Update() {
        float LX = Input.GetAxis(InputManager.AXIS_X);
//        float LY = Input.GetAxis(InputManager.AXIS_Y);
        bool jump = Input.GetButtonDown(InputManager.JUMP);
        bool grounded = isGrounded();
        bool attackOne = Input.GetButton(InputManager.ATTACK1);
//        bool attackTwo = Input.GetButton(InputManager.ATTACK2);
        bool skillOne = Input.GetAxisRaw(InputManager.SKILL1) > 0;
//        bool skillTwo = Input.GetButton(InputManager.SKILL2);
        

        // moving
        if (LX.Equals(0) && grounded) {
            nextVx = 0;
        } else {
            if (!grounded) {
                // move slower (airSlowness = 0.7)
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

        if (skillOne) {
            inventory.UseLT(anim);
        }

//        if(attackTwo){
//            // get secondary weapon animation
//            playAnim();
//            // get secondary weapon attack
//            spawnAttack();
//        }
    }

    private void FixedUpdate() {
        if (jumped) {
            rd2d.velocity = new Vector2(rd2d.velocity.x, 0);
            rd2d.AddRelativeForce(new Vector2(0, 400));
            jumped = false;
        } else {
            rd2d.velocity = new Vector2(nextVx, rd2d.velocity.y);
        }
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


    void OnDrawGizmosSelected() {
//        Handles.color = Color.yellow;
//        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, followingRange);
    }
}