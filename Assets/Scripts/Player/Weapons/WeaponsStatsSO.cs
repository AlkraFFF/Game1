using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Vampire survivors/WeaponsStat")]
public class WeaponsStatsSO : ScriptableObject
{
    public float Damage;
    public float Cooldown;
    public float DestroyDelay;
}
