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
    private float attackTimer = 0f;


    private Transform previousTransform;

    private GameObject attackChild;

    private float turnTime;
    private int turnChance;
    private Vector3 forcedMovement;
    private EnemyHealth myHealth;
    private EnemyAttack myAttack;

    public float forcedMoveSpeedMultiplier;

    public string enemyType;
    public bool isBoss;

    void Awake()
    {
        _animator = GetComponent<Animator>();
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
        if (col.name.Equals("TienHitBox"))
        {
          //  Debug.Log("onTriggerEnterEvent: " + col.gameObject.name);
            updateAttack(true);
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

        if (_controller.isGrounded)
        {
            turnTime += Time.deltaTime;
        }

        if (attackTimer > 0) 
            attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            attackTimer = 0;
        }
        _velocity = _controller.velocity;

        #region Spider
        if (enemyType.Equals("Spider"))
        {
            if (_controller.isGrounded)
            {
                turnTime += Time.deltaTime;
            }
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
                    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);
                }
                else
                {
                    _velocity.x = 0;
                }
            }
        }
        #endregion

        #region Doll
        if (enemyType.Equals("Doll"))
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
                    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);
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
                    _animator.Play(Animator.StringToHash("Trip"));
                    _velocity.x = 0;
                    myAttack.myBoxSwitch(true);
                }

            }
        }
        #endregion
        #region Colossus
        if (enemyType.Equals("BigGuy"))
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
                _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime);
            }
        }



        #endregion


        if (myHealth.getInvulnState())
        {
            _animator.Play("HitReaction");
            if (!isBoss)
            {
                if (_velocity.y <= 0)
                {
                    _velocity.y = forcedMovement.y;
                }
                else
                {
                    _velocity.y += forcedMovement.y + (-gravity * Time.deltaTime);
                }
                _velocity.x = Mathf.Lerp(forcedMovement.x * forcedMoveSpeedMultiplier, forcedMovement.x * normalizedHorizontalSpeed, Time.deltaTime);
            }
        }
        else
        {
            _velocity.y += gravity * Time.deltaTime;
        }
        _controller.move(_velocity * Time.deltaTime);
    }

    public void updateAttack(bool doAttack)
    {
        inRange = doAttack;
    }

    public void updateDirection()
    {
        left = !left;
    }

    public void getKnockedBack(Vector3 receivedMovement)
    {
        forcedMovement = receivedMovement;
    }

    

}
