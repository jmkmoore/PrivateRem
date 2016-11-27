using UnityEngine;
using System.Collections;

public class VisionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            transform.parent.GetComponent<EnemyMovement>().beAggressive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            transform.parent.GetComponent<EnemyMovement>().beAggressive(false);
        }
    }

}
