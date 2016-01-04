using UnityEngine;
using System.Collections;

public class DeathTile : MonoBehaviour {

	void OnTrigger2DEnter(Collider2D other)
	{
		if(other.tag == "Player") other.GetComponent<Player>().KillPlayer();
        Debug.Log(other.name);
    }
}
