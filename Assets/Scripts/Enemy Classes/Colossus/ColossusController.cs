using UnityEngine;
using System.Collections;
using Prime31;

public class ColossusController : EnemyController {

    
    #region has hoe timer;
    public float overhandCooldown = 8f;
    public float pokeCooldown = 3f;
    public float swingCooldown = 6f;

    public float overhandTimer = 0f;
    public float pokeTimer = 0f;
    public float swingTimer = 0f;

    public bool canOverHand = false;
    public bool canPoke = false;
    public bool canSwing = false;

    public float attackCooldown = 0f;
#endregion


    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private EnemyMovement _movement;
    private Vector3 _velocity;

    public float turnTimer = 0f;
    public float turnCooldown = 5f;

    public float currentAttackTimer = 0;
    public string currentAttack = "idle";

    // Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();
        _movement = GetComponent<EnemyMovement>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        currentAttack = "idle";
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
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            if (attackCooldown < 0)
            {
                attackCooldown = 0;
            }
        }

        if (currentAttackTimer != 0)
        {
            currentAttackTimer += Time.deltaTime;
            if (currentAttackTimer > 1.1f)
            {
                _movement.stopToAttack(false);
            }
        }

        if (overhandTimer != 0)
        {
            overhandTimer += Time.deltaTime;
            if (overhandTimer > overhandCooldown)
            {
                overhandTimer = 0;
            }
        }

        if (swingTimer != 0)
        {
            swingTimer += Time.deltaTime;
            if (swingTimer > swingCooldown)
            {
                swingTimer = 0;
            }
        }

        if (pokeTimer != 0)
        {
            pokeTimer += Time.deltaTime;
            if (pokeTimer > pokeCooldown)
            {
                pokeTimer = 0;
            }
        }

        if (pokeTimer != 0 && overhandTimer != 0 && swingTimer != 0)
        {
            _movement.stopToAttack(false);
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        _velocity = _controller.velocity;
        updateTimers();

        #region attackLogic
        if (attackCooldown == 0)
        {
            if (isInRange())
            {
                if (canOverHand && overhandTimer == 0)
                {
                    overheadAttack();
                    _movement.stopToAttack(true);
                    attackCooldown = 1.5f;
                }
                else if (canSwing && swingTimer == 0)
                {
                    swingAttack();
                    attackCooldown = 2f;
                    _movement.stopToAttack(true);
                }
                else if (canPoke && pokeTimer == 0)
                {
                    pokeAttack();
                    attackCooldown = 1.0f;
                    _movement.stopToAttack(true);
                }
            }
            else
            {
                _movement.stopToAttack(false);
            }
        }    
        #endregion

        #region movement animations
       if (_velocity.x != 0)
        {
            _animator.Play(Animator.StringToHash("Walk"));
        }
        else if(currentAttack.Equals("idle"))
        {
            _animator.Play(Animator.StringToHash("ColossusIdle"));
        }
        
        #endregion
    }

    #region Attack methods
    void overheadAttack()
    {
        _animator.Play(Animator.StringToHash("Overhand"));
        overhandTimer += Time.deltaTime;
        currentAttack = "overhand";
    }

    void swingAttack()
    {
        Debug.Log("Called swing");
        _animator.Play(Animator.StringToHash("ColossusSwing"));
        swingTimer += Time.deltaTime;
        currentAttack = "swing";
    }

    void pokeAttack()
    {
        _animator.Play(Animator.StringToHash("Poke"));
        pokeTimer += Time.deltaTime;
        currentAttack = "poke";
    }

    public override void updateCanAttack(string attackName, bool canUse)
    {
        switch (attackName){
        case"overhand":    
            canOverHand = canUse;
            break;
        case "poke":
            canPoke = canUse;
            break;
        case "swing":
            canSwing = canUse;
            break;
        default:
            break;
              
        }
    }
    #endregion

    public override bool isInRange()
    {
        return canOverHand || canPoke || canSwing;
    }

    public int getCurrentAttackValue()
    {
        switch (currentAttack)
        {
            case "swing":
                return 30;
            case "overhand":
                return 20;
            case "poke":
                return 10;
            case "idle":
                return 0;
            default:
                return 0;
        }
    }
}
