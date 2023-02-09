using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Pool//没继承monobehaviour，这样写才能将Pool类中的序列化字段曝露出来
{


    public GameObject Prefab => prefab;

    public int Size => size;
    public int RuntimeSize => queue.Count;

    [SerializeField] int size = 1;
    [SerializeField] GameObject prefab;
    Queue<GameObject> queue;

    Transform parent;


    public void Initialize(Transform parent)
    {
        queue = new Queue<GameObject>();
        this.parent = parent;
        for(var i = 0; i< size; i++)
        {
            queue.Enqueue(Copy());
        }
    }


    GameObject Copy()
    {
        //var变量是弱化变量类型，会根据你的赋值觉得具体的变量类型
        var copy = GameObject.Instantiate(prefab, parent);
        copy.SetActive(false);//只能靠其他脚本来激活
        return copy;
    }


    GameObject AvailableObject()
    {
        GameObject availableObject = null;
        if(queue.Count > 0 && !queue.Peek().activeSelf)//防止返回空值，当队列所有元素都被使用时队列可能是空的
        {
            availableObject = queue.Dequeue();
        }
        else
        {
            availableObject = Copy();
        }

        queue.Enqueue(availableObject);

        return availableObject;

    }

    #region PREPAREDOBJECT
    public GameObject praparedObject()
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        return preparedObject;
    }

    public GameObject praparedObject(Vector3 position)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        return preparedObject;
    }

    public GameObject praparedObject(Vector3 position,Quaternion rotation)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        return preparedObject;
    }

public GameObject praparedObject(Vector3 position,Quaternion rotation,Vector3 localSclale)
    {
        GameObject preparedObject = AvailableObject();
        preparedObject.SetActive(true);
        preparedObject.transform.position = position;
        preparedObject.transform.rotation = rotation;
        preparedObject.transform.localScale = localSclale;
        return preparedObject;
    }

    #endregion
}
