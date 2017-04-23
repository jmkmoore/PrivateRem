using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackController : MonoBehaviour {
    private Dictionary<string, PlayerAttack> myAttackBoxes;
    private BoxCollider2D myActiveBox;
    
	// Use this for initialization
	void Start () {
        myAttackBoxes = new Dictionary<string, PlayerAttack>();
        PlayerAttack[] attacks = GetComponentsInChildren<PlayerAttack>();
        foreach (PlayerAttack attack in attacks)
        {
            myAttackBoxes.Add(attack.name, attack);
        }
    }

    private void turnOffBox()
    {
    }

    public void attack(string attackName)
    {
        PlayerAttack attack = null;
        if (myAttackBoxes.TryGetValue(attackName, out attack))
        {
            attack.turnOnAttack();
        }

    }
}
