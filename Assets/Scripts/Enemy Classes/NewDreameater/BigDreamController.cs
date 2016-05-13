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

   public float biteDuration = 1.2f;
    public float flipDuration = 1.2f;
    public float quickAtkDuration;
    public float screechDuration;
    public float waitTimer = 1f;

    public float attackDuration;

    private int attackRng;
    private int prevAttack;

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
        if (attackDuration > 0)
        {
            attackDuration -= Time.deltaTime;
        }

        if (attackDuration <= 0)
        {
            attackDuration = 0;
        }

    }

    public override void updateCanAttack(string attackName, bool canUse)
    {
        if (myHealth.currentHealth > 0)
        {
            if(attackDuration == 0)
            {
                attackRng = Random.Range(0, 4);
                if(prevAttack == attackRng)
                {
                    attackRng = Random.Range(0, 4);
                }
                switch (attackRng)
                {
                    case 0:
                        attackDuration = biteDuration + waitTimer;
                        _animator.Play(Animator.StringToHash("Bite"));
                        myAttacks[0].myBoxSwitch(true);
                        break;
                    case 1:
                        attackDuration = flipDuration + waitTimer;
                        _animator.Play(Animator.StringToHash("Flip"));
                        myAttacks[1].myBoxSwitch(true);
                        break;
                    case 2:
                        attackDuration = screechDuration + waitTimer;
                        _animator.Play(Animator.StringToHash("Screech"));
                        myAttacks[2].myBoxSwitch(true);
                        break;
                    case 3:
                        attackDuration = quickAtkDuration + waitTimer;
                        _animator.Play(Animator.StringToHash("QuickBite"));
                        myAttacks[3].myBoxSwitch(true);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
