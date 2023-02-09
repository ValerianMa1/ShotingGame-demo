using System.Collections.Specialized;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoller : MonoBehaviour
{

    [Header("----- Move -----")]
    
    public float MoveSpeed = 6;
    public float MoverotationAngle = 25f;
    public float minFireInterval;
    public float maxFireInterval;
    
    [Header("----- Fire -----")]
    [SerializeField] AudioData[] projectileLaunchSFX;
    public GameObject[] projecties;
    public Transform muzzle; 
    
    
    
    float paddingX;
    float paddingY;


    WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();



    void Awake() 
    {
        var size = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        paddingX = size.x / 2;
        paddingY = size.y / 2;
    }



    private void OnEnable() 
    {
        StartCoroutine(nameof(RandomlyMovingCoroutine)); 
        StartCoroutine(nameof(RandomlyFireCoroutine));   
    }

    private void OnDisable() 
    {
        StopAllCoroutines();    
    }

    
    IEnumerator RandomlyMovingCoroutine()
    {
        transform.position = Viewport.instance.RandomEnemySpawnPosition(paddingX,paddingY);
        Vector3 targetPosition = Viewport.instance.RandomRightHalfPosition(paddingX,paddingY);
        while(gameObject.activeSelf)
        {
            //如果没移动到目标位置就接着移动，移动到了就设定新的目标位置
            if(Vector3.Distance(transform.position,targetPosition) >= MoveSpeed *Time.fixedDeltaTime)//利用distance判断当前位置和目标位置之间的距离是否大于一个极小值
            {
                transform.position = Vector3.MoveTowards(transform.position,targetPosition,MoveSpeed *Time.fixedDeltaTime);
                //旋转角度是当前位置指向目标向量的y轴的值乘以设定旋转角度，第二个参数是旋转轴
                transform.rotation = Quaternion.AngleAxis((targetPosition - transform.position).normalized.y * MoverotationAngle, Vector3.right);
            }
            else
            {
                targetPosition = Viewport.instance.RandomRightHalfPosition(paddingX,paddingY);
            }
            yield return waitForFixedUpdate;
        }
    }
    IEnumerator RandomlyFireCoroutine()
    {
        while(gameObject.activeSelf)
        {
            //随机开火间隔时机
            yield return new WaitForSeconds(Random.Range(minFireInterval,maxFireInterval));
            foreach(var projectile in projecties)
            {
                PoolManager.Release(projectile,muzzle.position);
            }


            AudioManager.Instance.PlayRandomSFX(projectileLaunchSFX);
        }
    }




}
