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
        if (isBlocking)
            blockTimer += Time.deltaTime;
        else
            blockTimer = 0f;

        if (currentHealth > maxHealth)
        {
            currentHealth -= 1;
        }
	}

	public void adjustCurrentHealth(int adj){
        if (isBlocking)
        {
            if (blockTimer < parryTimer){
                currentHealth -= adj;
                adj = 0;
            }
            else
            {
                adj = adj * 1 / 2;
                currentHealth += adj;
            }
        }
        else
        {
            if (adj < 0)
            {
                if (!invuln)
                {
                    currentHealth += adj;
                    invuln = true;
                    invulnTimer += Time.deltaTime;
                }
            }
        }
        if (currentHealth < 1)
            currentHealth = 0;


		//TienGUI.getInstance().LifeBar = ((float)currentHealth / (float)maxHealth);
		}

    public void updateBlocking(bool onOff)
    {
        isBlocking = onOff;
        if (!isBlocking)
        {
            blockTimer = 0f;
        }
    }

    public void resetHP()
    {
        currentHealth = maxHealth;
    }
}
