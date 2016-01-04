using UnityEngine;
using System.Collections;

public class PlayerMode : MonoBehaviour {

	public string mode;

    public float buffTime = 5f;
    public float timer = 0f;
    public float speed = 1f;
    public float resetCount = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!mode.Equals("normal"))
        {
            timer += Time.deltaTime;
        }
        if (timer > buffTime)
        {
            speed = 1f;
            mode = "normal";
            timer = 0f;
            resetCount = 0f;
        }
	}

    public void resetTimer()
    {
        speed += .05f;
        timer = 0f;
        mode = "speed";
        resetCount += 1;
    }

    public float getResetCount()
    {
        return resetCount;
    }
}
