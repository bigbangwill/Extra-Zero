using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;
using System.Linq;
using System;

public enum craftingItemState { CanFill,CantFill,DontFill,Filled}
public class ItemPrinter : MonoBehaviour ,ISaveable
{
    [SerializeField] private CraftingItem craftingItem1;
    [SerializeField] private CraftingItem craftingItem2;
    [SerializeField] private CraftingItem craftingItem3;

    [SerializeField] private Button cancelCrafting;

    private Image itemImage;

    private BluePrintItem currentBluePrint;

    private CraftingItem[] craftingItemArray = new CraftingItem[3];

    List<ItemBehaviour> requiredItemsForCrafting = new();

    private bool iscrafting = false;

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

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        craftingItemArray[0] = craftingItem1;
        craftingItemArray[1] = craftingItem2;
        craftingItemArray[2] = craftingItem3;
    }

    private void Start()
    {
        AddISaveableToDictionary();
    }

    private void OnEnable()
    {
        if(currentBluePrint != null)
            Initialize();
        CheckSavedTime();
        if (isCrafting)
        {
            EventManager.Instance.SecondsElapsedAddListener(SecondElapsed);
        }
    }


    public bool SentToPrinter(BluePrintItem sentItem)
    {
        for(int i = 0; i < craftingItemArray.Length; i++)
        {
            if (isCrafting || craftingItemArray[i].GetState() == craftingItemState.Filled)
            {
                Debug.Log("Is false or is crafting");
                return false;
            }
        }
        currentBluePrint = sentItem;
        Initialize();
        return true;
    }


    

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
            Debug.Log("Check");
        }
    }

    public void FillAllButtonClicked()
    {

        int i = 0;
        foreach (var item in craftingItemArray)
        {
            i++;
            Debug.Log(i);
            if (item.GetState() == craftingItemState.CanFill)
            {
                item.GetItemFromInventory();
            }
        }
    }

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


    public void StartButtonClicked()
    {
        bool allAreFilled = true;
        foreach (var item in craftingItemArray)
        {
            if (item.GetState() == craftingItemState.CanFill || item.GetState() == craftingItemState.CantFill)
                allAreFilled = false;
        }
        if (allAreFilled)
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
        if (EventManager.Instance != null)
        {
            if (isCrafting)
            {
                savedTime = new GameTime().CurrentTime();
                isSaved = true;
                EventManager.Instance.SecondsElapsedRemoveListener(SecondElapsed);
            }
        }
    }


    private void CheckSavedTime()
    {
        if (isSaved)
        {
            int differentSeconds = (int)new GameTime().RawTimeCurrentMinusSaved(savedTime);
            for (int i = 0; i < differentSeconds; i++)
            {
                SecondElapsed();
            }
            isSaved = false;
        }

    }

    private void StartCrafting()
    {
        EventManager.Instance.SecondsElapsedAddListener(SecondElapsed);
        isCrafting = true;
        craftMaxTimer = currentBluePrint.CraftTimer();
        currentCraftTimer = craftMaxTimer;
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

    private void FinishedCrafting()
    {
        EventManager.Instance.SecondsElapsedRemoveListener(SecondElapsed);
        isCrafting = false;
        ExportItem(currentBluePrint);
        currentBluePrint = null;
        itemImage.sprite = null;
        foreach (var item in craftingItemArray)
        {
            item.ResetState();
        }
    }

    public void CancledCrafting()
    {
        isCrafting = false;
        craftMaxTimer = 0;
        currentCraftTimer = 0;
        EventManager.Instance.SecondsElapsedRemoveListener(SecondElapsed);
        foreach (var item in craftingItemArray)
        {
            item.RemoveItemFromPrinterSlot();
        }

    }


    private void ExportItem(BluePrintItem bluePrintItem)
    {
        Debug.Log("Exported item: " + bluePrintItem.GetName());
    }

    public void AddISaveableToDictionary()
    {
        SaveClassManager.Instance.AddISaveableToDictionary(GetName(), this, 4);
    }

    public object Save()
    {
        SaveClassesLibrary.ItemPrinter saveData = new(
            craftingItem1.GetState(),
            craftingItem2.GetState(),
            craftingItem3.GetState(),
            currentBluePrint.GetName(),
            isCrafting,
            currentCraftTimer,
            craftMaxTimer);
        return saveData;
    }

    public void Load(object savedData)
    {

        Dictionary<string, BluePrintItem> itemFinderDictionary = new();

        List<Type> childTypesList = Assembly.GetAssembly(typeof(BluePrintItem))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(BluePrintItem))).ToList();

        List<BluePrintItem> itemList = new();
        foreach (var item in childTypesList)
        {
            itemList.Add((BluePrintItem)Activator.CreateInstance(item));
        }

        foreach (var child in itemList)
        {
            itemFinderDictionary.Add(child.GetName(), child);
        }

        SaveClassesLibrary.ItemPrinter saved = (SaveClassesLibrary.ItemPrinter)savedData;
        craftingItemArray[0].SetState(saved.state1);
        craftingItemArray[1].SetState(saved.state2);
        craftingItemArray[2].SetState(saved.state3);

        isCrafting = saved.isCrafting;
        if (isCrafting)
        {
            EventManager.Instance.SecondsElapsedAddListener(SecondElapsed);
        }
        currentBluePrint = itemFinderDictionary[saved.savedBluePrintName];
        itemImage.sprite = currentBluePrint.IconRefrence();
        currentCraftTimer = saved.currentCraftTimer;
        craftMaxTimer = saved.maxCraftTimer;

        int leftToFillTheList = craftingItemArray.Length - requiredItemsForCrafting.Count;
        for (int i = 0; i < leftToFillTheList; i++)
        {
            requiredItemsForCrafting.Add(new EmptyItem());
        }

        for (int i = 0; i < craftingItemArray.Length; i++)
        {
            craftingItemArray[i].SetBluePrint(requiredItemsForCrafting[i]);
        }
    }

    

    public string GetName()
    {
        return "ItemPrinter";
    }
}
