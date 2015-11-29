using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public GameObject enemyToSpawn;
    public bool spawnOnView = false;
    public Renderer renderer;
    public bool wasSeen;

	// Use this for initialization
	void Start () {
        renderer = GetComponentInChildren<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
        if (spawnOnView)
        {
            if (renderer.isVisible && !wasSeen)
            {
                wasSeen = true;
                spawnEnemy();
            }
            if (!renderer.isVisible && wasSeen)
            {
                wasSeen = false;
            }
        }
	
	}

    public void spawnEnemy()
    {
        Instantiate(enemyToSpawn, gameObject.transform.position, gameObject.transform.rotation);
    }
}
