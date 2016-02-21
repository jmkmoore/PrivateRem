using UnityEngine;
using System.Collections;

public class TellEnemyTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            Debug.Log(other.name);
            other.gameObject.GetComponentInParent<EnemyMovement>().turnAround();
        }
    }
}
