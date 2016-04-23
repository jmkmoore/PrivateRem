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

    public double maxShield = 50;
    public double currentShield;
    public double shieldRechargeRate;
    public double dashCost;

    public GameObject hitParticle;
    public GameObject parryParticle;
    public GameObject blockParticle;
    private DeathExplosionController block;
    private DeathExplosionController parry;
    private DeathExplosionController hit;

    public AudioClip parrySound;
    public AudioClip blockSound;
    private AudioSource mySource;

    public GameObject shield;
    
    // Use this for initialization
	void Start () {
        mySource = GetComponent<AudioSource>();
        parry = (DeathExplosionController)parryParticle.GetComponent<DeathExplosionController>();
        hit = (DeathExplosionController)hitParticle.GetComponent<DeathExplosionController>();
        block = (DeathExplosionController)hitParticle.GetComponent<DeathExplosionController>();
        myself = gameObject;
        currentShield = maxShield;
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
        {
            blockTimer += Time.deltaTime;
            shield.SetActive(true);
        }
        else
        {
            shield.SetActive(false);
            blockTimer = 0f;
        }

        if (currentHealth > maxHealth)
        {
            currentHealth -= 1;
        }

        if (currentShield < maxShield)
        {
            currentShield += shieldRechargeRate;
            if (currentShield > maxShield)
                currentShield = maxShield;
        }
        TienGUI.getInstance().LifeBar = ((float)currentHealth / (float)maxHealth);
        TienGUI.getInstance().PowerBar = ((float)currentShield / (float)maxShield);
	}

	public void adjustCurrentHealth(int adj){
        double totalDamage = Mathf.Abs(adj);
        double leftOverDamage;
        if (isBlocking)
        {
            if (blockTimer < parryTimer){
                if (parryParticle != null)
                {
                    DeathExplosionController parryPart = (DeathExplosionController)Instantiate(parry, transform.position, transform.rotation);
                }
                if (parrySound != null)
                {
                    mySource.PlayOneShot(parrySound);
                }
                currentHealth -= adj;
                adj = 0;
            }
            else
            {
                totalDamage = totalDamage * 0.5;
                if (totalDamage > currentShield)
                {
                    if (blockParticle != null)
                    {
                        DeathExplosionController blockPart = (DeathExplosionController)Instantiate(block, transform.position, transform.rotation);
                    }
                    if (blockSound != null)
                    {
                        mySource.PlayOneShot(blockSound);
                    }
                    leftOverDamage = (int)(totalDamage - currentShield);
                    currentShield = 0;
                    currentHealth -= (int)leftOverDamage;
                }
                else
                {
                    currentShield -= totalDamage;
                }
            }
        }
        else
        {
            if (!invuln)
            {
                if (totalDamage > currentShield)
                {
                    leftOverDamage = (int)(totalDamage - currentShield);
                    currentShield = 0;
                    currentHealth -= (int)leftOverDamage;
                }
                else
                {
                    currentShield -= totalDamage;
                }
                DeathExplosionController hitPart = (DeathExplosionController)Instantiate(hit, transform.position, transform.rotation);
                invuln = true;
                invulnTimer += Time.deltaTime;
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

    public void resetHP()
    {
        currentHealth = maxHealth;
    }

    public void fillShield(int shieldGain)
    {
        currentShield += shieldGain;
        if (currentShield > maxShield)
            currentShield = maxShield;
    }

    public void dashDrain()
    {
        currentShield -= (int)dashCost;
        if (currentShield < 0)
        {
            currentShield = 0;
        }
    }

    public bool canDash()
    {
        return currentShield >= dashCost;
    }
}
