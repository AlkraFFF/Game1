using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class FireShieldWeapon : PlayerWeapon
{
    private List<GameObject> markedEnemies = new List<GameObject>();
    private NetworkVariable<bool> isDisabledNow = new NetworkVariable<bool>();

    protected virtual void Start()
    {
        if (isDisabledNow.Value)
        {
            gameObject.SetActive(false);
        }
           
    }
    public override void OnWeaponCDFinished()
    {
        DisableMelee(false);
    }

    public override void OnWeaponDestroyed()
    {
        markedEnemies.Clear();
        DisableMelee(true);
    }
    private void DisableMelee(bool isDisabled)
    {
        isDisabledNow.Value = isDisabled;
        DisableMeleeClientRpc(isDisabled);
    }

    [ClientRpc]
    private void DisableMeleeClientRpc(bool isDisabled)
    {
        gameObject.SetActive(!isDisabled);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) 
            return;
        if (collision.TryGetComponent(out EnemyStats enemyStats)
            && !markedEnemies.Contains(collision.gameObject))
        {
            enemyStats.TakeDamage(weaponStats.Damage);
            markedEnemies.Add(enemyStats.gameObject);
        }
    }
}
