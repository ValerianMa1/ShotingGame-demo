using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Pool[] enemyPools;
    [SerializeField] Pool[] playerProjectilePools;
    [SerializeField] Pool[] enemyProjectilePools;
    [SerializeField] Pool[] VFXPools;
    static Dictionary<GameObject, Pool> dictionary;


    private void Awake() 
    {
        dictionary = new Dictionary<GameObject, Pool>();

        Initialize(enemyPools);
        Initialize(playerProjectilePools);
        Initialize(enemyProjectilePools);    
        Initialize(VFXPools);    
    }


    #if UNITY_EDITOR
    private void OnDestroy() {
        CheckPoolSize(enemyPools);
        CheckPoolSize(playerProjectilePools);
        CheckPoolSize(enemyProjectilePools);
        CheckPoolSize(VFXPools);
    }
    #endif

    void CheckPoolSize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
            if(pool.RuntimeSize > pool.Size)
            {
                Debug.LogWarning(string.Format("Pool:{0}runtimesize{1}大于{2}",pool.Prefab.name,pool.RuntimeSize,pool.Size));
            }
        }
    }

    //初始化所有对象池
    void Initialize(Pool[] pools)
    {
        foreach(var pool in pools)
        {
        #if UNITY_EDITOR//只会在unityeditor中运行编译，在其他平台会直接不执行这段代码，作用仅仅是debug
            if(dictionary.ContainsKey(pool.Prefab))
            {
                //Debug.LogError("Same prefab in multiple pools! Prefab:" + pool.Prefab.name);
                continue;
            }
        #endif

            dictionary.Add(pool.Prefab,pool);
            Transform poolParent = new GameObject("Pool:" + pool.Prefab.name).transform;
            poolParent.parent = transform;

            pool.Initialize(poolParent);
        }
    }


    public static GameObject Release(GameObject prefab)//静态函数中所有引用对象都必须是静态的
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 不道啊 prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].praparedObject();
    }

    public static GameObject Release(GameObject prefab,Vector3 position)//静态函数中所有引用对象都必须是静态的
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 不道啊 prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].praparedObject(position);
    }

    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotation)//静态函数中所有引用对象都必须是静态的
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 不道啊 prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].praparedObject(position,rotation);
    }

    public static GameObject Release(GameObject prefab,Vector3 position,Quaternion rotation,Vector3 localScale)//静态函数中所有引用对象都必须是静态的
    {
        #if UNITY_EDITOR
        if(!dictionary.ContainsKey(prefab))
        {
            Debug.LogError("Pool Manager 不道啊 prefab:" + prefab.name);
            return null;
        }
        #endif
        return dictionary[prefab].praparedObject(position,rotation,localScale);
    }
}



