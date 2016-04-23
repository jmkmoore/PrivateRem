using UnityEngine;
using System.Collections;
using Prime31;

public class EnemyAttack : MonoBehaviour {
    public GameObject target;
    private CharacterController2D enemyController;
    private BoxCollider2D myBox;
    public int attackValue = 10;
    public float attackKnockbackX = 1000f;
    public float attackKnockbackY = 1000f;
    private Vector3 attackKnockback;
    public float frames, activeStart, maxDur;
    private bool on;
    public AudioSource mySource;
    public AudioClip startUpSound;
    public AudioClip connectedSound;

    // Use this for initialization
	void Start () {
        mySource = GetComponentInParent<AudioSource>();
        attackKnockback.x = attackKnockbackX;
        attackKnockback.y = attackKnockbackY * Time.deltaTime;
        myBox = gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (frames != 0)
        {
            frames += Time.deltaTime;
        }
        if(frames > activeStart && frames < maxDur)
        {
            myBox.enabled = true;
        }
        if (frames > maxDur)
        {
            myBox.enabled = false;
            frames = 0;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            target = other.gameObject;
            Attack();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
 //       Debug.Log(other.name + other.tag);

        if (other.tag.Equals("Player"))
        {
            if (frames > activeStart && frames < maxDur)
            {
                Attack();
            }
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
            ph.adjustCurrentHealth(-attackValue);
            if (mySource != null && connectedSound != null)
            {
                mySource.PlayOneShot(connectedSound);
            }
        }
    //    Vector3 thisKnockback = attackKnockback;
  //      if (gameObject.GetComponentInParent<EnemyMovement>().left)
    //    {
     //       thisKnockback.x = attackKnockback.x * -1f;
      //  }
       // else
       // {
       //     thisKnockback.x = Mathf.Abs(attackKnockback.x);
        //}
        //    enemyController = target.transform.parent.GetComponent<CharacterController2D>();
          //  enemyController.move(thisKnockback);

    }

    public void myBoxSwitch(bool onOrOff)
    {
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
}
