using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileOverdrive : PlayerProjectile
{
    [SerializeField] ProjectileGuidanceSystem guidanceSystem;
    protected override void OnEnable()
    {
        
        SetTarget(EnemyManager.instance.RandomEnemy);
        transform.rotation = Quaternion.identity;

        if(target == null) base.OnEnable();
        else 
        {
            //追综目标
            StartCoroutine(guidanceSystem.GuidingCoroutine(target));
        }
    }
}
