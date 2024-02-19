using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStash : MonoBehaviour, IStashable
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


    private void Awake()
    {

        EventManager.Instance.RefreshUIAddListener(RefreshUI);
        inventoryArray = new ItemBehaviour[inventorySlotCount];
        itemSlotPrefabWidth = itemSlotPrefab.GetComponent<RectTransform>().rect.width;
        itemSlotPrefabHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            inventoryArray[i] = new EmptyItem();
        }

        InitUI();

        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
        HaveEmptySlot(new Herb.Lavender(30), true);
        HaveEmptySlot(new Herb.Sage(30), true);
        HaveEmptySlot(new Herb.Chamomile(30), true);
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
        if (inventoryArray[slot].ItemTypeValue() == ItemType.empty)
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
            if (!PlayerInventory.Instance.HaveEmptySlot(currentActiveItem, true))
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
        EventManager.Instance.RefreshInventory();
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
        float width, height;
        width = GetComponentInParent<RectTransform>().rect.width;
        height = GetComponentInParent<RectTransform>().rect.height;

        int maxDevidedWidth = (int)((width) / itemSlotPrefabWidth);
        int maxDevidedHeight = (int)((height - offsetHeight - startingPointOffsetHeight) / itemSlotPrefabHeight);

        // To automatically turn the extra count of inventory slot available that is more than the 
        // rows needed to offset.
        int offsetWidth = 0;
        if (maxDevidedWidth > rowCountMax)
        {
            int extra = maxDevidedWidth - rowCountMax;
            int extraWidth = (int)(extra * itemSlotPrefabWidth);
            offsetWidth = extraWidth / rowCountMax;
        }


        // for the if checker to see if it's gonna pass the total count of the inventoryslotcount
        int instantiatedSlots = 0;
        for (int columnCount = 0; columnCount < maxDevidedHeight; columnCount++)
        {
            //****There is a minus -2 here to make sure that the col count wouldnt go up to ** slots****
            for (int rowCount = 0; rowCount < rowCountMax; rowCount++)
            {
                GameObject go = Instantiate(itemSlotPrefab);
                // To find the starting point to instantiate the slot UI at the proper position
                float startingPointWidth = ((width - startingPointOffsetWidth) / 2) * -1;
                float startingPointHeight = startingPointOffsetHeight * -1;
                Vector2 itemSlotPosition = new Vector2(startingPointWidth + (rowCount * (itemSlotPrefabWidth + offsetWidth))
                    , startingPointHeight - (columnCount * (itemSlotPrefabHeight + offsetHeight)));
                go.transform.localPosition = itemSlotPosition;
                go.transform.SetParent(transform, false);
                ItemSlotUI itemSlotUI = go.GetComponentInChildren<ItemSlotUI>();
                itemSlotUI.SetStashable(this);
                itemSlotUI.slotNumber = instantiatedSlots;
                itemSlotUIList.Add(itemSlotUI);
                instantiatedSlots++;
                if (instantiatedSlots == inventorySlotCount)
                    return;
            }
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
            if (item.ItemTypeValue() == _ItemType)
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
        Debug.Log(inventoryArray[a].ItemTypeValue());
        Debug.Log(inventoryArray[b].ItemTypeValue());
    }
}
