using System.Collections.Generic;
using UnityEngine;

public class ScannerSlotManager : SingletonComponent<ScannerSlotManager>
{

    #region Sinleton
    public static ScannerSlotManager Instance
    {
        get { return ((ScannerSlotManager)_Instance); }
        set { _Instance = value; }
    }
    #endregion

    public delegate void RefreshScannerUI();
    public RefreshScannerUI refreshUI;
    

    public enum slotState { canAdd, canRemove,passive,isDone }

    public List<ScannerSlotUI> slots = new();


    private ScannerSlotUI currentActiveSlot = null;

    //will get called from outerScope
    public ScannerHologramUI currentHologram = null;

    private void Awake()
    {
        Instance = this;
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
        if (PlayerInventory.Instance.HaveItemInInventory(item, true))
        {
            slotUI.state = slotState.canRemove;
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
            PlayerInventory.Instance.AddItemToInventory(item);
        refreshUI();
        slotUI.SetDeactive();
        currentActiveSlot = null;
        slotUI.state = slotState.canAdd;
    }

    /// <summary>
    /// To add the ScannerSlotUI script in the list in the OnEnable method.
    /// The incoming slot number should be minus one based on the current slotnumber in unity editor
    /// </summary>
    /// <param name="slotUI"></param>
    public void AddScannerSlotToList(ScannerSlotUI slotUI, int slotNumber)
    {
        if (slots.Count < slotNumber)
        {
            int leftToAdd = slotNumber - slots.Count;
            for(int i = 0; i < leftToAdd; i++)
            {
                slots.Add(null);
            }
        }
        slots[slotNumber - 1] = slotUI;
        slotUI.state = slotState.canAdd;
    }
    
}