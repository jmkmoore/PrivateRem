using UnityEngine;
using System.Collections;

public class EnemyMode : MonoBehaviour {

    public bool isInvuln;
    public bool alwaysInvuln = false;

    private float invulnTimer = 0f;
    private float invulnDuration = 1f;

	// Use this for initialization
	void Start () {
        isInvuln = alwaysInvuln;
	}
	
	// Update is called once per frame
	void Update () {
        if(!alwaysInvuln){
            if (isInvuln)
            {
                invulnTimer += Time.deltaTime;
            }
            if (invulnTimer > invulnDuration)
            {
                invulnTimer = 0f;
                isInvuln = false;
            }
        }
        else
        {
            isInvuln = alwaysInvuln;
        }
	}

    public void makeInvuln()
    {
        isInvuln = true;
    }
}
