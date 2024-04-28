using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoadDependent
{
    public void CallAwake();
    public void CallStart();
}

public class NewLoadDependencies : MonoBehaviour
{
    
    [SerializeField] private List<GameObject> gameObjects = new();


    private void Awake()
    {
        foreach(GameObject go in gameObjects)
        {
            go.GetComponent<ILoadDependent>().CallAwake();
        }
    }

    private void Start()
    {
        foreach(GameObject go in gameObjects)
        {
            go.GetComponent<ILoadDependent>().CallStart();
        }
    }

}