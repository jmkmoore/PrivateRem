using UnityEngine;
using System.Collections;
using Prime31;

public class PlayerAttack : MonoBehaviour {
	public int attackValue = 10;
	public GameObject target;
    private EnemyMovement enemyController;
    private BoxCollider2D myBox;

    public float attackKnockbackX = 1000f;
    public float attackKnockbackY = 1000f;
    public float lifetime, maxDur;
    
    public GameObject StrikeParticle;

    private bool on;
    public GameObject myParticle;

    private Vector3 attackKnockback;
    private PlayerHealth ph;

	// Use this for initialization
	void Start () {
        if (myParticle != null)
        {
            myParticle.SetActive(false);
        }
        ph = transform.gameObject.GetComponentInParent<PlayerHealth>();
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

            if (myParticle != null)
                myParticle.SetActive(false);
        }
    }

	void Attack(GameObject target){
        EnemyHealth eh = findEnemyHealth(target);
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
            if (StrikeParticle != null)
            {
                Instantiate(StrikeParticle, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y), new Quaternion(0, 0, 0, 0));
            }
            eh.adjustCurrentHealth(-attackValue);
            enemyController = target.transform.parent.GetComponent<EnemyMovement>();
            enemyController.getKnockedBack(thisKnockback);
            ph.fillShield(attackValue);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.transform.name);
        if (other.tag.Equals("Enemy"))
        {
            Attack(other.transform.gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
      //  Debug.Log(other.name + other.tag);
      //  if (other.tag.Equals("Enemy"))
      //  {
      //      Attack(other.transform.gameObject);
      //  }
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
        if (myParticle != null)
            myParticle.SetActive(true);
    }
}
