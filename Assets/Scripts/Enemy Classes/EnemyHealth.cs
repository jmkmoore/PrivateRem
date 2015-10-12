using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {
	public int maxHealth = 40;
	public int currentHealth = 40;
	private Transform myTransform;
    private GameObject[] player;
    private PlayerMode pm;
    // Use this for initialization
	void Start () {
		myTransform = transform;
        player = GameObject.FindGameObjectsWithTag("Player");
        pm = findPlayerMode(player);
	}
	
	// Update is called once per frame
	void Update () {
        if (currentHealth < 0)
        {
            Destroy(gameObject);
        }
	}

    PlayerMode findPlayerMode(GameObject[] playerParts)
    {
        foreach(GameObject obj in playerParts){
            if (obj.GetComponent<PlayerMode>() != null)
            {
                return obj.GetComponent<PlayerMode>();
            }
        }
        return null;
    }
	
	public void adjustCurrentHealth(int adj){
        Debug.Log("Taking " + adj + " damage");
		currentHealth += adj;
		if (currentHealth > maxHealth)
			currentHealth = maxHealth;
		if(currentHealth < 1)
			currentHealth = 0;
		if (currentHealth == 0) {
            pm.resetTimer();
            if (!transform.gameObject.tag.Equals("Boss"))
    			Destroy(gameObject);
		}
	}
}
