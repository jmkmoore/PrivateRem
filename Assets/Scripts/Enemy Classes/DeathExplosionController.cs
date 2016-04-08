using UnityEngine;
using System.Collections;

public class DeathExplosionController : MonoBehaviour {

    public float explosionLifetime = 1;
    private float lifeTimer;

	// Use this for initialization
	void Start () {
        lifeTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > explosionLifetime)
        {
            Destroy(gameObject);
        }
	}
}
