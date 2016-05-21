using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour {

    private bool played = false;
    public bool playEachTime;
    private AudioSource mySource;
    public AudioClip soundClip;

	// Use this for initialization
	void Start () {
        mySource = GetComponent<AudioSource>();
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.tag.Equals("Player"))
        {
            Debug.Log("Triggered");
            if (!played)
            {
                Debug.Log("played");
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
