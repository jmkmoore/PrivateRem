using UnityEngine;
using System.Collections;

public class DamageZone : MonoBehaviour {

	// Use this for initialization
    private PlayerHealth ph;
    public int myTickDamage;
    
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name + " entered");
        if (other.name.Equals("TienHitBox"))
        {
            other.GetComponentInParent<PlayerHealth>().adjustCurrentHealth(-myTickDamage);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            other.GetComponentInParent<PlayerHealth>().adjustCurrentHealth(-myTickDamage);
        }

    }

}
