using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStats : NetworkBehaviour
{
    [SerializeField] private EnemyStatsSO stats;

    public float CurSpeed => curSpeed;
    private float curSpeed;

    private float curHealth;

    public float CurDamage => curDamage;
    private float curDamage;

    [SerializeField] private EnemyDrop enemyDrop;

    void Start()
    {
        enemyDrop = GetComponent<EnemyDrop>();
        if (!IsServer)
        {
            return;
        }

        curSpeed = stats.Speed;
        curHealth = stats.MaxHealth;
        curDamage = stats.Damage;
    }

    
    public void TakeDamage(float damage)
    {
        if (!IsServer)
        {
            return;
        }

        curHealth -= damage;

        if (curHealth <= 0)
        {
            enemyDrop.DropItems();
            GetComponent<NetworkObject>().Despawn(true);
        }
    }
}
