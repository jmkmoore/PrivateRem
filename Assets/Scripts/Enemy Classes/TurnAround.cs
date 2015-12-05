using UnityEngine;
using System.Collections;

public class TurnAround : MonoBehaviour {

    private GameObject myObject;
    private EnemyMovement myController;

	// Use this for initialization
	void Start () {
        myObject = gameObject.transform.parent.gameObject;
        myController = (EnemyMovement)myObject.GetComponent<EnemyMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnColliderExit2D(Collider2D other)
    {
        Debug.Log("Exiting");
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == LayerMask.NameToLayer("plat"))
        {
            myController.left = !myController.left;
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            myController.left = !myController.left;
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit triggered");
        if (other.gameObject.layer == LayerMask.NameToLayer("Default") || other.gameObject.layer == LayerMask.NameToLayer("plat"))
        {
            myController.updateDirection();

        }
    }
}
