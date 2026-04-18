using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyMovement : NetworkBehaviour
{
    private EnemyStats stats;
    private Transform curTarget;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        stats = GetComponent<EnemyStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        stats = GetComponent<EnemyStats>();
    }

    void Update()
    {
        if (!IsServer) 
            return;

        FindNearestPlayerServerRPC();

        if (!curTarget)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position,
            curTarget.position, Time.deltaTime * stats.CurSpeed);

        bool needToFlip = curTarget.position.x < transform.position.x;
        ChangeSpriteFlipClientRPC(needToFlip);
    }

    [ClientRpc]
    private void ChangeSpriteFlipClientRPC(bool needToFlip)
    {
        spriteRenderer.flipX = needToFlip;  
    }

    [ServerRpc(RequireOwnership = false)]
    public void FindNearestPlayerServerRPC()
    {
        var players = NetworkManager.Singleton.ConnectedClients;

        float minRange = float.MaxValue;
        foreach (var player in players)
        {
            if (Vector3.Distance(transform.position,
                player.Value.PlayerObject.transform.position) < minRange)
            {
                curTarget = player.Value.PlayerObject.transform;
                minRange = Vector3.Distance(transform.position,
                    curTarget.position);
            }
        }
    }
}
