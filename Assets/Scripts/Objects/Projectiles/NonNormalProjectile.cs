﻿using UnityEngine;
using System.Collections;

public class NonNormalProjectile : MonoBehaviour {

	// Use this for initialization
    float projectileSpeed = 15;
    bool fireRight = true;
    public int bulletDamage;
    public float direction;
    public string myType;

    public float timeAlive;
    public float MaxTime;

    public float xAngle;
    public float yAngle;

    // Use this for initialization
    void Start()
    {
        timeAlive = 0;
        if (xAngle < 0)
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    // Update is called once per frame
    void Update()
    {
        float movementAmt = projectileSpeed * Time.deltaTime;

        transform.Translate(direction * (new Vector3(xAngle, yAngle)) * movementAmt);
        timeAlive += Time.deltaTime;
        if (timeAlive > MaxTime)
        {
            DestroyObject(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            DestroyObject(gameObject);
            EnemyHealth eh = (EnemyHealth)other.GetComponent("EnemyHealth");
            eh.adjustCurrentHealth(-bulletDamage);
        }
        else if (other.CompareTag("Player"))
        {
            DestroyObject(gameObject);
            GameObject go = other.gameObject.transform.parent.gameObject;
            PlayerHealth ph = (PlayerHealth)go.GetComponent("PlayerHealth");
            ph.adjustCurrentHealth(-bulletDamage);
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    public void setDirection(float directionValue)
    {
        direction = directionValue;
    }

    public void setAngle(float xVector, float yVector){
        xAngle = xVector;
        yAngle = yVector;
    }
}
