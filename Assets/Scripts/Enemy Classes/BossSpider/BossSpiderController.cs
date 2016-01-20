using UnityEngine;
using System.Collections;
using Prime31;

public class BossSpiderController : MonoBehaviour {

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

    private bool canSwipe;
    private bool canBite;
    private bool canLeap;


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

        myHealth = (EnemyHealth)gameObject.GetComponent<EnemyHealth>();
    }

    void FixedUpdate()
    {
        updateTimers();

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
        else
        {
            em.stopToAttack(false);
        }

    }

	
	// Update is called once per frame
	void Update ()  {
	
	}

    void updateTimers()
    {
        if (currentAttackTimer != 0)
        {
            currentAttackTimer -= Time.deltaTime;
        }

        if (_velocity.x != 0 && _controller.isGrounded)
        {
            _animator.Play(Animator.StringToHash("Walk"));
        }

    }

    #region Attack
    public void updateCanAttack(string attackName, bool canUse){
        switch (attackName){
        case"swipe":
            currentAttackTimer = swipeCooldown;
            break;
        case "bite":
            currentAttackTimer = biteCooldown;
            break;
        case "leap":
            currentAttackTimer = leapCooldown;
            break;
        default:
            break;
              
        }
    }
    #endregion

    bool isInRange()
    {
        return canBite || canLeap || canSwipe;
    }

}
