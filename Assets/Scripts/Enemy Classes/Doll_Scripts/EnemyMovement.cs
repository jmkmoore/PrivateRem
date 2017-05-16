using UnityEngine;
using System.Collections;
using Prime31;

public class EnemyMovement : MonoBehaviour {
    public float gravity = -25f;
    public float runSpeed = 8f;
    public float groundDamping = 20f; // how fast do we change direction? higher means faster
    public float inAirDamping = 5f;

    [HideInInspector]
    private float normalizedHorizontalSpeed = 0;

    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;

    public bool left;

    public bool inRange = false;
    private int direction = 1;

    public float attackCooldown = 4f;
    public float attackTimer = 0f;
    public float attackDuration = 1f;

    #region Turning chances
    private int turn;
    private float turnTime;
    private int turnChance;
    private float turnTimer = 0f;
    public float turnCooldown = 2f;
    #endregion

    public Vector3 forcedMovement;
    private EnemyHealth myHealth;
    private EnemyAttack myAttack;

    public float forcedMoveSpeedMultiplier;

    public string enemyType;
    public bool isBoss;
    public bool isAttacking;

    public bool isVisible;
    private Renderer renderer;

    public float fuseLength;
    public float myTimer;
    public float myDeathTimer;
    private bool isSelfDestroying = false;

    private float idleDuration = 0f;
    public float idleTime = 0.5f;

    public float knockbackMultiplier;

    private bool canSeeTien;
    public float increasedSpeedFactor;
    public float idleTimer;

    public bool suffersKnockback;
    public bool vulnerable;
    public float vulnerableTimer;
    private float currentVulnTimer;

    void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        myHealth = gameObject.GetComponent<EnemyHealth>();
        myAttack = gameObject.GetComponentInChildren<EnemyAttack>();
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
        if (col.gameObject.layer == 21)
        {
            turnAround();
        }

