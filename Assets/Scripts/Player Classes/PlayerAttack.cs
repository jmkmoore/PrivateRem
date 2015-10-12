using UnityEngine;
using System.Collections;
using Prime31;

public class PlayerAttack : MonoBehaviour {
	public int attackValue = 10;
	private int superAttackValue = 2;
	public GameObject target;
    private CharacterController2D enemyController;
    private BoxCollider2D myBox;


    public float attackKnockbackX = 1000f;
    public float attackKnockbackY = 1000f;
    public float lifetime, maxDur;

    private bool on;

    private Vector3 attackKnockback;
	// Use this for initialization
	void Start () {
        attackKnockback.x = attackKnockbackX;
        attackKnockback.y = attackKnockbackY * Time.deltaTime;
        myBox = gameObject.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (myBox.enabled == true)
        {
            lifetime += Time.deltaTime;
        }        
        if (lifetime > maxDur)
        {
            lifetime = 0;
            myBox.enabled = false;
        }
    }

	void Attack(GameObject target){
        EnemyHealth eh = findEnemyHealth(target);
        float enemyGravity = target.transform.parent.GetComponent<Gravity>().gravityValue;
        Vector3 thisKnockback = attackKnockback;
        if (gameObject.GetComponentInParent<DemoScene>().isLeft())
        {
            thisKnockback.x = attackKnockback.x * -1f;
        }
        else
        {
            thisKnockback.x = Mathf.Abs(attackKnockback.x);
        }
        if (eh != null)
        {
            eh.adjustCurrentHealth(-attackValue);
            
            enemyController = target.transform.parent.GetComponent<CharacterController2D>();
            if (enemyGravity == 0f)
            {
                thisKnockback.x = 0;
                thisKnockback.y = 0;
            }
            enemyController.move(thisKnockback);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            Attack(other.transform.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name + other.tag);
        if (other.tag.Equals("Enemy"))
        {
            Attack(other.transform.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }

    public void setAttackDamage(int Damage)
    {
        attackValue = Damage;
    }

    public EnemyHealth findEnemyHealth(GameObject obj){
        EnemyHealth eh = (EnemyHealth)obj.transform.GetComponent<EnemyHealth>();
        if (eh == null)
        {
            eh = obj.transform.parent.GetComponent<EnemyHealth>();
        }
        if (eh == null)
        {
            eh = obj.transform.parent.parent.GetComponent<EnemyHealth>();
        }
        return eh;
    }

    public void resetTimers()
    {
        lifetime = 0f;
    }

    public void turnOnAttack()
    {
        lifetime = 0f;
        myBox.enabled = true;
    }
}
