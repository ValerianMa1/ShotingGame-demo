using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtuoDeactivate : MonoBehaviour
{
    WaitForSeconds waitLifeTime;

    [SerializeField] bool destoryGameObject;
    [SerializeField] float lifeTime = 4f;


    private void Awake() 
    {
        waitLifeTime = new WaitForSeconds(lifeTime);//MonoBehaviour的时间函数中Awake->OnEnable->Reset->Start,这里后面会用到，所以再awake中初始化    
    }


    private void OnEnable() {
        StartCoroutine(DeactivateCoroutine());
    }


    IEnumerator DeactivateCoroutine()
    {
        yield return waitLifeTime;
        if(destoryGameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
