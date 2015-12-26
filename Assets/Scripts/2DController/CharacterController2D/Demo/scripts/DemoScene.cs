using UnityEngine;
using System.Collections;
using Prime31;

public class DemoScene : MonoBehaviour
{
    // movement config
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;
    public float jumpHeight = 3f;
    public float dashBoost = 15f;
    public float airDashBoost = 15f;
    private int dashCount = 0;
    private int dashMax = 1;
    public float doubleJumpHeight = 1.1f;

    private float airDashTime = 0f;
    public float airDashDuration = .5f;

    public float comboTime = 2f;
    public float comboCountdown = 0f;
    public int attackCount = 0;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    

    private bool left = false;
    private bool useAirDash = false;
    private bool isDiving = false;
    private AttackController atkController;
    private PlayerHealth ph;
    private float shotCountdown = .5f;
    public bool isBlocking = false;

    public float ButtonDelay;
    float lastJump = 0;
    float lastUse = 0;
    public bool isDashing = false;
    BoxCollider2D myBoxCollider;
    
    public int jumpCount = 0;
    public int airDashCount = 0;
    public GameObject attackBox;
    public float shotTime = 0;
    private Vector3 moveDir = Vector3.zero;

    public float doubleJumpDelayTimer = 0f;
    public float doubleJumpCooldown = 0.1f;

