using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUp: NetworkBehaviour
{
    [SerializeField] private int expCap;
    [SerializeField] private int expCapIncrease;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private Image expBar;

    private int curExp;
    private int curLevel = 1;

    void Start()
    {
        expText.text = curLevel.ToString();
        //UpdateExpBarServerRpc(curExp, expCap, curLevel);
    }

}  
