using UnityEngine;
using System.Collections;
using Prime31;

public class BossSpiderController : EnemyController {

    #region Movement Values
    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    #endregion
    
    private EnemyHealth myHealth;
    public float currentAttackTimer = 0;
    public string currentAttack = "idle";
    private EnemyMovement em;

    public float swipeCooldown;
    public float biteCooldown;
    public float leapCooldown;

    private float swipeDuration;
    private float biteDuration;
    private float leapDuration;

    public bool canSwipe;
    public bool canBite;
    public bool canLeap;

    private float swipeTimer;
    private float biteTimer;
    private float leapTimer;

    public float timer = 0f;

    private float attackDuration = 0;
    public EnemyAttack[] myAttacks;
    
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
        //Debug.Log(col.name);
        //if (col.gameObject.layer == 19)
        // {
        //     left = !left;
        // }
    }


    void onTriggerExitEvent(Collider2D col)
    {

    }

    #endregion

	// Use this for initialization
    void Start()
    {
        em = GetComponent<EnemyMovement>();
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        currentAttack = "idle";

        myAttacks = gameObject.GetComponentsInChildren<EnemyAttack>();

        myHealth = (EnemyHealth)gameObject.GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        em.stopToAttack(true);
        updateTimers();

        if (attackDuration < 0)
        {
            if (currentAttackTimer == 0)
            {
                if (isInRange())
                {
                    if (canSwipe && !currentAttack.Equals("swipe"))
                    {
                        em.stopToAttack(true);
                    }
                    else if (canBite && !currentAttack.Equals("bite"))
                    {
                        em.stopToAttack(true);
                    }
                    else if (canLeap && !currentAttack.Equals("leap"))
                    {
                        em.stopToAttack(true);
                    }
                }
            }
            else if (currentAttackTimer < 2f)
            {
                if (isInRange())
                {
                    if (canSwipe && !currentAttack.Equals("swipe"))
                    {
                        em.stopToAttack(true);
                    }
                    else if (canBite && !currentAttack.Equals("bite"))
                    {
                        em.stopToAttack(true);
                    }
                    else if (canLeap && !currentAttack.Equals("leap"))
                    {
                        em.stopToAttack(true);
                    }
                }
            }
        }
        else
        {
            em.stopToAttack(false);
        }

    }

    public override void updateTimers()
    {
        if (swipeTimer > 0)
            swipeTimer -= Time.deltaTime;

        if (biteTimer > 0)
            biteTimer -= Time.deltaTime;

        if (leapTimer > 0)
            leapTimer -= Time.deltaTime;

        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
        }

        if(!currentAttack.Equals("idle") || currentAttackTimer > 0)
            currentAttackTimer -= Time.deltaTime;

        if(currentAttackTimer < 0)
            currentAttackTimer = 0;

        if (currentAttackTimer < 3f)
            currentAttack = "idle";

        if (_velocity.x != 0 && _controller.isGrounded)
        {
            _animator.Play(Animator.StringToHash("Walk"));
        }

    }

    #region Attack
    public override void updateCanAttack(string attackName, bool canUse)
    {
        if (currentAttack.Equals("idle"))
        {
            currentAttack = attackName;
        }
        switch (attackName){
        case"swipe":
            currentAttackTimer = swipeCooldown;
            attackDuration = 1.5f;
            _animator.Play(Animator.StringToHash("Swipe"));
            swipeTimer = swipeCooldown;
            myAttacks[2].myBoxSwitch(true);
            break;
        case "bite":
            if(Random.Range(0,1) == 1){
                attackDuration = 2.5f;
                currentAttackTimer = biteCooldown;
                _animator.Play(Animator.StringToHash("BigBite"));
                myAttacks[0].myBoxSwitch(true);
            }else{
                attackDuration = 1.1f;
                currentAttackTimer = biteCooldown;
                _animator.Play(Animator.StringToHash("SmallBite"));
                myAttacks[1].myBoxSwitch(true);
            }
            break;
        case "leap":
            currentAttackTimer = leapCooldown;
            attackDuration = 1f;
            currentAttackTimer = leapCooldown;
            _animator.Play(Animator.StringToHash("Leap"));
            myAttacks[3].myBoxSwitch(true);
            break;
        default:
            break;
        }
    }
    #endregion

    public override bool isInRange()
    {
        return canBite || canLeap || canSwipe;
    }

}
