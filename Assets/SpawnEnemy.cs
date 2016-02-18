using UnityEngine;
using System.Collections;

public class SpawnEnemy : MonoBehaviour {

    public GameObject enemyToSpawn;
    public bool spawnOnView = false;
    public Renderer renderer;
    public bool wasSeen;
    public bool spawnFacingLeft = true;

    private EnemyController spawnedEm;

    public float spawnCooldown;
    public float spawnTimer;

	// Use this for initialization
	void Start () {
        renderer = GetComponentInChildren<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
        if (spawnTimer != 0)
        {
            spawnTimer += Time.deltaTime;
        }
        if (spawnTimer > spawnCooldown)
        {
            spawnTimer = 0;
        }

        if (spawnOnView)
        {
            if (renderer.isVisible && !wasSeen && spawnTimer == 0)
            {
                wasSeen = true;
                spawnEnemy();
                spawnTimer += Time.deltaTime;
            }
            if (!renderer.isVisible && wasSeen)
            {
                wasSeen = false;
            }
        }
	
	}

    public void spawnEnemy()
    {
        GameObject enemy = enemyToSpawn;
        enemy.GetComponent<EnemyMovement>().startDirection(spawnFacingLeft);
        Instantiate(enemy, gameObject.transform.position, gameObject.transform.rotation);
    }
}
