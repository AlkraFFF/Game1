using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveWeapon : FireShieldWeapon
{
    [SerializeField] private float maxSize = 4f;

    private float resizeTime;
    private float resizeProgress;

    protected override void Start()
    {
        if (!IsServer)
            return;
        
        base.Start();

        resizeTime = weaponStats.DestroyDelay;
        InitializeResize();
    }

    public override void OnWeaponCDFinished()
    {
        base.OnWeaponCDFinished();
    }

    public override void OnWeaponDestroyed()
    {
        base.OnWeaponDestroyed();
        InitializeResize();
    }

    private void InitializeResize()
    {
        resizeProgress = 0f;
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        if (!IsServer || resizeProgress >= 1f)
            return;
        
        resizeProgress += Time.deltaTime / resizeTime;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * maxSize, resizeProgress);
    }
}
