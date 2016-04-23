using UnityEngine;
using System.Collections;
using Prime31;

public class IrvineController : MonoBehaviour {
    #region Cooldowns
    public float slamLockout = 2.5f;
    public float spitLockout = 3f;
    public float seedLockout = 3f;
    public float childLockout = 3f;
    private float attackLockout = 0f;

    private int attackRng;
    private int prevAttack = 5;
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
    public GameObject myFireSeed;
    private EnemyMovement em;

    private bool fireOne = false;
    private bool fireTwo = false;
    private bool fireThree = false;

    public bool thrownSeed = false;
    public bool spawnChild = false;

    public float fireSpitDeg;
    public float fireSpitDif;

    private GameObject log;
    private GameObject mySolidBox;

    public AudioClip FireSpitSound;
    public AudioClip FireThrowSound;
    private AudioSource mySource;


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
        mySource = GetComponent<AudioSource>();
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
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (myHealth.currentHealth <= 0)
        {
            _animator.Play(Animator.StringToHash("Death"));
        }
        else {
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

                if (currentAttack.Equals("spit"))
                {
                    if (currentAttackTimer > 1.2f && !fireOne)
                    {
                        fireOne = fireSeeds(false);
                    }
                    if (currentAttackTimer > 1.7f && !fireTwo)
                    {
                        fireTwo = fireSeeds(true);
                    }

                    if (currentAttackTimer > 2.3f && !fireThree)
                    {
                        fireThree = fireSeeds(true);
                    }
                }

                if (currentAttack.Equals("child") && currentAttackTimer > 1.5f && !spawnChild)
                {
                    raiseChild();
                }

                if (currentAttack.Equals("seed") && currentAttackTimer > 2.2f && !thrownSeed)
                {
                    throwSeed();
                }

                if (currentAttackTimer > attackLockout || currentAttackTimer == 0)
                {
                    attackRng = Random.Range(0, 4);
                    if (attackRng == prevAttack)
                    {
                        attackRng = Random.Range(0, 4);
                    }

                    if(attackRng == 0){ //Child Attack
                        currentAttackTimer = 0;
                        currentAttackTimer += Time.deltaTime;
                        attackLockout = childLockout;
                        childAttack();
                        spawnChild = false;
                    }
                    else if (attackRng == 1) // Slam Attack
                    {
                        currentAttackTimer = 0;
                        SlamAttack();
                        currentAttackTimer += Time.deltaTime;
                        attackLockout = slamLockout;
                    }
                    else if (attackRng == 2) //Spit Attack
                    {
                        currentAttackTimer = 0;
                        currentAttackTimer += Time.deltaTime;
                        spitStage1Attack();
                        fireOne = false;
                        fireTwo = false;
                        fireThree = false;
                        attackLockout = spitLockout;
                    }
                    else if (attackRng == 3) //Seed throw
                    {
                        currentAttackTimer = 0;
                        seedAttack();
                        currentAttackTimer += Time.deltaTime;
                        attackLockout = seedLockout;
                        thrownSeed = false;
                    }
                    prevAttack = attackRng;
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
        currentAttack = "spit";
    }

    void childAttack()
    {
        _animator.Play(Animator.StringToHash("ChildSpawn"));
        currentAttack = "child";
    }

    void SlamAttack()
    {
        _animator.Play(Animator.StringToHash("IrvineSlam"));
        log.GetComponent<EnemyAttack>().myBoxSwitch(true);
        currentAttack = "slam";
    }

    void seedAttack()
    {
        _animator.Play(Animator.StringToHash("SeedThrow"));
        currentAttack = "seed";
    }

    bool fireSeeds(bool randomize)
    {
        float randDeg = 0;
        if (FireSpitSound != null)
        {
            mySource.PlayOneShot(FireSpitSound);
        }
        if (randomize) {
            randDeg = Random.Range(-fireSpitDif/2, fireSpitDif/2);
        }

        spitStage1Attack();
        FireSeed newSeed = (FireSeed)myFireball.GetComponent("FireSeed");
        newSeed.setRoller(false);
        newSeed.setAngle(fireSpitDeg * Mathf.Deg2Rad);
        FireSeed floater = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
        newSeed.setAngle((fireSpitDeg - fireSpitDif + randDeg) * Mathf.Deg2Rad);
        FireSeed floater2 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
        newSeed.setAngle((fireSpitDeg + fireSpitDif + randDeg) * Mathf.Deg2Rad);
        FireSeed floater3 = (FireSeed)Instantiate(newSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
        return true;
    }

    void throwSeed()
    {
        if (FireThrowSound != null)
        {
            mySource.PlayOneShot(FireThrowSound);
        }
        FireSeed throwSeed = (FireSeed)myFireSeed.GetComponent("FireSeed");
        throwSeed.setAngle(fireSpitDeg * Mathf.Deg2Rad);
        throwSeed.setRoller(true);
        FireSeed roller = (FireSeed)Instantiate(throwSeed, new Vector3(transform.position.x + (-8f * transform.localScale.x), transform.position.y + 30, transform.position.z), transform.rotation);
        thrownSeed = true;
    }

    void raiseChild()
    {
        spawner1.GetComponent<SpawnEnemy>().spawnEnemy();
        spawner2.GetComponent<SpawnEnemy>().spawnEnemy();
        spawner3.GetComponent<SpawnEnemy>().spawnEnemy();
        spawnChild = true;
    }

}
