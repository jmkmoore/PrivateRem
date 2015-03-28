using UnityEngine;
using System.Collections;

public class PlayerMode : MonoBehaviour {

	public string mode;

    public float buffTime = 5f;
    public float timer = 0f;

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
            mode = "normal";
            timer = 0f;
        }
	}

    public void resetTimer()
    {
        timer = 0f;
        mode = "speed";
    }
}
