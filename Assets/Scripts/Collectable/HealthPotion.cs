using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Collectable
{
    [SerializeField] private float healthToRestore;
    private PlayerStats playerStats;

    public override void Collect(GameObject player)
    {
        base.Collect(player);

        playerStats = player.GetComponentInParent<PlayerStats>();
    }

    protected override void OnCollected()
    {
        if (playerStats)
        {
            playerStats.HealServerRpc(healthToRestore);
        }
    }
}
