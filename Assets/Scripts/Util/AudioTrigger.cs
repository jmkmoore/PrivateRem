using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour {

    private bool played = false;
    public bool playEachTime;
    private AudioSource mySource;
    public AudioClip soundClip;

	// Use this for initialization
	void Start () {
	
	}
	
    void OnTriggerEnter2d(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if (!played)
            {
                mySource.PlayOneShot(soundClip);
                if (!playEachTime)
                {
                    played = true;
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
