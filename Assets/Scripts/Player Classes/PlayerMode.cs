using UnityEngine;
using System.Collections;

public class PlayerMode : MonoBehaviour {

	public string mode;

    public float buffTime = 5f;
    public float addTimeAmt = 2f;
    public float maxTimeAmt = 10f;
    public float maxSpeedBoost = 1.25f;
    public float speedGrowth = .05f;
    
    private float buffTimer = 0f;
    public float speed = 1f;
    private int resetCount = 0;
    private float buffDelay = 1f;
    private float delayTimer = 0f;

	// Use this for initialization
	void Start () {
	
	}

    void FixedUpdate()
    {
        if (delayTimer > 0)
        {
            delayTimer += Time.deltaTime;
        }
        if (delayTimer > buffDelay)
        {
            emptyCounter();
            delayTimer = 0;
        }
        if (mode.Equals("speed"))
        {
            if (Input.GetAxis("Horizontal") == 0 && buffTimer == 0)
            {
                delayTimer += Time.deltaTime;
            }
            else
            {
                delayTimer = 0;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (buffTimer > 0)
        {
            buffTimer -= Time.deltaTime;
        }
        if (buffTimer < 0)
        {
            speed = 1f;
            mode = "normal";
            buffTimer = 0f;
            resetCount = 0;
        }
        TienGUI.getInstance().LightBar = ((float)buffTimer / (float)buffTime);
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
            Mathf.Clamp(speed, 1.1f, maxSpeedBoost);
        }
        addTime();
        mode = "speed";
        resetCount += 1;
    }

    public void addTime()
    {
        if (buffTimer == 0)
        {
            buffTimer = buffTime;
        }
        else
        {
            buffTimer += addTimeAmt;
            Mathf.Clamp(buffTimer, buffTime, maxTimeAmt);
        }
    }

    public int getResetCount()
    {
        return resetCount;
    }

    public void emptyCounter()
    {
        resetCount = 0;
        speed = 1;
        buffTimer = 0;
        mode = "normal";
    }

    public float getSpeed()
    {
        return speed;
    }
}
