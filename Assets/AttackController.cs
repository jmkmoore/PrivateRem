using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour {

    public GameObject punchBox;
    public GameObject crosspunchBox;
    public GameObject kickBox;
    public GameObject airKickBox;

    private BoxCollider2D myActiveBox;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void turnOnBox(int boxNum)
    {
        switch (boxNum)
        {
            case 0:
                punchBox.GetComponent<PlayerAttack>().turnOnAttack();
                break;
            case 1:
                crosspunchBox.GetComponent<PlayerAttack>().turnOnAttack();
                break;
            case 2:
                kickBox.GetComponent<PlayerAttack>().turnOnAttack();
                break;
            case 3:
                airKickBox.GetComponent<PlayerAttack>().turnOnAttack();
                break;
            default:
                break;
        }

    }

    private void turnOffBox()
    {
    }

    public void attack(int attackNum)
    {
        turnOnBox(attackNum);
    }
}
