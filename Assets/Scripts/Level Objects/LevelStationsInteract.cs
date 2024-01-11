using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStationsInteract : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject miniGame;

    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    [SerializeField] private GameObject lastActive;

    public void EnteredBox()
    {
        BasementManager.Instance.SetInteractDelegate(Interact);
        BasementManager.Instance.SetIntractCancelDelegate(ExitInteract);
    }

    public void ExitBox()
    {
        BasementManager.Instance.RemoveInteractDelegate();
        ExitInteract();
    }

    public void TriggerExit()
    {
        BasementManager.Instance.RemoveInteractDelegate();

        foreach (GameObject go in gameObjects)
        {
            if (go.activeSelf)
            {
                lastActive = go;
                lastActive.SetActive(false);
                break;
            }
        }
    }

    public void Interact()
    {
        Debug.Log("Here");
        if (lastActive != null)
        {
            lastActive.SetActive(true);
            return;
        }
        canvas.SetActive(true);
    }
    
    public void ExitInteract()
    {
        foreach (GameObject go in gameObjects)
        {
            if (go.activeSelf)
            {
                lastActive = go;
                break;
            }
        }

        foreach (var go in gameObjects)
        {
            go.SetActive(false);
        }
    }
    
}