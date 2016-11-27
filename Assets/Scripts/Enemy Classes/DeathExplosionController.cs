using UnityEngine;
using System.Collections;

public class DeathExplosionController : MonoBehaviour {

    public float explosionLifetime = 1;
    private float lifeTimer;
    private AudioSource mySource;
    public AudioClip deathSound;

	// Use this for initialization
	void Start () {
        mySource = GetComponent<AudioSource>();
        lifeTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(lifeTimer == 0f)
        {
            mySource.PlayOneShot(deathSound);
        }
        lifeTimer += Time.deltaTime;
        if (lifeTimer > explosionLifetime)
        {
            Destroy(gameObject);
        }
	}
}
