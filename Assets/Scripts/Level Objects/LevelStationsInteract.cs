using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStationsInteract : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject canvas;

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

    public void Interact()
    {
        Debug.Log("Here");
        canvas.SetActive(true);
    }
    
    public void ExitInteract()
    {
        Debug.Log("Here");
        canvas.SetActive(false);
    }
    
}