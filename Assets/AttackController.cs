using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour {
    public PlayerAttack[] attacks;

    private BoxCollider2D myActiveBox;

	// Use this for initialization
	void Start () {
        attacks = GetComponentsInChildren<PlayerAttack>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void turnOnBox(int boxNum)
    {
        attacks[boxNum].turnOnAttack();
    }

    private void turnOffBox()
    {
    }

    public void attack(int attackNum)
    {
        turnOnBox(attackNum);
    }
}
