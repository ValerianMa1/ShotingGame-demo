using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGuidanceSystem : MonoBehaviour
{
    [SerializeField] Projectile projectile;
    [SerializeField] float minBallisticAngle = 50f;
    [SerializeField] float maxBallisticAngle = 75f;
    float ballisticAngle;

    Vector3 targetDirection;
    
    public IEnumerator GuidingCoroutine(GameObject target)
    {
        ballisticAngle = UnityEngine.Random.Range(minBallisticAngle,maxBallisticAngle);
        while(gameObject.activeSelf)
        {
            if(target.activeSelf)
            {
                //move to target
                targetDirection = target.transform.position - transform.position;
                //rotate to target
                //双参数反正切函数
                //var angle = Mathf.Atan2(target.transform.position.y,target.transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(targetDirection.y,targetDirection.x) * Mathf.Rad2Deg,Vector3.forward);
                transform.rotation *= Quaternion.Euler(0f,0f,ballisticAngle);
                //move projectile
                projectile.Move();
            }
            else
            {
                //move projectile in move direction
                projectile.Move();
            }
            yield return null;
        }
    }
}
