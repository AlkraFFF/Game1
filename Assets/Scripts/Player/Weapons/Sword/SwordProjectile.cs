using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SwordProjectile : NetworkBehaviour
{
    private Vector3 direction;
    private float destroyCooldown;
    private float speed;
    private float damage;
    private int pierceCount;

    public void Setup(WeaponsStatsSO stats, float projectileSpeed, int pierce)
    {
        destroyCooldown = stats.DestroyDelay;
        damage = stats.Damage;
        speed = projectileSpeed;
        pierceCount = pierce;
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;

        float dirX = direction.x;
        float dirY = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirX < 0 && dirY == 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY < 0)
        {
            scale.y = scale.y * -1;
        }
        else if (dirX == 0 && dirY > 0)
        {
            scale.x = scale.x * -1;
        }
        else if (dirX > 0 && dirY > 0)
        {
            rotation.z = 0f;
        }
        else if (dirX > 0 && dirY < 0)
        {
            rotation.z = -90f;
        }
        else if (dirX < 0 && dirY > 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -90f;
        }
        else if (dirX < 0 && dirY < 0)
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 0f;
        }

        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
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

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsServer)
                return;
        if (collision.TryGetComponent(out EnemyStats enemyStats))
        {
            enemyStats.TakeDamage(damage);
            ReducePierce();
        }
    }

    private void ReducePierce()
    {
        pierceCount--;
        if (pierceCount <= 0)
        {
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
