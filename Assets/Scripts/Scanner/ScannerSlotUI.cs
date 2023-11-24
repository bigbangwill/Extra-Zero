using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Reflection;
using System;
using System.Linq;

public class ScannerSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ISaveable
{


    public ScannerSlotManager.slotState state;

    public int slotNumber;
    public BluePrintItem holdingItem;

    [SerializeField] private GameObject activeHightlightGameObject;

    private Image image;

    private bool shouldLoad = false;

    private void Start()
    {
        image = GetComponent<Image>();
        AddISaveableToDictionary();
        Init();
    }

    private void Init()
    {
        ScannerSlotManager.Instance.AddScannerSlotToList(this, slotNumber);
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
        ScannerSlotManager.Instance.AddedToSlot(item,this);
        holdingItem = item;
        image.sprite = item.IconRefrence();
    }

    /// <summary>
    /// To remove the item in scanner slot
    /// </summary>
    /// <param name="item"></param>
    public void RemoveFromSlot(BluePrintItem item)
    {
        ScannerSlotManager.Instance.RemovedFromSlot(item,this);
        image.sprite = null;
        holdingItem = null;
    }



    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (state == ScannerSlotManager.slotState.canAdd 
            && ScannerSlotManager.Instance.currentHologram != null)
        {
            AddToSlot(ScannerSlotManager.Instance.currentHologram.cacheItem);
        }

        else if (state == ScannerSlotManager.slotState.canRemove
            || state == ScannerSlotManager.slotState.isDone)
        {
            Debug.Log(holdingItem.IsStackable());
            RemoveFromSlot(holdingItem);
        }
    }

    public void AddISaveableToDictionary()
    {
        int currentOrder = 0 + slotNumber;
        SaveClassManager.Instance.AddISaveableToDictionary(slotNumber + "ScannerSlotUI", this,currentOrder);
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
            EventManager.Instance.RefreshInventory();
            return;
        }
        string savedDataName = saved.holdingItemSpecificName;
        ScannerSlotManager.slotState savedState = saved.state;
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
        EventManager.Instance.RefreshInventory();

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