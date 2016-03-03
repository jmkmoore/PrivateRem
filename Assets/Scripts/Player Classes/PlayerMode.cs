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
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if (timer < 0)
        {
            speed = 1f;
            mode = "normal";
            timer = 0f;
            resetCount = 0f;
        }
        TienGUI.getInstance().LightBar = ((float)timer / (float)buffTime);

	}

    public void resetTimer()
    {
        if (speed == 1)
        {
            speed = 1.1f;
        }
        else
        {
            speed += .05f;
        }
        timer += buffTime;
        mode = "speed";
        resetCount += 1;
    }

    public float getResetCount()
    {
        return resetCount;
    }
}
