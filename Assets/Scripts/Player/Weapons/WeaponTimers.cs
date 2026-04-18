using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponTimers : NetworkBehaviour
{
    [SerializeField] private WeaponsStatsSO weaponsStats;
    [SerializeField] private PlayerWeapon weapon;

    private void Start()
    {
        if (!IsServer) 
            return;

        weapon.Setup(weaponsStats);
        StartCoroutine(SetTimer(weaponsStats.DestroyDelay));
    }
    private IEnumerator SetTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

        weapon.OnWeaponDestroyed();
        StartCoroutine(SetCooldownTimer(weaponsStats.Cooldown));
    }
    private IEnumerator SetCooldownTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

        weapon.OnWeaponCDFinished();
        StartCoroutine(SetTimer(weaponsStats.DestroyDelay));
    }
}
