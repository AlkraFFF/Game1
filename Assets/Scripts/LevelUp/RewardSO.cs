using System.Collections;
using System.Collections.Generic;
using Unity.Netcode.Components;
using UnityEngine;

public abstract class RewardSO : ScriptableObject
{
    public Sprite Icon;
    [TextArea] public string Description;

    [Space]
    public float StartModifier;
    public float LevelModifier;
    public float MaxLevel;

    public abstract void OnChoosen();
}  
