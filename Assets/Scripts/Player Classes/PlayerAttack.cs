using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {
	public int attackValue = 10;
	private int superAttackValue = 2;
	public GameObject target;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Attack(GameObject target){
        EnemyHealth eh = findEnemyHealth(target);
        if (eh != null)
        {
            eh.adjustCurrentHealth(-attackValue);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        /**Debug.Log(other.transform.name);
        if (other.tag.Equals("Enemy"))
        {
            Attack(other.transform.gameObject);
        }
         **/
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
}
