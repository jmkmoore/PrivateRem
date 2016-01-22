using UnityEngine;
using System.Collections;

public abstract class EnemyController : MonoBehaviour {

    public abstract bool isInRange();

    public abstract void updateTimers();

    public abstract void updateCanAttack(string attackName, bool canAttack);

}
