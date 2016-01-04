using UnityEngine;
using System.Collections;

public class DiveTrigger : MonoBehaviour {

    private GameObject myParent;
    private EnemyMovement myDollMovement;

    void awake()
    {
        myParent = transform.parent.gameObject;
        EnemyMovement myDollMovement = transform.parent.GetComponent<EnemyMovement>();
    }

	void OnColliderEnter2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myParent.GetComponent<EnemyMovement>().updateAttack(true);
        }
    }

    void OnColliderExit2D(Collider2D other)
    {
        if (other.name.Equals("TienHitBox"))
        {
            myParent.GetComponent<EnemyMovement>().updateAttack(false);
        }
    }


}