    public PlayerMode pm;
    private bool doJump = false;    

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        myBoxCollider = gameObject.GetComponent<BoxCollider2D>();
        pm = gameObject.GetComponent<PlayerMode>();
        atkController = gameObject.GetComponent<AttackController>();
        ph = gameObject.GetComponent<PlayerHealth>();
    }


    #region Event Listeners

    void onControllerCollider(RaycastHit2D hit)
    {
        // bail out on plain old ground hits cause they arent very interesting
        if (hit.normal.y == 1f)
            return;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }


    void onTriggerEnterEvent(Collider2D col)
    {
     //   Debug.Log("onTriggerEnterEvent: " + col.tag + " " + col.name + " ");
        if (col.tag.Equals("DestructPlat"))
        {
            FallApart obj = (FallApart)col.gameObject.GetComponent<FallApart>();
            obj.triggerTimer();
        }
        if (col.tag.Equals("DeathWall"))
        {
      //      Debug.Log("Should Die");
            gameObject.GetComponent<PlayerHealth>().adjustCurrentHealth(-100000);
        }

    }


    void onTriggerExitEvent(Collider2D col)
    {
        //Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    void FixedUpdate()
    {
        updateTimers();

        if (_controller.collisionState.becameGroundedThisFrame)
        {
            jumpCount = 0;
            doubleJumpDelayTimer = 0f;
        }

    }

    #endregion
    // the Update loop contains a very simple example of moving the character around and controlling the animation
    void Update()
    {
        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
        moveDir.x = Input.GetAxis("Horizontal");
        moveDir.y = Input.GetAxis("Vertical");
        if (!isDashing)
        {
            if (moveDir.x < 0)
            {
                left = true;
            }
            else if (moveDir.x > 0)
            {
                left = false;
            }
        }

        #region movement
        if (isDashing)
        {
            if (airDashTime > airDashDuration)
            {
                isDashing = false; 
            }
        }
        else
        {
            if (comboCountdown > ButtonDelay || comboCountdown == 0)
            {
                normalizedHorizontalSpeed = moveDir.x;
                if(normalizedHorizontalSpeed > 0){
                    if (transform.localScale.x < 0f)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    left = false;
                    isBlocking = false;
                }
                else if (normalizedHorizontalSpeed < 0)
                {
                    if (transform.localScale.x > 0f)
                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    left = true;
                    isBlocking = false;
                }
                else
                {
                    normalizedHorizontalSpeed = 0;
                }
            }
            if (Input.GetButtonDown("Dash"))
            {
                if (!_controller.isGrounded)
                {
                    if (airDashCount < 1)
                    {
                        useAirDash = true;
                        airDashCount++;
                        isDashing = true;
                        _animator.Play(Animator.StringToHash("Airdash"));
                    }
                    else
                    {
                        isDashing = false;
                    }
                }
                else
                {
                    isDashing = true;
                    _animator.Play(Animator.StringToHash("ShoulderCharge"));
                    comboCountdown = 0;
                    comboCountdown += Time.deltaTime;
                    attackCount = 0;
                    comboCountdown = 0;
                    comboCountdown += Time.deltaTime;
                    attack(6);
                }
                isBlocking = false;
            }
        }
        #endregion
        
        if (_controller.isGrounded && !isDashing)
        {
            jumpCount = 0;
            useAirDash = false;
            airDashCount = 0;
            isDiving = false;
            doubleJumpDelayTimer = 0f;
        }

        

        #region combat

        if (Input.GetButtonDown("Attack"))
        {
            if (_controller.isGrounded)
            {
                if (moveDir.y > 0)
                {
                    _animator.Play(Animator.StringToHash("Uppercut"));
                    attackCount = 1;
                    comboCountdown = 0;
                    comboCountdown += Time.deltaTime;
                    attack(5);
                    normalizedHorizontalSpeed = 0;
                }
                else if (moveDir.y < 0)
                {
                    _animator.Play(Animator.StringToHash("Sweep"));
                    attackCount = 1;
                    comboCountdown = 0;
                    comboCountdown += Time.deltaTime;
                    attack(4);
                    normalizedHorizontalSpeed = 0;
                }
                else if (comboCountdown == 0 || comboCountdown > ButtonDelay)
                {
                    ph.isBlocking = false;
                   /** if (normalizedHorizontalSpeed != 0)
                    {
                        _animator.Play(Animator.StringToHash("ShoulderCharge"));
                        comboCountdown = 0;
                        comboCountdown += Time.deltaTime;
                        attackCount = 0;
                        comboCountdown = 0;
                        comboCountdown += Time.deltaTime;
                        attack(6);
                        normalizedHorizontalSpeed = .8f * transform.localScale.x;
                       // attack(1);
                    }
                    else
                    {
                    **/
                    if (attackCount == 0 || attackCount == 3)
                        {
                            _animator.Play(Animator.StringToHash("Jab"));
                            attackCount = 1;
                            comboCountdown = 0;
                            comboCountdown += Time.deltaTime;
                            attack(0);
                            normalizedHorizontalSpeed = 0;
                        }
                        else if (attackCount == 1)
                        {
                            _animator.Play(Animator.StringToHash("Cross"));
                            attackCount = 2;
                            comboCountdown = 0;
                            comboCountdown += Time.deltaTime;
                            attack(1);
                            normalizedHorizontalSpeed = 0;

                        }
                        else if (attackCount == 2)
                        {
                            _animator.Play(Animator.StringToHash("Kick"));
                            attackCount = 0;
                            comboCountdown = 0;
                            comboCountdown += Time.deltaTime;
                            attack(2);
                            normalizedHorizontalSpeed = 1 * transform.localScale.x;
                        }
                    }
                //}
            }
        }

        #region Block
        if (Input.GetButton("DiveBlock")) {
            if(!_controller.isGrounded){
            isDiving = true;
            isDashing = false;
            }
            else{
                if (!ph.isBlocking && comboCountdown < 0.25f)
                {
                    _animator.Play(Animator.StringToHash("Block"));
                    ph.isBlocking = true;
                }
            }
        }
        else
        {
            ph.isBlocking = false;
        }
        #endregion
        #endregion

        #region Movement Animation
        if (_controller.isGrounded && normalizedHorizontalSpeed != 0 && !ph.isBlocking &&  !isDashing && (comboCountdown == 0 || comboCountdown > ButtonDelay))
        {
            _animator.Play(Animator.StringToHash("Run"));
        }
        if (_controller.isGrounded && normalizedHorizontalSpeed == 0 && !ph.isBlocking && !isDashing && (comboCountdown == 0 || comboCountdown > ButtonDelay) && !ph.isBlocking)
        {
            _animator.Play(Animator.StringToHash("Idle"));
        }
        #endregion



        // apply horizontal speed smoothing it
        var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?

        if (normalizedHorizontalSpeed != 0 && pm.mode.Equals("speed"))
        {
            normalizedHorizontalSpeed = moveDir.x * pm.speed;
        }
        if (!ph.isBlocking)
        {
            if (!isDashing && !isDiving)
            {
                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);
                // we can only jump whilst grounded
                if (!_controller.isGrounded && Input.GetButtonDown("Jump") && jumpCount < 1)
                {
                    _velocity.y = Mathf.Sqrt(doubleJumpHeight * jumpHeight * -gravity);
                    jumpCount++;
                    _animator.Play(Animator.StringToHash("TienAirKick"));
                }
                else if ((_controller.isGrounded) && Input.GetButtonDown("Jump"))
                {
                    _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                    _animator.StopPlayback();
                    doubleJumpDelayTimer += Time.deltaTime;
                    if (!isDashing)
                    {
                        _animator.Play(Animator.StringToHash("Jump"));
                    }
                }
                _velocity.y += gravity * Time.deltaTime;
            }
            else if (isDashing)
            {
                if (_controller.isGrounded)
                {
                    if(left)
                        _velocity.x = Mathf.Lerp(runSpeed * dashBoost * -1, runSpeed * dashBoost * -1, Time.deltaTime);
                    else
                        _velocity.x = Mathf.Lerp(runSpeed * dashBoost , runSpeed * dashBoost, Time.deltaTime);
                }
                else
                {
                    if(left)
                        _velocity.x = Mathf.Lerp(runSpeed * airDashBoost * -1, runSpeed * airDashBoost * -1, Time.deltaTime);
                    else
                        _velocity.x = Mathf.Lerp(runSpeed * airDashBoost, moveDir.x * runSpeed * airDashBoost, Time.deltaTime);
                }
                if (Input.GetButtonDown("Jump"))
                {
                    if (_controller.isGrounded)
                    {
                        jumpCount++;
                        _velocity.y = Mathf.Sqrt(jumpHeight * -gravity) + (-gravity * Time.deltaTime);
                        //  myBoxCollider.size = new Vector3(10f, 1.75f);
                    }
                    else
                    {
                        if (_velocity.y > 0)
                        {
                            _velocity.y = Mathf.Sqrt(jumpHeight * -gravity) + (-gravity * Time.deltaTime);
                            _animator.Play(Animator.StringToHash("Jump"));
                        }
                        else
                        {
                            _velocity.y = -gravity * Time.deltaTime;
                        }
                    }
                }
            }
            else if (isDiving)
            {
        //        _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * 10f, Time.deltaTime);
                _velocity.y = gravity * 50f * Time.deltaTime;
            }
            _controller.move(_velocity * Time.deltaTime);
        }
    }

    public void attack(int attackName)
    {
        atkController.attack(attackName);
        //        PlayerAttack attack = (PlayerAttack)attackBox.GetComponent<PlayerAttack>();
        //      attack.setAttackDamage(attackDamage);
        //    PlayerAttack attackClone = (PlayerAttack)Instantiate(attack, new Vector3(transform.position.x + (6f * transform.localScale.x), transform.position.y + 5.5f, transform.position.z), transform.rotation);
    }

    public void updateTimers()
    {

        if (doubleJumpDelayTimer > 0)
        {
            doubleJumpDelayTimer += Time.deltaTime;
        }

        if (isDashing)
        {
            airDashTime += Time.deltaTime;
        }
        if (airDashTime > airDashDuration)
        {
            // myBoxCollider.size = new Vector3(1.75f, 10);
            isDashing = false;
            airDashTime = 0;
        }

        if (shotTime != 0)
        {
            shotTime += Time.deltaTime;
        }

        if (shotTime > .3f)
        {
            shotTime = 0;
        }


        if (comboCountdown > comboTime)
        {
            attackCount = 0;
            comboCountdown = 0;
        }

        if (comboCountdown != 0)
        {
            comboCountdown += Time.deltaTime;
        }

    }

    public bool isLeft()
    {
        return left;
    }

}
