using UnityEngine;
using System.Collections;


public class HitParticle : MonoBehaviour {

    private float timer;
    public float lifetime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;
        if (timer > lifetime)
            Destroy(gameObject);

	}
}