        if (col.name.Equals("Wall"))
        {
            updateDirection();
        }
    }


    void onTriggerExitEvent(Collider2D col)
    {
        if (col.name.Equals("TienHitBox"))
        {
            updateAttack(false);
        }
      //  Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
    }

    #endregion
	
	// Update is called once per frame
    void FixedUpdate()
    {
        isVisible = renderer.isVisible;
        _velocity = _controller.velocity;
        updateTimers();
        if (isVisible)
        {
            if (!myHealth.getInvulnState())
            {
                #region GooSpider
                if (enemyType.Equals("Goo"))
                {
                    if (attackTimer == 0)
                    {
                        if (_controller.isGrounded && !inRange)
                        {
                            if (_controller.collisionState.becameGroundedThisFrame && !canSeeTien)
                            {
                                idleTimer = idleTime;
                                _velocity.x = 0;
                                turn = Random.Range(0, 1000);
                                if (turn < 75)
                                {
                                    left = !left;
                                }
                            }
                            if (left)
                            {
                                normalizedHorizontalSpeed = -1;
                                if (transform.localScale.x < 0f)
                                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                            }
                            else
                            {
                                normalizedHorizontalSpeed = 1;
                                if (transform.localScale.x > 0f)
                                    transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                            }
                            if (idleTimer == 0)
                            {
                                _velocity.y = Mathf.Sqrt(-gravity);
                                _animator.StopPlayback();
                                _animator.Play(Animator.StringToHash("Hop"));
                            }
                        }
                        else if (_controller.isGrounded && inRange && attackTimer == 0)
                        {
                            _velocity.x = 0;
                            _animator.Play(Animator.StringToHash("Attack"));
                            myAttack.myBoxSwitch(true);
                            attackTimer = 1f;
                        }
                        else if (!_controller.isGrounded)
                        {
                            if (canSeeTien)
                            {
                                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * increasedSpeedFactor, Time.deltaTime);
                            }
                            else
                            {
                                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);

                            }
                        }
                    }
                }

                #endregion

                #region Pumpkin
                if (enemyType.Equals("Pumpkin"))
                {
                    if (_controller.isGrounded)
                    {
                        myTimer += Time.deltaTime;
                    }
                    if (myTimer < fuseLength)
                    {
                        if (_controller.isGrounded)
                        {
                            if (left)
                            {
                                normalizedHorizontalSpeed = -1;
                                if (transform.localScale.x > 0f)
                                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                            }
                            else
                            {
                                normalizedHorizontalSpeed = 1;
                                if (transform.localScale.x < 0f)
                                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                            }
                            _animator.Play(Animator.StringToHash("Walk"));
                            if (canSeeTien)
                            {
                                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * increasedSpeedFactor, Time.deltaTime);
                            }
                            else
                            {
                                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);

                            }
                        }
                        else
                        {
                            _velocity.x = 0;
                        }
                    }
                    else
                    {
                        _velocity.x = 0;
                        if (!isSelfDestroying)
                        {
                            _animator.Play(Animator.StringToHash("Explode"));
                            myAttack.myBoxSwitch(true);
                            isSelfDestroying = true;
                        }
                    }
                    if (myTimer > myDeathTimer)
                    {
                        myHealth.explode();
                    }
                }
                #endregion

                #region Spider
                if (enemyType.Equals("Spider"))
                {
                    if (!inRange && attackTimer < 3f)
                    {
                        if (_controller.isGrounded)
                        {
                            if (left)
                            {
                                normalizedHorizontalSpeed = -1;
                                if (transform.localScale.x > 0f)
                                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                            }
                            else
                            {
                                normalizedHorizontalSpeed = 1;
                                if (transform.localScale.x < 0f)
                                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                            }
                            _animator.Play(Animator.StringToHash("Walk"));
                            if (canSeeTien)
                            {
                                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * increasedSpeedFactor, Time.deltaTime);
                            }
                            else
                            {
                                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);

                            }
                        }
                        else
                        {
                            _velocity.x = 0;
                        }
                    }
                    else
                    {
                        if (attackTimer == 0)
                        {
                            attackTimer = attackCooldown;
                            _animator.StopPlayback();
                            _animator.Play(Animator.StringToHash("Bite"));
                            _velocity.x = 0;
                            myAttack.myBoxSwitch(true);
                        }

                    }
                }
                #endregion

                #region Doll
                if (enemyType.Equals("Doll"))
                {
                    if(attackTimer == 0f){
                        if(_controller.isGrounded && inRange){
                            attackTimer = attackCooldown + attackDuration;
                            _animator.Play(Animator.StringToHash("Trip"));
                            _velocity.x = 0;
                            myAttack.myBoxSwitch(true);
                        }
                        else{
                            if (_controller.isGrounded){
                                if (left)
                                {
                                    normalizedHorizontalSpeed = -1;
                                    if (transform.localScale.x > 0f)
                                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                                }
                                else
                                {
                                    normalizedHorizontalSpeed = 1;
                                    if (transform.localScale.x < 0f)
                                        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                                }
                                _animator.Play(Animator.StringToHash("Walk"));
                                if (canSeeTien)
                                {
                                    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * increasedSpeedFactor, Time.deltaTime);
                                }
                                else
                                {
                                    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);

                                }
                            }
                        }
                    }
                    }
                #endregion

                #region Turtle
                if (enemyType.Equals("Turtle"))
                {
                    if (_controller.isGrounded && !inRange && attackTimer == 0)
                    {
                        if (left)
                        {
                            normalizedHorizontalSpeed = -1;
                            if (transform.localScale.x < 0f)
                                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                        }
                        else
                        {
                            normalizedHorizontalSpeed = 1;
                            if (transform.localScale.x > 0f)
                                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
                        }
                        if (_controller.collisionState.becameGroundedThisFrame)
                        {
                            _velocity.x = 0;
                            _animator.Play(Animator.StringToHash("Idle"));
                            idleDuration += Time.deltaTime;
                        }
                        else if (_controller.isGrounded && idleTime < idleDuration)
                        {
                            turn = Random.Range(0, 1000);
                            if (turn < 75)
                            {
                                left = !left;
                            }
                            _velocity.y = Mathf.Sqrt(runSpeed * -gravity);
                            _animator.StopPlayback();
                            _animator.Play(Animator.StringToHash("Hop"));
                            idleDuration = 0f;
                        }
                    }
                    else if (_controller.isGrounded && inRange && attackTimer == 0)
                    {
                        _velocity.x = 0;
                        _animator.Play(Animator.StringToHash("Attack"));
                        myAttack.myBoxSwitch(true);
                        attackTimer = attackCooldown;
                    }
                    else if (!_controller.isGrounded)
                    {
                        if (canSeeTien)
                        {
                            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed * increasedSpeedFactor, Time.deltaTime);
                        }
                        else
                        {
                            _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);

                        }
                    }
                }
                #endregion
            }

            #region BossMovement
            if (enemyType.Equals("BigGuy") || enemyType.Equals("BossSpider") || enemyType.Equals("Plant"))
            {
                if (myHealth.currentHealth > 0)
                {
                    if (!isAttacking)
                    {
                        if (left)
                        {
                            normalizedHorizontalSpeed = -1;
                            if (transform.localScale.x > 0f)
                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        }
                        else
                        {
                            normalizedHorizontalSpeed = 1;
                            if (transform.localScale.x < 0f)
                                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                        }
                    }
                }
            }
            #endregion


            if (suffersKnockback && vulnerable)
            {
                    if (_controller.isGrounded || _velocity.y <= 0)
                    {
                        _velocity.y = forcedMovement.y;
                    }
                    else
                    {
                        _velocity.y += forcedMovement.y * knockbackMultiplier * Time.deltaTime;
                    }
                _velocity.x = forcedMovement.x * knockbackMultiplier * Time.deltaTime;
            }
            if (currentVulnTimer != 0)
            {
                _velocity.y += (gravity/2) * Time.deltaTime;
            }
            else
            {
                _velocity.y += gravity * Time.deltaTime;
            }
            _controller.move(_velocity * Time.deltaTime);
        }
    }

    public void updateAttack(bool doAttack)
    {
        inRange = doAttack;
    }

    public void updateDirection()
    {
        left = !left;
        
    }

    public void startDirection(bool spawnLeft)
    {
        left = spawnLeft;
    }

    public void getKnockedBack(Vector3 receivedMovement)
    {
        forcedMovement = receivedMovement;
    }

    public void stopToAttack(bool attacking)
    {
        this.isAttacking = attacking;
    }

    void updateTimers()
    {
        if (vulnerable)
        {
            currentVulnTimer += Time.deltaTime;
            if (currentVulnTimer > vulnerableTimer)
            {
                vulnerable = false;
                currentVulnTimer = 0;
            }
        }

        if(idleDuration != 0){
            idleDuration += Time.deltaTime;
        }

        if (turnTimer != 0)
        {
            turnTimer += Time.deltaTime;
        }

        if (turnTimer > turnCooldown)
        {
            turnTimer = 0;
        }

        if (attackTimer != 0)
        {
            attackTimer -= Time.deltaTime;
            if (attackTimer < 0)
            {
                attackTimer = 0;
            }
        }

        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer < 0)
            {
                idleTimer = 0;
            }
        }
    }

    public void turnAround()
    {
        if (turnTimer == 0)
        {
            updateDirection();
            turnTimer += Time.deltaTime;
        }
    }

    public void beAggressive(bool inSight)
    {
        canSeeTien = inSight;
    }

    public float getRunSpeed()
    {
        if (canSeeTien)
        {
            return runSpeed * increasedSpeedFactor;
        }
        else
            return runSpeed;
    }

    public void setVulnerable(bool isVulnerable)
    {
        Debug.Log("blah");
        if (isVulnerable)
        {
            if (suffersKnockback)
            {
                if (_animator.HasState(0, Animator.StringToHash("HitReaction")))
                {
                    _animator.Play(Animator.StringToHash("HitReaction"));
                }
            }
            currentVulnTimer = 0;
            currentVulnTimer += Time.deltaTime;
        }
        vulnerable = isVulnerable;
    }
}
