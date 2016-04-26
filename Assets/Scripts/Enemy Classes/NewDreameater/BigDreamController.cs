using UnityEngine;
using System.Collections;
using Prime31;

public class BigDreamController : EnemyController {

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

    public GameObject MiniHead;
    public GameObject BiteHead;
    public EnemyAttack[] myAttacks;

    public bool canBite;
    public bool canFlip;

    public float biteTimer;
    public float biteCooldown;

    public float flipTimer;
    public float flipCooldown;

    public float attackDuration;

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
	
	// Update is called once per frame
	void FixedUpdate () {
        updateTimers();
        
        if (myHealth.currentHealth == 0)
        {
            _animator.Play(Animator.StringToHash("Death"));
            myHealth.currentHealth = -1;
        }
	}

    public override bool isInRange()
    {
        return canBite || canFlip;
    }

    public override void updateTimers()
    {
        if (biteTimer > 0)
            biteTimer -= Time.deltaTime;

        if (flipTimer > 0)
            flipTimer -= Time.deltaTime;

        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
        }

        if (attackDuration <= 0)
        {
            currentAttack = "idle";
            attackDuration = 0;
        }

        if (!currentAttack.Equals("idle") || currentAttackTimer > 0)
            currentAttackTimer -= Time.deltaTime;

        if (currentAttackTimer < 0)
            currentAttackTimer = 0;
    }

    #region Attack
    public override void updateCanAttack(string attackName, bool canUse)
    {
        if (myHealth.currentHealth > 0)
        {
            if (currentAttack.Equals("idle") && attackDuration == 0)
            {
                currentAttack = attackName;
                switch (attackName)
                {
                    case "bite":
                        currentAttackTimer = biteCooldown;
                        attackDuration = 1.2f;
                        _animator.Play(Animator.StringToHash("Bite"));
                        biteTimer = biteCooldown;
                        myAttacks[0].myBoxSwitch(true);
                        break;
                    case "flip":
                        currentAttackTimer = flipCooldown;
                        attackDuration = 1.2f;
                        currentAttackTimer = flipCooldown;
                        _animator.Play(Animator.StringToHash("Flip"));
                        myAttacks[1].myBoxSwitch(true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    #endregion

}
