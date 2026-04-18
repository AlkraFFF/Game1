using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpearWeapon : PlayerWeapon
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fallSpeed;

    public override void OnWeaponCDFinished()
    {
        List<NetworkObject> enemies = EnemySpawner.Instance.ActiveEnemies;

        if (enemies.Count == 0) 
            return;

        SpawnSpearServerRpc(enemies[Random.Range(0, enemies.Count)].transform.position);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnSpearServerRpc(Vector3 enemyPos)
    {
        GameObject spawnedSpear = Instantiate(projectilePrefab, enemyPos + Vector3.up * 3,
            Quaternion.Euler(0, 0, -135f));
        spawnedSpear.GetComponent<NetworkObject>().Spawn();
        spawnedSpear.GetComponent<SpearProjectile>().Setup(weaponStats, fallSpeed);
    }

    public override void OnWeaponDestroyed()
    {

    }
}
