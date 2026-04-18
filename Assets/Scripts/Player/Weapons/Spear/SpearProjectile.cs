using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpearProjectile : NetworkBehaviour
{
    private float destroyCooldown;
    private float damage;
    private float fallspeed;

    public void Setup(WeaponsStatsSO stats, float speed)
    {
        destroyCooldown = stats.DestroyDelay;
        damage = stats.Damage;
        fallspeed = speed;
    }

    private void Update()
    {
        if (!IsServer) 
            return;

        destroyCooldown -= Time.deltaTime;
        if (destroyCooldown <= 0)
        {
            GetComponent<NetworkObject>().Despawn(true);
            return;
        }

        transform.position -= Vector3.up * fallspeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) 
            return;
        if (collision.TryGetComponent(out EnemyStats enemyStats))
        {
            enemyStats.TakeDamage(damage);
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
