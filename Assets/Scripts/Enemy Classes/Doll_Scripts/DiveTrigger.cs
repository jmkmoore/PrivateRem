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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            transform.parent.GetComponent<EnemyMovement>().updateAttack(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            transform.parent.GetComponent<EnemyMovement>().updateAttack(false);
        }
    }


}
