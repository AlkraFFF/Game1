using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class PlayerWeapon : NetworkBehaviour
{
    protected WeaponsStatsSO weaponStats;

    public virtual void Setup(WeaponsStatsSO stats)
    {
        weaponStats = stats;
    }

    public abstract void OnWeaponCDFinished();
    public abstract void OnWeaponDestroyed();
}
