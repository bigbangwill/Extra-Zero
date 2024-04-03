using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Linq;

public class ScannerSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISaveable
{


    [SerializeField] private ScannerSlotManager.SlotState _state;

    public ScannerSlotManager.SlotState state
    {
        get { return _state; }
        set 
        {
            _state = value;
            SetCurrentImage();
        }

    }





    public int slotNumber;
    public BluePrintItem holdingItem;

    [SerializeField] private GameObject activeHightlightGameObject;

    private Image image;

    private bool shouldLoad = false;

    private EventManagerRefrence eventManagerRefrence;
    private ScannerSlotManagerRefrence scannerSlotManagerRefrence;
    private SaveClassManagerRefrence saveClassManagerRefrence;
    private EventTextManager eventTextManager;

    [SerializeField] private Color spriteCanAdd;
    [SerializeField] private Color spriteCanRemove;
    [SerializeField] private Color spritePassive;
    [SerializeField] private Color spriteIsLocked;
    [SerializeField] private Color spriteIsDone;
    [SerializeField] private Image overlay;

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
        scannerSlotManagerRefrence = (ScannerSlotManagerRefrence)FindSORefrence<ScannerSlotManager>.FindScriptableObject("Scanner Slot Manager Refrence");
        saveClassManagerRefrence = (SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence");
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
    }

    private void Start()
    {
        LoadSORefrence();
        image = GetComponent<Image>();
        AddISaveableToDictionary();
        SetCurrentImage();
    }

    /// <summary>
    /// might be removed as well
    /// </summary>
    public void SetDeactive()
    {
        activeHightlightGameObject.SetActive(false);
    }


    /// <summary>
    /// To add the currenta active item from scroll view in to the related scanner slot
    /// </summary>
    /// <param name="item"></param>
    public void AddToSlot(BluePrintItem item)
    {
        scannerSlotManagerRefrence.val.AddedToSlot(item,this);
        holdingItem = item;
        image.sprite = item.IconRefrence();
        eventTextManager.CreateNewText("Added to slot", TextType.Information);
    }

    /// <summary>
    /// To remove the item in scanner slot
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromSlot(BluePrintItem item,bool isDone)
    {
        scannerSlotManagerRefrence.val.RemovedFromSlot(item,this,isDone);
        image.sprite = null;
        holdingItem = null;
        eventTextManager.CreateNewText("Removed from slot", TextType.Information);
    }


    private void SetCurrentImage()
    {
        switch (_state)
        {
            case ScannerSlotManager.SlotState.canAdd: overlay.color = spriteCanAdd; break;
            case ScannerSlotManager.SlotState.canRemove: overlay.color = spriteCanRemove; break;
            case ScannerSlotManager.SlotState.isDone: overlay.color = spriteIsDone; break;
            case ScannerSlotManager.SlotState.isLocked: overlay.color = spriteIsLocked; break;
            case ScannerSlotManager.SlotState.passive: overlay.color = spritePassive; break;
            default: break;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == ScannerSlotManager.SlotState.canAdd 
            && scannerSlotManagerRefrence.val.currentHologram != null)
        {
            AddToSlot(scannerSlotManagerRefrence.val.currentHologram.cacheItem);
        }
        else if (state == ScannerSlotManager.SlotState.canRemove
            || state == ScannerSlotManager.SlotState.isDone)
        {
            if (state == ScannerSlotManager.SlotState.isDone)
                RemoveFromSlot(holdingItem, true);
            else
                RemoveFromSlot(holdingItem, false);
        }
        else if(state == ScannerSlotManager.SlotState.isLocked)
        {
            eventTextManager.CreateNewText("Slot is locked", TextType.Error);
        }
        else if (state == ScannerSlotManager.SlotState.passive)
        {
            eventTextManager.CreateNewText("Slot is pending", TextType.Error);
        }
        else if (state == ScannerSlotManager.SlotState.canAdd
            && scannerSlotManagerRefrence.val.currentHologram == null)
        {
            eventTextManager.CreateNewText("Select a item from the list first!", TextType.Error);
        }

    }

    public void AddISaveableToDictionary()
    {
        int currentOrder = 0 + slotNumber;
        saveClassManagerRefrence.val.AddISaveableToDictionary(slotNumber + "ScannerSlotUI", this,currentOrder);
    }

    public object Save()
    {
        if (holdingItem == null)
        {
            SaveClassesLibrary.ScannerSlotUI saveData = new("Empty", state, slotNumber);
            return saveData;
        }
        else
        {
            SaveClassesLibrary.ScannerSlotUI saveData = new(holdingItem.GetName(), state, slotNumber);
            return saveData;
        }
    }

    public void Load(object savedData)
    {
        List<Type> childTypesList = Assembly.GetAssembly(typeof(BluePrintItem))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(BluePrintItem))).ToList();
        Dictionary<string,BluePrintItem> allChildDictionary = new();

        SaveClassesLibrary.ScannerSlotUI saved = (SaveClassesLibrary.ScannerSlotUI)savedData;
        if (saved.holdingItemSpecificName == "Empty")
        {
            holdingItem = null;
            image.sprite = null;
            state = saved.state;
            slotNumber = saved.slotNumber;
            eventManagerRefrence.val.RefreshInventory();
            return;
        }
        string savedDataName = saved.holdingItemSpecificName;
        ScannerSlotManager.SlotState savedState = saved.state;
        int savedSlotNumber = saved.slotNumber;


        foreach (var child in childTypesList)
        {
            BluePrintItem targetItem = (BluePrintItem)Activator.CreateInstance(child);
            allChildDictionary.Add(targetItem.GetName(), targetItem);
        }
        if (allChildDictionary.ContainsKey(savedDataName))
        {
            holdingItem = allChildDictionary[savedDataName];
            holdingItem.Load();
            image.sprite = holdingItem.IconRefrence();
            state = savedState;
            slotNumber = savedSlotNumber;
            shouldLoad = true;
        }
        eventManagerRefrence.val.RefreshInventory();

    }

    private void Update()
    {
        if (shouldLoad)
        {
            image.sprite = holdingItem.IconRefrence();
            if (image.sprite != null)
                shouldLoad = false;
        }
    }

    public string GetName()
    {
        return slotNumber + "ScannerSlotUI";
    }
    

    
}