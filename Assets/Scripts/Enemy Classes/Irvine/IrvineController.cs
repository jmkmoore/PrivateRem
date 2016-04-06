using UnityEngine;
using System.Collections;
using Prime31;

public class IrvineController : MonoBehaviour {
    #region Cooldowns
    public float slamCooldown = 8f;
    public float spitCooldown = 5f;
    public float throwCooldown = 6f;
    public float childCooldown = 10f;


    public float slamTimer = 0f;
    public float spitTimer = 0f;
    public float childTimer = 1f;
    public float throwTimer = 0f;
    #endregion

    #region Movement Values
    private CharacterController2D _controller;
    private Animator _animator;
    private RaycastHit2D _lastControllerColliderHit;
    private Vector3 _velocity;
    #endregion

    public GameObject spawner1;
    public GameObject spawner2;
    public GameObject spawner3;

    private EnemyHealth myHealth;

    public int stage = 1;
    public float currentAttackTimer = 0;
    public string currentAttack = "idle";
    public bool transitioned = false;

    public GameObject myFireball;
    private EnemyMovement em;

    public bool fireOne = false;
    public bool thrownSeed = false;
    public bool spawnChild = false;

    public float fireSpitDeg;
    public float fireSpitDif;

    private GameObject log;
    private GameObject mySolidBox;


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
	void Start () {
        em = GetComponent<EnemyMovement>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController2D>();

        // listen to some events for illustration purposes
        _controller.onControllerCollidedEvent += onControllerCollider;
        _controller.onTriggerEnterEvent += onTriggerEnterEvent;
        _controller.onTriggerExitEvent += onTriggerExitEvent;

        currentAttack = "idle";

        myHealth = (EnemyHealth)gameObject.GetComponent<EnemyHealth>();
        log = GameObject.FindWithTag("Log");
        mySolidBox = GameObject.FindWithTag("Box");
	}

    void FixedUpdate()
    {
        updateTimers();
    }
	
	// Update is called once per frame
    void Update()
    {
        if (em.isVisible)
        {      
            #region Stage triggering
            if (currentAttackTimer == 0)
            {
                if (myHealth.currentHealth < myHealth.maxHealth * (4.0 / 5))
                {
                    stage = 2;
                }

                if (myHealth.currentHealth < myHealth.maxHealth * (3.0 / 5))
                {
                    stage = 3;
                }

                if (myHealth.currentHealth < myHealth.maxHealth * (2.0 / 5))
                {
                    stage = 4;
                }

                if (myHealth.currentHealth <= 0)
                {
                    stage = 5;
                }
            }
            #endregion
            
            #region Attacks
            if (currentAttackTimer != 0)
            {
                currentAttackTimer += Time.deltaTime;
            }

            if (currentAttack.Equals("spit") && currentAttackTimer > 1.2f && !fireOne)
            {
                fireSeedsOne();
                fireOne = true;
            }

            if (currentAttack.Equals("child") && currentAttackTimer > 1.5f && !spawnChild)
            {
                raiseChild();
            }

            if (currentAttack.Equals("seed") && currentAttackTimer > 2f && !thrownSeed)
            {
                throwSeed();
                thrownSeed = true;
            }

            if ((currentAttackTimer > 2.5f && currentAttack.Equals("slam")) || (currentAttackTimer > 3f && currentAttack.Equals("spit")) || (currentAttackTimer > 3f && currentAttack.Equals("seed")) || currentAttackTimer == 0 || (currentAttackTimer > 3f && currentAttack.Equals("child")))
            {
                if (childTimer == 0)
                {
                    currentAttackTimer = 0;
                    currentAttackTimer += Time.deltaTime;
                    childAttack();
                }else if (slamTimer == 0)
                {
                    currentAttackTimer = 0;
                    SlamAttack();
                    currentAttackTimer += Time.deltaTime;
                }
                else if (spitTimer == 0)
                {
                    currentAttackTimer = 0;
                    spitStage1Attack();
                    currentAttackTimer += Time.deltaTime;
                    fireOne = false;
                }
                else if (throwTimer == 0)
                {
                    currentAttackTimer = 0;
                    seedAttack();
                    currentAttackTimer += Time.deltaTime;
                }
            }
            #endregion

            #region Idle
            if (currentAttackTimer > 4f || currentAttackTimer == 0)
            {
                if (stage == 1)
                {
                    _animator.Play(Animator.StringToHash("Idle"));
                }
                else if (stage == 2)
                {
                    _animator.Play(Animator.StringToHash("Idle2"));

                }
                else if (stage == 3)
                {
                    _animator.Play(Animator.StringToHash("Idle3"));

                }
                else if (stage == 4)
                {
                    _animator.Play(Animator.StringToHash("Idle4"));
                }
            }
            if (stage == 5)
            {
                _animator.Play(Animator.StringToHash("Death"));
                mySolidBox.SetActive(false);
            }
            #endregion
        }

    }

    void spitStage1Attack()
    {
        _animator.Play(Animator.StringToHash("FireSpit1"));
        spitTimer += Time.deltaTime;
        currentAttack = "spit";
    }

    void childAttack()
    {
        _animator.Play(Animator.StringToHash("ChildSpawn"));
        childTimer += Time.deltaTime;
        currentAttack = "child";
    }

    void SlamAttack()
    {
        _animator.Play(Animator.StringToHash("IrvineSlam"));
        slamTimer += Time.deltaTime;
        currentAttack = "slam";
    }

    void seedAttack()
    {
        _animator.Play(Animator.StringToHash("SeedThrow"));
        throwTimer += Time.deltaTime;
        currentAttack = "seed";
    }

    void fireSeedsOne()
    {
        spitStage1Attack();
        FireSeed newSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        newSeed.setRoller(false);
        newSeed.setAngle(fireSpitDeg * Mathf.Deg2Rad);
        FireSeed floater = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
        newSeed.setAngle((fireSpitDeg - fireSpitDif) * Mathf.Deg2Rad);
       FireSeed floater2 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
        newSeed.setAngle((fireSpitDeg + fireSpitDif) * Mathf.Deg2Rad);
        FireSeed floater3 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
    }

    void throwSeed()
    {
        FireSeed throwSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        throwSeed.setAngle(fireSpitDeg * Mathf.Deg2Rad);
        throwSeed.setRoller(true);
        FireSeed roller = (FireSeed)Instantiate(throwSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
    }

    void updateTimers()
    {
        if (slamTimer != 0)
        {
            slamTimer += Time.deltaTime;
            if (spitTimer != 0)
                spitTimer += Time.deltaTime;
        }
        
        if (spitTimer > spitCooldown)
        {
            spitTimer = 0;
            if (slamTimer > slamCooldown)
                slamTimer = 0;
        }

        if(throwTimer != 0){
            throwTimer += Time.deltaTime;
            if (throwTimer > throwCooldown)
            {
                throwTimer = 0;
                thrownSeed = false;
            }
        }

        
        if (childTimer != 0)
        {
            childTimer += Time.deltaTime;
            if (childTimer > childCooldown)
            {
                spawnChild = false;
                childTimer = 0;
            }
        }
    }

    void raiseChild()
    {
        spawner1.GetComponent<SpawnEnemy>().spawnEnemy();
        spawner2.GetComponent<SpawnEnemy>().spawnEnemy();
        spawner3.GetComponent<SpawnEnemy>().spawnEnemy();
        spawnChild = true;
    }

}
