using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
	public int maxHealth = 100;
	public int currentHealth = 100;
	private GameObject myself;

    public float invulnTime = 1f;
    private float invulnTimer = 0f;
    public bool invuln = false;

    public bool isBlocking = false;
    public float blockTimer = 0f;
    public float parryTimer = 0.25f;
    
    // Use this for initialization
	void Start () {
		myself = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (invuln)
        {
            invulnTimer += Time.deltaTime;
        }
        if (invulnTimer > invulnTime)
        {
            invuln = false;
            invulnTimer = 0f;
        }
		
						
	}

	public void adjustCurrentHealth(int adj){
        if (isBlocking)
        {
            if (blockTimer < parryTimer)
                adj = 0;
            else
            {
                adj = adj * 3 / 4;
                currentHealth -= adj;
            }
        }
        else
        {
            if (adj < 0)
            {
                if (!invuln)
                {
                    currentHealth -= adj;
                    invuln = true;
                    invulnTimer += Time.deltaTime;
                }
            }
        }
        if (currentHealth < 1)
            currentHealth = 0;


		TienGUI.getInstance().LifeBar = ((float)currentHealth / (float)maxHealth);
		}

    public void updateBlocking(bool onOff)
    {
        isBlocking = onOff;
        if (!isBlocking)
        {
            blockTimer = 0f;
        }
    }
}
