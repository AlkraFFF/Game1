using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SwordWeapon : PlayerWeapon
{
    [SerializeField] private PlayerMove playerMove;
    [SerializeField] private GameObject projectilePrefab;

    [Space]
    [SerializeField] private float projectileSpeed = 2f;
    [SerializeField] private int projectilePierce = 2;

    public override void OnWeaponCDFinished()
    {
        SpawnSwordServerRpc(playerMove.LastMovedVector.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnSwordServerRpc(Vector3 lastMovedVector)
    {
        GameObject spawnedSword = Instantiate(projectilePrefab, transform.position,
            Quaternion.Euler(0, 0, -45f));
        spawnedSword.GetComponent<NetworkObject>().Spawn();
        spawnedSword.GetComponent<SwordProjectile>().SetDirection(lastMovedVector);
        spawnedSword.GetComponent<SwordProjectile>().Setup(weaponStats, projectileSpeed,
            projectilePierce);
    }

    public override void OnWeaponDestroyed()
    {
        
    }
}
