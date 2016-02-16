using UnityEngine;
using System.Collections;

public class RespawnTracker : MonoBehaviour {

    public Vector3 myCheckpoint;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setCheckpoint(Vector3 newCheckpoint)
    {
        myCheckpoint = newCheckpoint;
    }
}
