using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasementManager : SingletonComponent<BasementManager>
{

    #region Singleton Manager
    public static BasementManager Instance
    {
        get { return ((BasementManager)_Instance); }
        set { _Instance = value; }
    }
    #endregion

    private UnityEvent InventoryEvent = new();

    /// <summary>
    /// To add method as a listener to get the inventorybind
    /// </summary>
    /// <param name="action"></param>
    public void InventoryEventAddListener(UnityAction action)
    {
        InventoryEvent.AddListener(action);
    }

    /// <summary>
    /// To remove a method as a listener to get the inventorybind
    /// </summary>
    /// <param name="action"></param>
    public void InventoryEventRemoveListener(UnityAction action)
    {
        InventoryEvent.RemoveListener(action);
    }


    private void OnInventory()
    {
        InventoryEvent.Invoke();
    }

    private void OnFirstSlot()
    {
        PlayerInventory.Instance.SetActiveItem(0);
    }
    private void OnSecondSlot()
    {
        PlayerInventory.Instance.SetActiveItem(1);
    }
    private void OnThirdSlot()
    {
        PlayerInventory.Instance.SetActiveItem(2);
    }
    private void OnForthSlot()
    {
        PlayerInventory.Instance.SetActiveItem(3);
    }
    private void OnFifthSlot()
    {
        PlayerInventory.Instance.SetActiveItem(4);
    }


}