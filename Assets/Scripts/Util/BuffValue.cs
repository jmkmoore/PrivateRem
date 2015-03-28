using UnityEngine;
using System.Collections;

public class BuffValue : MonoBehaviour {

    public int maxHealth = 50;
    public int HealthRestore;
    public string myBuff;

    bool type = true;

	// Use this for initialization
	void Start () {
        HealthRestore = maxHealth;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (HealthRestore == maxHealth)
        {
            type = !type;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Enemy"))
        {
            Buffed buff = (Buffed)other.GetComponent<Buffed>();
            buff.isBuffed = true;            
        }
        if (other.tag.Equals("Player"))
        {
            PlayerHealth ph = (PlayerHealth)other.transform.parent.GetComponent<PlayerHealth>();
            int missing = ph.maxHealth - ph.currentHealth;
            int restore = HealthRestore - (HealthRestore - missing);
            ph.adjustCurrentHealth(restore);
            HealthRestore = HealthRestore - restore;

           // PlayerMode pm = (PlayerMode)other.transform.parent.GetComponent<PlayerMode>();
           // if (pm.mode != "normal")
           // {
           //     pm.timer = 0;
           // }
           // pm.mode = myBuff;
        }
    }
}
