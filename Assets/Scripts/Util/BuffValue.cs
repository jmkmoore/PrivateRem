using UnityEngine;
using System.Collections;

public class BuffValue : MonoBehaviour {

    public int maxHealth = 50;
    public int HealthRestore;
    public string myBuff;

    bool type = true;

	// Use this for initialization
	void Start () {
        HealthRestore = maxHealth;
	}
	
	// Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other){
    }
}
