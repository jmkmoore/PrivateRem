using UnityEngine;
using System.Collections;

public class PostBossMusic : MonoBehaviour {
    private AudioSource source;
    public AudioClip clip;
    public GameObject myBoss;
    bool isPlaying = false;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (myBoss == null && !isPlaying)
        {
            isPlaying = true;
            source.PlayOneShot(clip);
        }
	
	}
}
