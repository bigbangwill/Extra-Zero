using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using System.Linq;
using System;

public enum craftingItemState { CanFill,CantFill,DontFill,Filled}
public class ItemPrinter : MonoBehaviour
{
    [SerializeField] private CraftingItem craftingItem1;
    [SerializeField] private CraftingItem craftingItem2;
    [SerializeField] private CraftingItem craftingItem3;
    [SerializeField] private Button cancelCrafting;
    [SerializeField] private Button fillButton;
    [SerializeField] private Button removeButton;

    private Image itemImage;

    private BluePrintItem currentBluePrint;

    private CraftingItem[] craftingItemArray = new CraftingItem[3];

    List<ItemBehaviour> requiredItemsForCrafting = new();

    private bool iscrafting = false;
    private bool isDone = false;

    private bool isCrafting { 
        get 
        { 
            return iscrafting;
        }
        set
        {
            iscrafting = value;
            if (iscrafting)
                cancelCrafting.interactable = true;
            else
                cancelCrafting.interactable = false;
        }
    }


    private EventManagerRefrence eventManagerRefrence;
    private PlayerInventory inventory;
    private SaveClassManagerRefrence saveClassManagerRefrence;
    private EventTextManager eventText;

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
        saveClassManagerRefrence = (SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence");
        inventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
        eventText = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;

    }

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        craftingItemArray[0] = craftingItem1;
        craftingItemArray[1] = craftingItem2;
        craftingItemArray[2] = craftingItem3;
    }

    private void Start()
    {
        LoadSORefrence();
        //AddISaveableToDictionary();
    }

    private void OnEnable()
    {
        if(currentBluePrint != null && !isDone)
            Initialize();
        CheckSavedTime();
        if (isCrafting)
        {
            eventManagerRefrence.val.SecondsElapsedAddListener(SecondElapsed);
        }
    }

    /// <summary>
    /// This method get called from import holder to set the bluepring that the player is trying to create.
    /// </summary>
    /// <param name="sentItem"></param>
    /// <returns></returns>
    public bool SentToPrinter(BluePrintItem sentItem)
    {
        for(int i = 0; i < craftingItemArray.Length; i++)
        {
            if (isCrafting || craftingItemArray[i].GetState() == craftingItemState.Filled || isDone)
            {
                Debug.Log("Is false or is crafting");
                return false;
            }
        }
        currentBluePrint = sentItem;
        Initialize();
        return true;
    }


    
    // to refill the requiredItemsForCrafting list with the related materials and call the
    // FillTheCraftingListAndSetIndicator
    private void Initialize()
    {
        itemImage.sprite = currentBluePrint.IconRefrence();
        requiredItemsForCrafting.Clear();
        foreach (ItemBehaviour item in currentBluePrint.materialsList)
        {
            requiredItemsForCrafting.Add(item);
        }

        FillTheCraftingListAndSetIndicator();
    }

    /// <summary>
    ///  To see how many material the current blueprint needs and to set the needed indicator
    ///  depending on the player inventory.
    /// </summary>
    private void FillTheCraftingListAndSetIndicator()
    {
        int leftToFillTheList = craftingItemArray.Length - requiredItemsForCrafting.Count;
        for (int i = 0; i < leftToFillTheList; i++)
        {
            requiredItemsForCrafting.Add(new EmptyItem());
        }

        for (int i = 0; i < craftingItemArray.Length; i++)
        {
            craftingItemArray[i].SetIndicator(requiredItemsForCrafting[i]);
        }
    }

    /// <summary>
    /// The button method to fill the slots if the player has the material.
    /// </summary>
    public void FillAllButtonClicked()
    {
        bool showedError = false;
        int i = 0;
        foreach (var item in craftingItemArray)
        {
            i++;
            if (item.GetState() == craftingItemState.CanFill)
            {
                item.GetItemFromInventory();
            }
            else if(item.GetState() == craftingItemState.CantFill)
            {
                if (!showedError)
                {
                    eventText.CreateNewText("Don't have the material to fill", TextType.Error);
                    showedError = true;
                }

            }
        }
    }

    /// <summary>
    /// The button method to remove the filled slots.
    /// </summary>
    public void RemoveAllButtonClicked()
    {
        if (!isCrafting)
        {
            foreach (var item in craftingItemArray)
            {
                if (item.GetState() == craftingItemState.Filled)
                {
                    item.RemoveItemFromPrinterSlot();
                }
            }
        }
    }


    /// <summary>
    /// The button method to check if it can start to create the item. 
    /// </summary>
    public void StartButtonClicked()
    {
        bool allAreFilled = true;
        foreach (var item in craftingItemArray)
        {
            if (item.GetState() == craftingItemState.CanFill || item.GetState() == craftingItemState.CantFill)
            {
                allAreFilled = false;
                eventText.CreateNewText("Fill the materials first!", TextType.Error);
            }
        }
        if (allAreFilled && !isDone)
        {
            StartCrafting();
        }
    }

    private int craftMaxTimer = 0;
    private int currentCraftTimer = 0;
    [SerializeField] private TextMeshProUGUI timerText;

    private bool isSaved = false;
    private GameTime savedTime;

    private void OnDisable()
    {        
        if (eventManagerRefrence.val != null)
        {
            if (isCrafting)
            {
                savedTime = new GameTime().CurrentTime(eventManagerRefrence.val);
                isSaved = true;
                eventManagerRefrence.val.SecondsElapsedRemoveListener(SecondElapsed);
            }
        }
    }

    
    // Gets called by the script it self to check if the object went disabled while creating and item
    // to elapse the seconds that the object was OFFLINE! :D
    private void CheckSavedTime()
    {
        if (isSaved)
        {
            int differentSeconds = (int)new GameTime().RawTimeCurrentMinusSaved(savedTime, eventManagerRefrence.val);
            for (int i = 0; i < differentSeconds; i++)
            {
                SecondElapsed();
            }
            isSaved = false;
        }

    }

    // gets checked before and now it start crafting.
    private void StartCrafting()
    {
        eventManagerRefrence.val.SecondsElapsedAddListener(SecondElapsed);
        isCrafting = true;
        craftMaxTimer = currentBluePrint.CraftTimer();
        currentCraftTimer = craftMaxTimer;
        fillButton.interactable = false;
        removeButton.interactable = false;
    }


    private void SecondElapsed()
    {
        if (isCrafting)
        {
            currentCraftTimer -= 1;
            timerText.text = currentCraftTimer.ToString();
            if (currentCraftTimer <= 0)
            {
                FinishedCrafting();
            }
        }
    }

    

    // the mothod to reset the printer back to normal state and call the export method.
    private void FinishedCrafting()
    {
        eventManagerRefrence.val.SecondsElapsedRemoveListener(SecondElapsed);
        iscrafting = false;
        isDone = true;
        itemImage.sprite = currentBluePrint.CraftedItemReference().IconRefrence();
        fillButton.interactable = true;
        removeButton.interactable = true;
        foreach (var item in craftingItemArray)
        {
            item.ResetState();
        }
    }

    /// <summary>
    /// The button method to cancel the current active crafting work.
    /// </summary>
    public void CancledCrafting()
    {
        isCrafting = false;
        craftMaxTimer = 0;
        currentCraftTimer = 0;
        eventManagerRefrence.val.SecondsElapsedRemoveListener(SecondElapsed);
        foreach (var item in craftingItemArray)
        {
            item.RemoveItemFromPrinterSlot();
        }

    }

    /// <summary>
    /// This method gets called from the cover GO in the scene. it's only for when the item is complete and 
    /// the player has to pick it up.
    /// </summary>
    public void CoverClicked()
    {
        if (isDone)
        {
            if (inventory.HaveEmptySlot(currentBluePrint.CraftedItemReference(), true))
            {
                ExportItem(currentBluePrint);
                currentBluePrint = null;
                itemImage.sprite = null;
                isDone = false;
            }
            else
            {
                //eventText.CreateNewText("Inventory is full!",Color.green,Color.red);
            }
        }
    }

    // Gets implemented when the player can pick up item.
    private void ExportItem(BluePrintItem bluePrintItem)
    {
       
    }
}
