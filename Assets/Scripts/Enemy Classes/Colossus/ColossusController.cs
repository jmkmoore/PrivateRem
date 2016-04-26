using UnityEngine;
using System.Collections;
using Prime31;

public class ColossusController : EnemyController {

    public float slamDuration = 0f;
    public float shoulderDuration = 0f;
    public float stompDuration = 0f;
    public float jumpDuration = 0f;
    public float jumpSpeed = 15f;
    public float jumpStrength = 10f;
    public float attackWaitTime = 0f;
    public float attackCooldown = 0f;
    
    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private EnemyMovement _movement;
    private Vector3 _velocity;
    private float normalizedHorizontalSpeed = 0;

    private EnemyHealth myHealth;
    public EnemyAttack[] myAttacks;
    private bool isAttacking = false;
    public int attackRng = 4;
    private int previousRng = 4;

    // Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _movement = GetComponent<EnemyMovement>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;
        myAttacks = GetComponentsInChildren<EnemyAttack>();
        myHealth = GetComponent<EnemyHealth>();
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
    }


    void onTriggerExitEvent(Collider2D col)
    {
    }

    #endregion


    public override void updateTimers()
    {
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        if (attackCooldown < 0f)
        {
            attackCooldown = 0f;
        }
        if (attackCooldown < attackWaitTime)
        {
            isAttacking = false;
            _movement.isAttacking = false;
        }
    }

	// Update is called once per frame
    void FixedUpdate()
    {
        _velocity = _controller.velocity;
        updateTimers();

        #region attackLogic
        if (myHealth.currentHealth > 0)
        {
            if (attackCooldown == 0)
            {
                if (isInRange())
                {
                    _velocity.x = 0;
                    attackRng = Random.Range(0, 3);
                    if (attackRng == previousRng)
                        attackRng = Random.Range(0, 3);

                    if (attackRng == 0)
                    {
                        _animator.Play(Animator.StringToHash("Slam"));
                        attackCooldown = slamDuration + attackWaitTime;
                    }
                    else if (attackRng == 1)
                    {
                        _animator.Play(Animator.StringToHash("StompTransition"));
                        attackCooldown = stompDuration + attackWaitTime;
                    }
                    else if (attackRng == 2)
                    {
                        _animator.Play(Animator.StringToHash("ShoulderTransition"));
                        attackCooldown = shoulderDuration + attackWaitTime;
                    }
                    isAttacking = true;
                    myAttacks[attackRng].myBoxSwitch(true);
                    _movement.isAttacking = true;
                }
            }
        #endregion
            if (!isAttacking)
            {
                if (_velocity.x != 0f)
                    _animator.Play(Animator.StringToHash("Walk"));
                else
                    _animator.Play(Animator.StringToHash("ColossusIdle"));
            }
            if (!isInRange() && !isAttacking)
            {
                if (_controller.isGrounded)
                {
                    if (_movement.left)
                    {
                        _velocity.x = Mathf.Lerp(_velocity.x, -1 * _movement.runSpeed, Time.deltaTime);
                    }
                    else
                    {
                        _velocity.x = Mathf.Lerp(_velocity.x, _movement.runSpeed, Time.deltaTime);
                    }
                }
            }
            _velocity.y += _movement.gravity * Time.deltaTime;
            _controller.move(_velocity * Time.deltaTime);        
        }
    }

    private bool canAttack()
    {
        return isInRange() && attackCooldown == 0f;
    }

    public override bool isInRange()
    {
        return _movement.inRange;
    }

    public override void updateCanAttack(string name, bool yn)
    {
    }

}
