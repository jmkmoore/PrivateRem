using UnityEngine;
using System.Collections;

public class TellEnemyTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.B + " " + other.tag);
        if (other.tag.Equals("Enemy"))
        {
            other.gameObject.GetComponentInParent<EnemyMovement>().updateDirection();
        }
    }
}
