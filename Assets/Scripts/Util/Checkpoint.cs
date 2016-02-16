using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

    public Vector3 myPosition;
	// Use this for initialization
	void Start () {
        myPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 getPosition()
    {
        return myPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            Debug.Log("checkpoint reached");
            other.GetComponentInParent<RespawnTracker>().setCheckpoint(myPosition);
        }
    }

}
