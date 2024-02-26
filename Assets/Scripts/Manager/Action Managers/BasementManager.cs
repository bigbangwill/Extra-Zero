using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class BasementManager : SingletonComponent<BasementManager>
{
    //#region Singleton Manager
    //public static BasementManager Instance
    //{
    //    get { return ((BasementManager)_Instance); }
    //    set { _Instance = value; }
    //}
    //#endregion

    private UnityEvent InventoryEvent = new();
    public delegate void Intract();
    private Intract intractVoid;
    private Intract intractCancelVoid;

    private PlayerInput playerInput;

    private BasementManagerRefrence refrence;
    private PlayerInventoryRefrence inventoryRefrence;

    private void SetRefrence()
    {
        refrence = (BasementManagerRefrence)FindSORefrence<BasementManager>.FindScriptableObject("Basement Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Check ASAP");
            return;
        }
        refrence.val = this;
    }
    private void LoadSORefrence()
    {
        
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        playerInput = GetComponent<PlayerInput>();
    }

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


    public void SetInteractDelegate(Intract action)
    {
        intractVoid = action;
    }

    public void SetIntractCancelDelegate(Intract action)
    {
        intractCancelVoid = action;
    }

    public void RemoveInteractDelegate()
    {
        intractVoid = null;
        intractCancelVoid = null;
    }

    public Vector2 MousePos()
    {
        Vector2 mousePos = playerInput.actions["MousePosition"].ReadValue<Vector2>();
        return mousePos;
    }


    private void OnInventory()
    {
        InventoryEvent.Invoke();
    }

    private void OnInteract()
    {
        if (intractVoid != null)
            intractVoid();
    }

    private void OnIntractCancel()
    {
        if (intractCancelVoid != null)
            intractCancelVoid();
    }

    private void OnFirstSlot()
    {
        inventoryRefrence.val.SetActiveItem(0);
    }
    private void OnSecondSlot()
    {
        inventoryRefrence.val.SetActiveItem(1);
    }
    private void OnThirdSlot()
    {
        inventoryRefrence.val.SetActiveItem(2);
    }
    private void OnForthSlot()
    {
        inventoryRefrence.val.SetActiveItem(3);
    }
    private void OnFifthSlot()
    {
        inventoryRefrence.val.SetActiveItem(4);
    }


}