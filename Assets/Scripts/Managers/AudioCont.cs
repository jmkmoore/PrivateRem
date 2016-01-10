using UnityEngine;
using System.Collections;

public class AudioCont : MonoBehaviour {

 

	// Use this for initialization
	void Start () {
        if (transform.localPosition.x < 2690)
        {
            AkSoundEngine.PostEvent("playOpeningMusic", gameObject);
        }


    }
	
	// Update is called once per frame
	void Update () {

        if (transform.localPosition.x > 2690 && transform.localPosition.x < 2691)
        {
            AkSoundEngine.PostEvent("playMinibossMusic", gameObject);
        }

        if (transform.localPosition.x > 3090 && transform.localPosition.x < 3091)
        {
            AkSoundEngine.PostEvent("continueLevelMusic", gameObject);
        }


        if (transform.localPosition.x > 5300 && transform.localPosition.x < 5301)
        {
            AkSoundEngine.PostEvent("playIrvineMusic", gameObject);
        }


        //else if (transform.localPosition.x > 5300)
        //{
        //    AkSoundEngine.PostEvent("playIrvineMusic", gameObject);
        //}
    }
}
