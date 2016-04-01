using UnityEngine;
using System.Collections;

public class AudioCont : MonoBehaviour {


    public AudioClip dialogue;
    public AudioClip demoLevel;
    public AudioClip IrvineBossMusic;

    private string currentSong;

	// Use this for initialization
	void Start () {
        if (transform.localPosition.x < 2690)
        {

        }


    }
	
	// Update is called once per frame
	void Update () {

        if (transform.localPosition.x > 2690 && transform.localPosition.x < 2691)
        {
            

        }

        if (transform.localPosition.x > 3090 && transform.localPosition.x < 3091)
        {

        }


        if (transform.localPosition.x > 5800)
        {

        }


        //else if (transform.localPosition.x > 5300)
        //{
        //    AkSoundEngine.PostEvent("playIrvineMusic", gameObject);
        //}
    }
}
