
using UnityEngine;


//泛型单类
public class Singleton<T> : MonoBehaviour where T : Component   //Base class for everything attached to GameObjects.
{
    public static T instance { get; set; }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}
