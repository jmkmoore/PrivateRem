using UnityEngine;
using System.Collections;

public class BossWall : MonoBehaviour {


    public GameObject myBoss;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (myBoss == null)
        {
            Destroy(gameObject);
        }
	
	}
}
