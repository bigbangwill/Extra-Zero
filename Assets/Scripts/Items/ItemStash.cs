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


    private void Start()
    {
        inventoryArray = new ItemBehaviour[inventorySlotCount];
        itemSlotPrefabWidth = itemSlotPrefab.GetComponent<RectTransform>().rect.width;
        itemSlotPrefabHeight = itemSlotPrefab.GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            inventoryArray[i] = new EmptyItem();
        }

        InitUI();
        Debug.Log("Start");
    }

    public void RefreshUI()
    {

    }

    public ItemBehaviour ItemRefrence(int slot)
    {
        return inventoryArray[slot];
    }

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
        return false;
    }

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
        Debug.Log(width + "Width");
        Debug.Log(height + "Height");

        int maxDevidedWidth = (int)((width) / itemSlotPrefabWidth);
        int maxDevidedHeight = (int)((height - offsetHeight - startingPointOffsetHeight) / itemSlotPrefabHeight);
        Debug.Log(itemSlotPrefabHeight);

        // To automatically turn the extra count of inventory slot available that is more than the 
        // rows needed to offset.
        int offsetWidth = 0;
        if (maxDevidedWidth > rowCountMax)
        {
            int extra = maxDevidedWidth - rowCountMax;
            int extraWidth = (int)(extra * itemSlotPrefabWidth);
            offsetWidth = extraWidth / rowCountMax;
            Debug.Log("Here");
        }


        // for the if checker to see if it's gonna pass the total count of the inventoryslotcount
        int instantiatedSlots = 0;
        for (int columnCount = 0; columnCount < maxDevidedHeight; columnCount++)
        {
            Debug.Log("Here");
            //****There is a minus -2 here to make sure that the col count wouldnt go up to ** slots****
            for (int rowCount = 0; rowCount < rowCountMax; rowCount++)
            {
                Debug.Log("Here");
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
