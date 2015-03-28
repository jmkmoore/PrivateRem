using UnityEngine;
using System.Collections;

public class AudioCont : MonoBehaviour {

    public AudioSource tienMain;
    public AudioSource tien2;
    public AudioSource Irvine;

    public AudioSource myAudioSource;

	// Use this for initialization
	void Start () {
        myAudioSource = gameObject.GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.localPosition.x > 2700 && transform.localPosition.x < 5300)
        {
            tien2.Play();
        }
        else if (transform.localPosition.x > 5300)
        {
            Irvine.Play();
        }
	}
}
