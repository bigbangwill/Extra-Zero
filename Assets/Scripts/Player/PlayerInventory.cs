
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour ,IStashable
{


    // Player inventory slot count
    [SerializeField] private int inventorySlotCount;
    // For the Item slot prefab in the UI
    [SerializeField] private GameObject itemSlotPrefab;
    // For offsetting the distance between each icon
    [SerializeField] private float offsetHeight;
    // For Setting a max value of slots in each rows
    [SerializeField] private int rowCountMax;



    // To put a bit of an offset for inventory instatiate method to not to start at the exact half point
    [SerializeField] private int startingPointOffsetWidth;
    [SerializeField] private int startingPointOffsetHeight;

    [SerializeField] private ItemBehaviour[] inventoryArray;
    [SerializeField] private List<ItemSlotUI> itemSlotUIList = new();

    // Width and height of the item slot prefab
    private float itemSlotPrefabWidth;
    private float itemSlotPrefabHeight;

    // Variable for the current active item
    private ItemBehaviour currentActiveItem;
    private int currentActiveItemSlotNum = int.MaxValue;
    [SerializeField] private Transform activeItemTranform;


    private PlayerInventoryRefrence refrence;
    private EventManagerRefrence eventManagerRefrence;
    private UsableCanvasManagerRefrence usableRefrence;
    private SaveClassManagerRefrence saveClassManagerRefrence;

    private EventTextManager eventText;

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
        usableRefrence = (UsableCanvasManagerRefrence)FindSORefrence<UseableItemCanvasScript>.FindScriptableObject("Usable Manager Refrence");
        saveClassManagerRefrence = (SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence");
        eventText = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
    }


    

    private void SetRefrence()
    {
        refrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }


    private void Awake()
    {
        SetRefrence();

        inventoryArray = new ItemBehaviour[inventorySlotCount];
        itemSlotPrefabWidth = itemSlotPrefab.GetComponent<RectTransform>().rect.width;
        itemSlotPrefabHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            inventoryArray[i] = new EmptyItem();
        }
        InitUI();

        
    }


    public void TESTADDHAMMER()
    {
        HaveEmptySlot(new CraftedItem.RepairHammer(usableRefrence.val.transform),true);
    }

    

    private void Start()
    {
        LoadSORefrence();
        //AddISaveableToDictionary();
        //AddItemToInventory(new CraftedItem.InsulationFoam());
        //AddItemToInventory(new CraftedItem.MemoryCore());

        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.MineralOilEffect()));
        //AddItemToInventory(new PotionItem(new PotionEffect.SyntheticOilEffect()));

        //AddItemToInventory(new MaterialItem.Plastic(10));
        //AddItemToInventory(new MaterialItem.AluminumAlloy(10));
        //AddItemToInventory(new MaterialItem.Ceramic(10));
        //AddItemToInventory(new MaterialItem.StainlessSteel(10));
        //AddItemToInventory(new MaterialItem.TitaniumAlloy(10));
        //AddItemToInventory(new BluePrintItem.WalkingStick());
        //AddItemToInventory(new BluePrintItem.Hoe());
        //AddItemToInventory(new BluePrintItem.Gun());
        //AddItemToInventory(new BluePrintItem.Plant());

        //HaveEmptySlot(new Herb.Chamomile(15), true);
        //HaveEmptySlot(new Herb.Lavender(15), true);
        //HaveEmptySlot(new Herb.Sage(15), true);
        //HaveEmptySlot(new Herb.Chamomile(50), true);
        //HaveEmptySlot(new Herb.Lavender(50), true);
        //HaveEmptySlot(new Herb.Sage(50), true);
        //HaveEmptySlot(new Herb.Patchouli(50), true);
        //HaveEmptySlot(new Herb.Hellebore(50), true);



        HaveEmptySlot(new Herb.Lavender(20), true);
        HaveEmptySlot(new Herb.Sage(20), true);
        HaveEmptySlot(new Herb.Chamomile(20), true);
        HaveEmptySlot(new Herb.Patchouli(20), true);
        HaveEmptySlot(new Herb.Hellebore(20), true);

    }

    /// <summary>
    /// A method to get called when ever a change has happend to inventory
    /// that requires a change in the ui. so it will call the event to invoke 
    /// all of the listeners that need a change in ui.
    /// </summary>
    public void CallUIRefreshEvent()
    {
        //InitUI();
        eventManagerRefrence.val.RefreshInventory();
    }

    /// <summary>
    /// A method for elements that need to find all of the needed item in inventory
    /// first use is scanner manager to build the content list
    /// </summary>
    /// <param name="_ItemType"></param>
    /// <returns></returns>
    public List<T> SearchInventoryOfItemBehaviour<T>(ItemType _ItemType)
    {
        List<T> itemBehaviours = new();
        foreach (var item in inventoryArray)
        {
            if (item.GetItemTypeValue() == _ItemType)
            {
                T newItem = (T)(object)item;
                itemBehaviours.Add(newItem);
            }
        }
        return itemBehaviours;
    }


    /// <summary>
    /// Method to get called from outer scope (the ui) to set the related item slot int to
    /// be the current active item
    /// </summary>
    /// <param name="slot"></param>
    public void SetActiveItem2(int slot)
    {
        if (inventoryArray[slot].GetItemTypeValue() == ItemType.empty)
            return;
        if (currentActiveItemSlotNum != slot)
        {
            currentActiveItem = inventoryArray[slot];
            currentActiveItemSlotNum = slot;
            activeItemTranform.gameObject.SetActive(true);
            activeItemTranform.position = itemSlotUIList[slot].transform.position;
        }
        else
        {
            if (currentActiveItem.IsUseable())
            {
                currentActiveItem.Use();
            }
            currentActiveItem = null;
            currentActiveItemSlotNum = int.MaxValue;
            activeItemTranform.gameObject.SetActive(false);
        }
    }


    public void SetActiveItem(int slot)
    {
        if (inventoryArray[slot].IsActiveable())
        {
            if (currentActiveItemSlotNum != slot)
            {
                currentActiveItem?.OnDeactive();
                currentActiveItem = inventoryArray[slot];
                currentActiveItemSlotNum = slot;
                currentActiveItem.OnActive();
                activeItemTranform.gameObject.SetActive(true);
                activeItemTranform.position = itemSlotUIList[slot].transform.position;
            }
            else
            {
                if (currentActiveItem.IsUseable())
                {
                    currentActiveItem.Use();
                }
                currentActiveItem.OnDeactive();
                currentActiveItem = null;
                currentActiveItemSlotNum = int.MaxValue;
                activeItemTranform.gameObject.SetActive(false);
            }
        }
        if (inventoryArray[slot].GetItemTypeValue() == ItemType.potion)
        {
            PotionItem item = (PotionItem)inventoryArray[slot];
            if (item.IsDrinkable)
            {
                if (currentActiveItemSlotNum != slot)
                {
                    currentActiveItem = inventoryArray[slot];
                    currentActiveItemSlotNum = slot;
                    activeItemTranform.gameObject.SetActive(true);
                    activeItemTranform.position = itemSlotUIList[slot].transform.position;
                }
                else
                {
                    item.Use();
                    if (item.IsConsumable())
                    { 
                        ItemBehaviour clone = (ItemBehaviour)currentActiveItem.Clone();
                        clone.SetCurrentStack(1);
                        HaveItemInInventory(clone, true);
                    }
                    currentActiveItem = null;
                    currentActiveItemSlotNum = int.MaxValue;
                    activeItemTranform.gameObject.SetActive(false);
                }
            }
        }
    }

    /// <summary>
    /// Method to directly remove the items or some stack of an item from the slot.
    /// </summary>
    /// <param name="slotNumber"></param>
    /// <param name="count"></param>
    public void RemoveFromSlotNumber(int slotNumber, int count)
    {
        if (inventoryArray[slotNumber].CurrentStack() <= count)
        {
            RemoveItemFromInventory(slotNumber);
        }
        else
        {
            int currentStack = inventoryArray[slotNumber].CurrentStack();
            inventoryArray[slotNumber].SetCurrentStack(currentStack - count);
            itemSlotUIList[slotNumber].RefreshText();
        }
    }

    /// <summary>
    /// Method to get called from outer scope to (the ui) to swap a and b item slots
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void SwapItemInInventory(int a, int b)
    {
        Debug.Log(a + "  " + b);
        ItemSlotUI slotA = itemSlotUIList[a];
        ItemBehaviour itemA = inventoryArray[a];
        ItemSlotUI slotB = itemSlotUIList[b];
        ItemBehaviour itemB = inventoryArray[b];
        itemSlotUIList[a] = slotB;
        itemSlotUIList[b] = slotA;
        inventoryArray[a] = itemB;
        inventoryArray[b] = itemA;
        Debug.Log(inventoryArray[a].GetItemTypeValue());
        Debug.Log(inventoryArray[b].GetItemTypeValue());
    }

    /// <summary>
    /// An overall method to check if the item is stackable or not to navigate to the right related method
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(ItemBehaviour item)
    {
        if (item.IsStackable())
            AddStackableItemToInventory(item);
        else
            AddNonStackableItemToInventory(item);
        if (item.IsActiveable())
        {
            Debug.Log("Here");
            item.OnCreate();
        }
        CallUIRefreshEvent();
    }

    /// <summary>
    /// To add the non stackable item to the inventory 
    /// </summary>
    /// <param name="item"></param>
    private void AddNonStackableItemToInventory(ItemBehaviour item)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Equals(new EmptyItem()))
            {
                inventoryArray[i] = item;
                itemSlotUIList[i].Reset();
                return;
            }
        }
        Debug.LogWarning("You might wanna check here cause there is no empty slot");
    }

    /// <summary>
    /// To check if there is an item that can be stacked with the new item or not
    /// </summary>
    /// <param name="item"></param>
    private void AddStackableItemToInventory(ItemBehaviour item)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (item.Equals(inventoryArray[i]))
            {
                if (inventoryArray[i].CurrentStack() != inventoryArray[i].MaxStack())
                {
                    int sum = item.CurrentStack() + inventoryArray[i].CurrentStack();
                    if (sum <= inventoryArray[i].MaxStack())
                    {
                        inventoryArray[i].SetCurrentStack(sum);
                        itemSlotUIList[i].Reset();
                        return;
                    }
                    else
                    {
                        int sub = sum - inventoryArray[i].MaxStack();
                        inventoryArray[i].SetCurrentStack(inventoryArray[i].MaxStack());
                        ItemBehaviour cloneItem = (ItemBehaviour)item.Clone();
                        cloneItem.SetCurrentStack(sub);
                        //item.SetCurrentStack(sub);
                        itemSlotUIList[i].Reset();
                        AddNonStackableItemToInventory(cloneItem);
                        return;
                    }
                }
            }            
        }
        AddNonStackableItemToInventory(item);
    }


    /// <summary>
    /// A method mainly for quests to check if the player has an specific item
    /// or a stack of an item.the bool shouldRemove will be determine if the
    /// if the item should be removed after it exists or not.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="shouldRemove"></param>
    /// <returns></returns>
    public bool HaveItemInInventory(ItemBehaviour item,bool shouldRemove)
    {
        int savedItemStack = item.CurrentStack();
        if (!item.IsStackable())
        {
            for (int i = 0; i < inventoryArray.Length; i++)
            {
                if (item.Equals(inventoryArray[i]))
                {
                    if (shouldRemove)
                    {
                        RemoveItemFromInventory(i);
                        itemSlotUIList[i].Reset();
                    }
                    return true;
                }
            }
            return false;
        }
        else
        {
            int removedTillNow = 0;
            for (int i = 0; i < inventoryArray.Length; i++)
            {
                if (item.Equals(inventoryArray[i]))
                {
                    if (item.CurrentStack() < inventoryArray[i].CurrentStack())
                    {
                        if (shouldRemove)
                        {
                            int desiredAfterRemoving = inventoryArray[i].CurrentStack() - item.CurrentStack();
                            inventoryArray[i].SetCurrentStack(desiredAfterRemoving);
                            itemSlotUIList[i].Reset();
                        }
                        return true;
                    }
                    else if (item.CurrentStack() == inventoryArray[i].CurrentStack())
                    {
                        if (shouldRemove)
                        {
                            RemoveItemFromInventory(i);
                            itemSlotUIList[i].Reset();
                        }
                        return true;
                    }
                    else
                    {
                        removedTillNow += inventoryArray[i].CurrentStack();
                        int leftToRemove = item.CurrentStack() - removedTillNow;
                        item.SetCurrentStack(leftToRemove);
                        if (shouldRemove)
                        {
                            RemoveItemFromInventory(i);
                            continue;
                        }
                        continue;
                    }
                }
            }
            if (shouldRemove)
            {
                item.SetCurrentStack(removedTillNow);
                if(removedTillNow > 0)
                    AddItemToInventory(item);
            }
            item.SetCurrentStack(savedItemStack);
            return false;
        }
    }

    /// <summary>
    /// To remove item from the inventory
    /// </summary>
    /// <param name="i"></param>
    public void RemoveItemFromInventory(int i)
    {
        if (!inventoryArray[i].Equals(new EmptyItem()))
        {
            inventoryArray[i] = (new EmptyItem());
            itemSlotUIList[i].Reset();
            CallUIRefreshEvent();
        }
        else
            Debug.LogWarning("Might wanna recheck cause it's null already");
    }

    /// <summary>
    /// Method to check if there is an empty inventory slot
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HaveEmptySlot(ItemBehaviour item, bool shouldAdd)
    {
        ItemBehaviour broughtItem = (ItemBehaviour)item.Clone();
        if (!broughtItem.IsStackable())
        {
            foreach (var itemsInArray in inventoryArray)
            {
                if (itemsInArray.Equals(new EmptyItem()))
                {
                    if (shouldAdd)
                    {
                        AddItemToInventory(broughtItem);
                    }
                    return true;
                }
            }
        }
        else
        {
            foreach (var itemsInArray in inventoryArray)
            {
                if (itemsInArray.Equals(broughtItem))
                {
                    int sum = itemsInArray.CurrentStack() + broughtItem.CurrentStack();
                    if (sum <= broughtItem.MaxStack())
                    {
                        if (shouldAdd)
                            AddItemToInventory(broughtItem);
                        return true;
                    }
                }
                else if (itemsInArray.Equals(new EmptyItem()))
                {
                    if (shouldAdd)
                        AddItemToInventory(broughtItem);
                    return true;
                }
            }
        }
        Debug.Log("No empty slot");
        if (shouldAdd)
        {
            MaxCurrentStacks(broughtItem);
        }
        eventText.CreateNewText("Inventory is full!", TextType.Error);
        return false;
    }

    // The last step of adding an item in to inventory. if the item is stackable this method will
    // check if it can add the related stack to different slots so it can drop the starting item 
    // stack to the minimum value to maximize the stacks of the related object in the invetory.
    private void MaxCurrentStacks(ItemBehaviour item) 
    {        
        int canAdd = 0;
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Equals(item))
            {
                canAdd = inventoryArray[i].MaxStack() - inventoryArray[i].CurrentStack();
                if (canAdd > 0)
                {
                    int currentStack = item.CurrentStack();
                    item.SetCurrentStack(currentStack - canAdd);
                    inventoryArray[i].SetCurrentStack(inventoryArray[i].MaxStack());
                    itemSlotUIList[i].Reset();
                }
            }
        }
        eventManagerRefrence.val.RefreshInventory();
    }

    /// <summary>
    /// Method to return the related icon of the item
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public ItemBehaviour ItemRefrence(int slot)
    {
        return inventoryArray[slot];
    }

    /// <summary>
    /// Method to instantiate item slot for the ui
    /// </summary>
    public void InitUI()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            ItemSlotUI target = child.GetComponentInChildren<ItemSlotUI>();
            target.SetStashable(this);
            target.slotNumber = count;
            itemSlotUIList.Add(target);
            count++;
        }
        return;
        //float width, height;
        //width = GetComponentInParent<RectTransform>().rect.width;
        //height = GetComponentInParent<RectTransform>().rect.height;

        //int maxDevidedWidth = (int)((width) / itemSlotPrefabWidth);
        //int maxDevidedHeight = (int)((height - offsetHeight - startingPointOffsetHeight) / itemSlotPrefabHeight);

        //Debug.Log(itemSlotPrefabHeight);

        //// To automatically turn the extra count of inventory slot available that is more than the 
        //// rows needed to offset.
        //int offsetWidth = 0;
        //if (maxDevidedWidth > rowCountMax)
        //{
        //    int extra = maxDevidedWidth - rowCountMax;
        //    int extraWidth = (int)(extra * itemSlotPrefabWidth);
        //    offsetWidth = extraWidth / rowCountMax;
        //}


        //// for the if checker to see if it's gonna pass the total count of the inventoryslotcount
        //int instantiatedSlots = 0;
        //for (int columnCount = 0; columnCount < maxDevidedHeight; columnCount++)
        //{
        //    //****There is a minus -2 here to make sure that the col count wouldnt go up to ** slots****
        //    for (int rowCount = 0; rowCount < rowCountMax ; rowCount++)
        //    {
        //        GameObject go = Instantiate(itemSlotPrefab);
        //        // To find the starting point to instantiate the slot UI at the proper position
        //        float startingPointWidth = ((width - startingPointOffsetWidth) / 2) * -1;
        //        float startingPointHeight = startingPointOffsetHeight * -1;
        //        Vector2 itemSlotPosition = new Vector2(startingPointWidth + (rowCount * (itemSlotPrefabWidth + offsetWidth))
        //            , startingPointHeight - (columnCount * (itemSlotPrefabHeight + offsetHeight)));
        //        go.transform.localPosition = itemSlotPosition;
        //        go.transform.SetParent(transform, false);
        //        ItemSlotUI itemSlotUI = go.GetComponentInChildren<ItemSlotUI>();
        //        itemSlotUI.SetStashable(this);
        //        itemSlotUI.slotNumber = instantiatedSlots;
        //        itemSlotUIList.Add(itemSlotUI);
        //        instantiatedSlots++;
        //        if (instantiatedSlots == inventorySlotCount)
        //            return;
        //    }
        //}

    }

    #region Save System

    //public void AddISaveableToDictionary()
    //{
    //    saveClassManagerRefrence.val.AddISaveableToDictionary(GetName(), this,0);
    //}

    //public object Save()
    //{
    //    SaveClassesLibrary.PlayerInventory playerInventorySaveData = new(inventorySlotCount, inventoryArray);
    //    return playerInventorySaveData;
    //}

    //public void Load(object savedData)
    //{
    //    SaveClassesLibrary.PlayerInventory saved = (SaveClassesLibrary.PlayerInventory)savedData;
    //    ItemBehaviour[] newInventory = new ItemBehaviour[inventorySlotCount];
    //    List<Type> childTypesList = Assembly.GetAssembly(typeof(ItemBehaviour))
    //    .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(ItemBehaviour))).ToList();
    //    Dictionary<string, ItemBehaviour> hashCodeItemBehaviourDic = new Dictionary<string, ItemBehaviour>();

    //    foreach (var childType in childTypesList)
    //    {
    //        ItemBehaviour target = (ItemBehaviour)Activator.CreateInstance(childType);
    //        string specificName = target.GetName();
    //        hashCodeItemBehaviourDic.Add(specificName, target);
    //    }
        
    //    foreach (var kvp in saved.inventorySlotsSpecificNameDictionary)
    //    {
    //        string specificName = kvp.Value;
    //        int slotNumber = kvp.Key;

    //        if (hashCodeItemBehaviourDic.ContainsKey(specificName))
    //        {
    //            ItemBehaviour itemTarget = hashCodeItemBehaviourDic[specificName];
    //            itemTarget.SetCurrentStack(saved.listCurrentStacks[slotNumber]);
    //            newInventory[slotNumber] = itemTarget;
    //        }
    //    }

    //    for (int i = 0;i<newInventory.Length;i++)
    //    {
    //        inventoryArray[i] = newInventory[i];
    //    }

    //    foreach (var item in itemSlotUIList)
    //    {
    //        item.Reset();
    //    }
    //    CallUIRefreshEvent();


    //}

    //public string GetName()
    //{
    //    return "PlayerInventory";
    //}
    #endregion
}