using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerCollector : NetworkBehaviour
{
    private PlayerStats playerStats;
    private CircleCollider2D magnetCollider;

    private void Start()
    {
        playerStats = GetComponentInParent<PlayerStats>();
        magnetCollider = GetComponent<CircleCollider2D>();

        magnetCollider.radius = playerStats.CurMagnetRadius;

        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsOwner) 
            return;
        if (collision.TryGetComponent(out Collectable collectable))
        {
            collectable.Collect(gameObject);
        }
    }
}
