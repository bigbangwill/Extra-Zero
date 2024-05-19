using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStash : MonoBehaviour, IStashable
{

    // Player inventory slot count
    [SerializeField] private int inventorySlotCount;
    // For the Item slot prefab in the UI
    [SerializeField] private GameObject itemSlotPrefab;



    [SerializeField] private ItemBehaviour[] inventoryArray;
    [SerializeField] private List<ItemSlotUI> itemSlotUIList = new();

    // Variable for the current active item
    private ItemBehaviour currentActiveItem;
    private int currentActiveItemSlotNum = int.MaxValue;
    [SerializeField] private Transform activeItemTranform;

    private PlayerInventoryRefrence inventoryRefrence;
    private EventManagerRefrence eventManagerRefrence;
    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
    }

    private void Awake()
    {
        LoadSORefrence();
        inventoryArray = new ItemBehaviour[inventorySlotCount];
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            inventoryArray[i] = new EmptyItem();
        }

        InitUI();
    }

    private void OnEnable()
    {
        eventManagerRefrence.val.RefreshUIAddListener(RefreshUI);
    }

    private void OnDisable()
    {
        eventManagerRefrence.val.RefreshUIRemoveListener(RefreshUI);
    }


    public void RefreshUI()
    {
        foreach (var slotUI in itemSlotUIList)
        {
            slotUI.Reset();
        }
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
    /// An overall method to check if the item is stackable or not to navigate to the right related method
    /// </summary>
    /// <param name="item"></param>
    public void AddItemToInventory(ItemBehaviour item)
    {
        if (item.IsStackable())
            AddStackableItemToInventory(item);
        else
            AddNonStackableItemToInventory(item);
        RefreshUI();
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
    /// Method to get called from outer scope (the ui) to set the related item slot int to
    /// be the current active item
    /// </summary>
    /// <param name="slot"></param>
    public void SetActiveItem(int slot)
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
            Debug.Log("here");
            if (!inventoryRefrence.val.HaveEmptySlot(currentActiveItem, true))
            {
                Debug.Log("Dont have empty Slot");
            }
            else
            {
                RemoveItemFromInventory(currentActiveItemSlotNum);
            }

            currentActiveItem = null;
            currentActiveItemSlotNum = int.MaxValue;
            activeItemTranform.gameObject.SetActive(false);
        }
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
                        item.SetCurrentStack(sub);
                        itemSlotUIList[i].Reset();
                        AddNonStackableItemToInventory(item);
                        return;
                    }
                }
            }
        }
        AddNonStackableItemToInventory(item);
    }


    /// <summary>
    /// Method to check if there is an empty inventory slot
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HaveEmptySlot(ItemBehaviour item, bool shouldAdd)
    {
        if (!item.IsStackable())
        {
            foreach (var itemsInArray in inventoryArray)
            {
                if (itemsInArray.Equals(new EmptyItem()))
                {
                    if (shouldAdd)
                        AddItemToInventory(item);
                    return true;
                }

            }
        }
        else
        {
            foreach (var itemsInArray in inventoryArray)
            {
                if (itemsInArray.Equals(item))
                {
                    int sum = itemsInArray.CurrentStack() + item.CurrentStack();
                    if (sum <= item.MaxStack())
                    {
                        if (shouldAdd)
                            AddItemToInventory(item);
                        return true;
                    }
                }
                else if (itemsInArray.Equals(new EmptyItem()))
                {
                    if (shouldAdd)
                        AddItemToInventory(item);
                    return true;
                }
            }
        }
        Debug.Log("No empty slot");
        if (shouldAdd)
        {
            MaxCurrentStacks(item);
        }
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
    /// A method mainly for quests to check if the player has an specific item
    /// or a stack of an item.the bool shouldRemove will be determine if the
    /// if the item should be removed after it exists or not.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="shouldRemove"></param>
    /// <returns></returns>
    public bool HaveItemInInventory(ItemBehaviour item, bool shouldRemove)
    {
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
                if (removedTillNow > 0)
                    AddItemToInventory(item);
            }
            return false;
        }
    }

    public void InitUI()
    {
        for (int count = 0; count < inventorySlotCount; count++)
        {
            GameObject go = Instantiate(itemSlotPrefab);
            go.transform.SetParent(transform, false);
            ItemSlotUI itemSlotUI = go.GetComponentInChildren<ItemSlotUI>();
            itemSlotUI.SetStashable(this);
            itemSlotUI.slotNumber = count;
            itemSlotUIList.Add(itemSlotUI);
        }
    }

    public void RemoveItemFromInventory(int i)
    {
        if (!inventoryArray[i].Equals(new EmptyItem()))
        {
            inventoryArray[i] = (new EmptyItem());
            itemSlotUIList[i].Reset();
            RefreshUI();
        }
        else
            Debug.LogWarning("Might wanna recheck cause it's null already");
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
    /// Method to get called from outer scope to (the ui) to swap a and b item slots
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void SwapItemInInventory(int a, int b)
    {
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
}
