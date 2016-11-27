using UnityEngine;
using System.Collections;

public class EnemyAttackUpdate : MonoBehaviour {

    public string attackName = "";

    private GameObject myParent;
    public EnemyController myController;


	// Use this for initialization
	void Start () {
        myParent = transform.parent.gameObject;
	}

    void Awake()
    {
        myParent = transform.parent.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
       	    
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myController.updateCanAttack(attackName, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myController.updateCanAttack(attackName, false);
        }
    }
}
