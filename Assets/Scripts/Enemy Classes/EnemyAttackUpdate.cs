using UnityEngine;
using System.Collections;

public class EnemyAttackUpdate : MonoBehaviour {

    public string attackName = "";

    private GameObject myParent;
    private BossSpiderController myController;


	// Use this for initialization
	void Start () {
        myParent = myParent = transform.parent.parent.gameObject;
        myController = (BossSpiderController)myParent.GetComponent<BossSpiderController>();

	}

    void Awake()
    {
        myParent = myParent = transform.parent.parent.gameObject;
        myController = (BossSpiderController)myParent.GetComponent<BossSpiderController>();
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

    void OnTriggerStay2D(Collider2D other)
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
