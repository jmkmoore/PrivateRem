using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour {

    public BoxCollider2D HighKick;

    private BoxCollider2D myActiveBox;
    public float boxTimer = 0f;
    private float boxLifetime = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (myActiveBox != null)
        {
            boxTimer += Time.deltaTime;
        }

        if (boxTimer > boxLifetime)
        {
            myActiveBox = null;
            boxTimer = 0;
            turnOffBox();
        }
	
	}

    public void turnOnBox()
    {
        if (boxTimer == 0)
        {
            if (HighKick.enabled)
            {
                HighKick.enabled = false;
            }
            else
            {
                HighKick.enabled = true;
                myActiveBox = HighKick;
            }
        }
    }

    private void turnOffBox()
    {
        HighKick.enabled = false;
    }
}
