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
    public float lifetime, maxDur;
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
        if (lifetime > maxDur)
        {
            lifetime = 0;
            myBox.enabled = false;
        }
        if (lifetime > cooldown)
        {
            myBox.enabled = true;
        }
        if (lifetime == 0)
        {
            myBox.enabled = true;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("a;slkdjfasdf");
            target = other.gameObject;
        }
        if (lifetime == 0)
        {
            Attack();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name + other.tag);
        if (other.tag.Equals("Player"))
        {
            if (lifetime == 0 || lifetime > cooldown)
            {
                Attack();
            }
        }
    }

		
	void Attack(){

		PlayerHealth ph = (PlayerHealth)target.GetComponentInParent<PlayerHealth>();
		ph.adjustCurrentHealth(-attackValue);
        lifetime += Time.deltaTime;
        myBox.enabled = true;

        Vector3 thisKnockback = attackKnockback;
        if (gameObject.GetComponentInParent<DollMovement>().left)
        {
            thisKnockback.x = attackKnockback.x * -1f;
        }
        else
        {
            thisKnockback.x = Mathf.Abs(attackKnockback.x);
        }
            enemyController = target.transform.parent.GetComponent<CharacterController2D>();
            enemyController.move(thisKnockback);

    }

    public void inRange(bool inRange)
    {
        on = inRange;
    }
}
