using UnityEngine;
using System.Collections;

public class TurnAround : MonoBehaviour {

    public GameObject myObject;
    public DollMovement myController;

	// Use this for initialization
	void Start () {
        myObject = gameObject.transform.parent.gameObject;
        myController = (DollMovement)myObject.GetComponent<DollMovement>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnColliderExit2D(Collider2D other)
    {
        Debug.Log("Exiting");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Triggered");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exit triggered");
        if (other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            myController.left = !myController.left;
        }
    }
}
