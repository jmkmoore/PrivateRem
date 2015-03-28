using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
	public float projectileSpeed = 15;
	bool fireRight = true;
	public int bulletDamage;
    public float direction;
    public string myType;

    public float timeAlive;
    public float MaxTime; 

	// Use this for initialization
	void Start ()
	{
			transform.localEulerAngles = new Vector3 (0, 0, 0);
            Transform parentTrans = transform.parent;
            timeAlive = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		float movementAmt = projectileSpeed * Time.deltaTime;

		transform.Translate (direction * Vector3.right * movementAmt);
        timeAlive += Time.deltaTime;
        if (timeAlive > MaxTime)
        {
            DestroyObject(gameObject);
        }
	}
    
	void OnTriggerEnter2D (Collider2D other)
	{
        Debug.Log("Collided with " + other.name);
		if (other.CompareTag ("Enemy")) {
            EnemyHealth eh = findEnemyHealth(other.gameObject);
		eh.adjustCurrentHealth(-bulletDamage);
        DestroyObject(gameObject);
		}
        else if (!myType.Equals("Player") && other.CompareTag("Player"))
        {
            DestroyObject(gameObject);
            GameObject go = other.gameObject.transform.parent.gameObject;
            PlayerHealth ph = (PlayerHealth)go.GetComponent ("PlayerHealth");
            ph.adjustCurrentHealth(-bulletDamage);
                
        }
        else
        {
            DestroyObject(gameObject);
        }
	}

    public void setDirection(float directionValue)
    {
       direction = directionValue;
    }
    public EnemyHealth findEnemyHealth(GameObject obj)
    {
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

