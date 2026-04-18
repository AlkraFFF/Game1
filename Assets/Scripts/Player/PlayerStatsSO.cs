using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Vampire survivors/Player Stats", fileName = "PlayerStats")]
public class PlayerStatsSO : ScriptableObject
{
    public float MaxHealth;
    public float Recovery;
    public float MaxSpeed;
    public float MagnetRadius;
}
