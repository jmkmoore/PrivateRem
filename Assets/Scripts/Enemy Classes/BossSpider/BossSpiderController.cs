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
    public string currentAttack = "idle";
    private EnemyMovement em;

    public float waitTime;

    private float swipeDuration;
    private float biteDuration;
    private float leapDuration;

    public bool canSwipe;
    public bool canBite;
    public bool canLeap;

    private float swipeTimer;
    private float biteTimer;
    private float leapTimer;

    private bool isVisible;
    private Renderer renderer;


    public float timer = 0f;

    public float attackDuration = 0;
    public EnemyAttack[] myAttacks;

    public string lastAttack;
    
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
        renderer = GetComponentInChildren<Renderer>();
        em = GetComponent<EnemyMovement>();
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        currentAttack = "idle";
        lastAttack = "idle";

        myAttacks = gameObject.GetComponentsInChildren<EnemyAttack>();

        myHealth = (EnemyHealth)gameObject.GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        isVisible = renderer.isVisible;

        //        em.stopToAttack(true);
        updateTimers();
        if (isVisible)
        {
            if (isInRange() && attackDuration == 0)
            {
                if (canSwipe && !lastAttack.Equals("swipe"))
                {
                    em.stopToAttack(true);
                    attackDuration = 1.5f + waitTime;
                    _animator.Play(Animator.StringToHash("Swipe"));
                    myAttacks[2].myBoxSwitch(true);
                    _velocity.x = 0;
                    lastAttack = "swipe";
                }
                else if (canBite && !lastAttack.Equals("bite"))
                {
                    if (Random.Range(0, 10) < 5)
                    {
                        attackDuration = 3.3f + waitTime;
                        _animator.Play(Animator.StringToHash("BigBite"));
                        myAttacks[0].myBoxSwitch(true);
                        _velocity.x = 0;
                        lastAttack = "bite";
                    }
                    else
                    {
                        attackDuration = 2f + waitTime;
                        _animator.Play(Animator.StringToHash("SmallBite"));
                        myAttacks[1].myBoxSwitch(true);
                        _velocity.x = 0;
                        lastAttack = "bite";
                    }
                    em.stopToAttack(true);
                }
                else if (canLeap)
                {
                    attackDuration = 3f + waitTime;
                    _animator.Play(Animator.StringToHash("Leap"));
                    myAttacks[3].myBoxSwitch(true);
                    em.stopToAttack(true);
                    _velocity.x = 0;
                    lastAttack = "leap";
                }
            }
            else if (!isInRange() && attackDuration == 0)
            {
                if (_controller.isGrounded)
                {
                    if (em.left)
                    {
                        _velocity.x = Mathf.Lerp(_velocity.x, -1 * em.runSpeed, Time.deltaTime);
                    }
                    else
                    {
                        _velocity.x = Mathf.Lerp(_velocity.x, em.runSpeed, Time.deltaTime);
                    }
                }
            }
            _velocity.y += em.gravity * Time.deltaTime;
            _controller.move(_velocity * Time.deltaTime);
        }
    }

    public override void updateTimers()
    {
        if (swipeTimer > 0)
        {
            swipeTimer -= Time.deltaTime;
        }

        if (biteTimer > 0)
            biteTimer -= Time.deltaTime;

        if (leapTimer > 0)
            leapTimer -= Time.deltaTime;

        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
        }

        if (attackDuration < 0)
        {
            currentAttack = "idle";
            em.stopToAttack(false);
            attackDuration = 0;
        }

    }

    #region Attack
    public override void updateCanAttack(string attackName, bool canUse)
    {
        currentAttack = attackName;
        switch (attackName)
        {
            case "swipe":
                canSwipe = canUse;
                break;
            case "bite":
                canBite = canUse;
                break;
            case "leap":
                canLeap = canUse;
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
