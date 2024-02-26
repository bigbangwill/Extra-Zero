using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindSORefrence<T> where T : class
{
    private static PlayerInventoryRefrence playerInventoryRefrence;

    private static Dictionary<string, BaseReference<T>> foundSO  = new();

    public static BaseReference<T> FindScriptableObject(string value)
    {
        if(foundSO.ContainsKey(value))
        {
            if (foundSO[value] == null)
            {
                foundSO[value] = Resources.Load<BaseReference<T>>(value);
            }
            return foundSO[value];
        }
        foundSO.Add(value, Resources.Load<BaseReference<T>>(value));
        return foundSO[value];
    }
    
    
}