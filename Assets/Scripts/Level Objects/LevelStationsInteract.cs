using System.Collections.Generic;
using UnityEngine;

public class LevelStationsInteract : MonoBehaviour, IInteractable, IReacheable
{

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject miniGame;

    [SerializeField] private List<GameObject> gameObjects = new List<GameObject>();

    protected GameObject lastActive;

    [SerializeField] protected Transform reachingTransfrom;


    protected BasementManagerRefrence basementManagerRefrence;


    private void LoadSORefrence()
    {
        basementManagerRefrence = (BasementManagerRefrence)FindSORefrence<BasementManager>.FindScriptableObject("Basement Manager Refrence");
    }

    private void Start()
    {
        LoadSORefrence();
    }

    public void EnteredBox()
    {
        basementManagerRefrence.val.SetInteractDelegate(Interact);
        basementManagerRefrence.val.SetIntractCancelDelegate(ExitInteract);
    }

    public void ExitBox()
    {
        basementManagerRefrence.val.RemoveInteractDelegate();
        ExitInteract();
    }

    public void TriggerExit()
    {
        basementManagerRefrence.val.RemoveInteractDelegate();

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

    public virtual void Interact()
    {
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

    public virtual NavmeshReachableInformation ReachAction()
    {
        NavmeshReachableInformation value = new(reachingTransfrom.position, Interact);
        return value;
    }

    public NavmeshReachableInformation ReachAction(ItemBehaviour item, int slotnum)
    {
        Debug.LogWarning("Check Here asap");
        return null;
    }
}