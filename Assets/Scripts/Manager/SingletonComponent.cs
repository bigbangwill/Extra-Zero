using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingletonComponent <T> : MonoBehaviour where T : SingletonComponent<T>
{
    private static T __Instance;

    private static bool applicationIsQuiting;

    protected static SingletonComponent<T> _Instance
    {
        get
        {
            if (applicationIsQuiting)
                return null;

            if (!__Instance)
            {
                T[] managers = GameObject.FindObjectsByType(typeof(T),FindObjectsSortMode.None) as T[];
                if(managers != null)
                {
                    if (managers.Length == 1)
                    {
                        __Instance = managers[0];
                        return __Instance;
                    }
                    else if(managers.Length > 1)
                    {
                        Debug.LogError("You have more than one " + typeof(T).Name);
                        for(int i = 0; i < managers.Length; i++)
                        {
                            T manager = managers[i];
                            Destroy(manager.gameObject);
                        }
                    }
                }
                GameObject go = new(typeof(T).Name,typeof(T));
                __Instance = go.GetComponent<T>();
                
            }
            return __Instance;
        }
        set
        {
            __Instance = value as T;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Gets destroyed");
        applicationIsQuiting = true;
    }
}