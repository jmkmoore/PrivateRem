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
    public float lifetime, maxDur, activeStart;
    public float cooldown;

    private bool on;

    
    // Use this for initialization
	void Start () {
        attackKnockback.x = attackKnockbackX;
        attackKnockback.y = attackKnockbackY * Time.deltaTime;
        myBox = gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if (lifetime != 0)
        {
            lifetime += Time.deltaTime;
        }
        if(lifetime > activeStart && lifetime < maxDur)
        {
            myBox.enabled = true;
        }
        if (lifetime > maxDur)
        {
            myBox.enabled = false;
        }
        if (lifetime > cooldown)
        {
            lifetime = 0;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            target = other.gameObject;
        }
        if (lifetime == 0)
        {
            Attack();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
 //       Debug.Log(other.name + other.tag);
        if (other.tag.Equals("Player"))
        {
            if (lifetime == 0 || lifetime > cooldown)
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
            lifetime += Time.deltaTime;
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
        Debug.Log("Called");
        lifetime += Time.deltaTime;
    }

    public void inRange(bool inRange)
    {
        on = inRange;
    }
}
