﻿using UnityEngine;
using System.Collections;

public class PassThroughPlat : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!transform.parent.GetComponent<Player>().GetGrounded() && other.GetComponent<Collider2D>().gameObject.layer == LayerMask.NameToLayer("OnewayPass"))
        {
          Physics2D.IgnoreLayerCollision(9, 8, true);
        }
    }
}
