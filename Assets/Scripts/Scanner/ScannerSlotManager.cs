using System.Collections.Generic;
using UnityEngine;

public class ScannerSlotManager : MonoBehaviour
{

    public delegate void RefreshScannerUI();
    public RefreshScannerUI refreshUI;
    

    public enum SlotState { canAdd, canRemove,passive,isDone,isLocked }

    public List<ScannerSlotUI> slots = new();


    private ScannerSlotUI currentActiveSlot = null;

    //will get called from outerScope
    public ScannerHologramUI currentHologram = null;

    private ScannerSlotManagerRefrence refrence;
    private PlayerInventoryRefrence inventoryRefrence;

    private void SetRefrence()
    {
        refrence = (ScannerSlotManagerRefrence)FindSORefrence<ScannerSlotManager>.FindScriptableObject("Scanner Slot Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        Debug.Log("We are here!");
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
    }

    /// <summary>
    /// It's used to set the current active scanner slot. might get removed soon.
    /// </summary>
    /// <param name="slotUI"></param>
    public void SetCurrentActiveSlot(ScannerSlotUI slotUI)
    {
        if (currentActiveSlot != null && currentActiveSlot != slotUI)
        {
            currentActiveSlot.SetDeactive();
        }
        if (currentActiveSlot == slotUI)
        {
            currentActiveSlot.SetDeactive();
            currentActiveSlot = null;
            return;
        }
        currentActiveSlot = slotUI;
    }

    /// <summary>
    /// To handle the refresh ui and removing the related hologram from the player inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotUI"></param>
    public void AddedToSlot(BluePrintItem item,ScannerSlotUI slotUI)
    {
        if (inventoryRefrence.val.HaveItemInInventory(item, true))
        {
            slotUI.state = SlotState.canRemove;
            refreshUI();
            slotUI.SetDeactive();
            currentActiveSlot = null;
        }
        else
        {
            Debug.LogWarning("Check here");
        }
    }

    /// <summary>
    /// To handle the refresh ui and adding the related hologram to the player inventory
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotUI"></param>
    public void RemovedFromSlot(BluePrintItem item, ScannerSlotUI slotUI, bool isDone)
    {
        if(!isDone)
            inventoryRefrence.val.AddItemToInventory(item);
        refreshUI();
        slotUI.SetDeactive();
        currentActiveSlot = null;
        slotUI.state = SlotState.canAdd;
    }

    public void UpgradeOrbit(bool isQubit)
    {
        slots[1].state = SlotState.canAdd;
        if(isQubit)
            slots[2].state = SlotState.canAdd;
    }
}