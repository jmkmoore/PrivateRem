using UnityEngine;
using System.Collections;
using Prime31;

public class EnemyAttack : MonoBehaviour {
    public GameObject target;
    public int attackValue = 10;
    public float attackKnockbackX = 50f;
    public float attackKnockbackY = 50f;
    public float frames, activeStart, maxDur;
    public AudioClip startUpSound;
    public AudioClip connectedSound;
    public bool interruptable;
    public int currentInterrupt, interruptLimit;

    private bool hit;
    private CharacterController2D myController;
    private BoxCollider2D myBox;
    private AudioSource mySource;
    private bool on;
    private Vector3 attackKnockback;
    private EnemyMovement myMovement;

    // Use this for initialization
	void Start () {
        mySource = GetComponentInParent<AudioSource>();
        attackKnockback.x = attackKnockbackX;
        attackKnockback.y = attackKnockbackY;
        myBox = gameObject.GetComponent<BoxCollider2D>();
        myController = GetComponentInParent<CharacterController2D>();
        myMovement = GetComponentInParent<EnemyMovement>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (interruptable && currentInterrupt >= interruptLimit)
        {
            resetInterruptDamage();
            myMovement.setVulnerable(true);
            myBox.enabled = false;
            frames = 0;
        }
        if (frames != 0)
        {
            frames += Time.deltaTime;
        }
        if (frames > activeStart)
        {
            myBox.enabled = true;
        }
        if (frames > maxDur)
        {
            myBox.enabled = false;
            frames = 0;
        }
	}

    void Update(){

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            target = other.gameObject;
            Attack();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            target = null;
        }
    }

		
	void Attack(){
        if (target != null)
        {
            PlayerHealth ph = (PlayerHealth)target.GetComponentInParent<PlayerHealth>();
            hit = ph.adjustCurrentHealth(-attackValue);
            if (hit)
                if(mySource != null && connectedSound != null)
            {
                mySource.PlayOneShot(connectedSound);
            }
        }
    }

    public void myBoxSwitch(bool onOrOff)
    {
        frames = 0f;
        if (mySource != null && startUpSound != null)
        {
            mySource.PlayOneShot(startUpSound);
        }
        frames += Time.deltaTime;
    }

    public void inRange(bool inRange)
    {
        on = inRange;
    }

    public void setAttackValue(int damage)
    {
        attackValue = damage;
        frames = 0;
        frames += Time.deltaTime;
    }

    public void addInterruptDamage(int damage)
    {
        currentInterrupt += Mathf.Abs(damage);
    }

    public void resetInterruptDamage()
    {
        currentInterrupt = 0;
    }
}
