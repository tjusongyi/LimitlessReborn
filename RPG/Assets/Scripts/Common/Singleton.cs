using UnityEngine;
using System.Collections;

public abstract class Singleton<T> where T:class,new() {

    private static T instance;
    private static readonly object lockObj = new object();

    public static T Instance
    {
        get
        {
            if(null == instance)
            {
                lock(lockObj)
                {
                    if(null == instance)
                        instance = new T();
                }
               
            }
            return instance;
        }
    }
}
