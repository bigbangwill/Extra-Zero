using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDependentManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> list = new();

    private void OnEnable()
    {
        StartCoroutine(LoadDependecies());
    }


    IEnumerator LoadDependecies()
    {
        GameObject lastActive = null;
        foreach (GameObject go in list) 
        {
            bool isActive = false;
            isActive = go.activeSelf;
            go.SetActive(true);
            yield return null;
            lastActive = go;
            lastActive.SetActive(isActive);
        }
    }

}