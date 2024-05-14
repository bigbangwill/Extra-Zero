using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Order
{





    private List<ItemBehaviour> orderItems = new();
    private List<ItemBehaviour> fulfilledItems = new();

    private List<ItemBehaviour> unFulFilledItems = new();

    private OrderPost relatedPost;
    private float orderFulfillTimer;
    private PlayerInventoryRefrence inventoryRefrence;
    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
    }

    public Order(List<ItemBehaviour> orders,OrderPost post, float orderFulfillTimer)
    {
        relatedPost = post;
        foreach (ItemBehaviour item in orders)
        {
            orderItems.Add(item);
        }

        this.orderFulfillTimer = orderFulfillTimer;
        LoadSORefrence();
    }

    /// <summary>
    /// To return the current items that are not finished.
    /// </summary>
    /// <returns></returns>
    public List<ItemBehaviour> GetOrderItems()
    {
        return orderItems;
    }
    
    /// <summary>
    /// To return the finished item.
    /// </summary>
    /// <returns></returns>
    public List<ItemBehaviour> GetFilledItems()
    {
        return fulfilledItems;
    }


    public int OrderItemCount()
    {
        return orderItems.Count;
    }

    public int TotalItemCount()
    {
        return orderItems.Count + fulfilledItems.Count;
    }


    /// <summary>
    /// To check if the raycast hit item is equal to this item.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotNumber"></param>
    /// <returns></returns>
    public bool ItemIsEqual(ItemBehaviour item,int slotNumber)
    {
        foreach (ItemBehaviour orderItem in orderItems)
        {
            if (item.GetItemTypeValue() == ItemType.potion && orderItem.GetItemTypeValue() == ItemType.potion)
            {
                if (((PotionItem)item).firstEffect.Equals(((PotionItem)orderItem).firstEffect))
                {
                    TryToFill(item,orderItem,slotNumber);
                    return true;
                }
            }
            if (orderItem.Equals(item))
            {
                TryToFill(item,orderItem, slotNumber);
                return true;
            }
        }
        return false;
    }


    // if the item is equals this method gets called to set the current stack of the item.
    private void TryToFill(ItemBehaviour inventoryItem,ItemBehaviour orderItem,int slotNumber)
    {
        int inventoryStack = inventoryItem.CurrentStack();
        int orderStack = orderItem.CurrentStack();
        inventoryRefrence.val.RemoveFromSlotNumber(slotNumber, orderStack);
        if (inventoryStack >= orderStack)
        {
            ItemMatched(orderItem);
        }
        else
        {
            orderItem.SetCurrentStack(orderStack - inventoryStack);
            relatedPost.RefreshUI();
        }
    }


    // this gets called if the item is fully met its requirement.
    private void ItemMatched(ItemBehaviour orderItem)
    {
        orderItems.Remove(orderItem);
        fulfilledItems.Add(orderItem);
        if (orderItems.Count == 0)
        {
            Fullfilled();
        }
        foreach (var item in fulfilledItems)
        {
            if (item.GetItemTypeValue() == ItemType.potion)
            {
                item.Use();
            }
        }

    }


    // will get called when it's fullfilled !!!!!!
    private void Fullfilled()
    {
        relatedPost.CurrentOrderFullfilled();
    }

    public float GetOrderTimer()
    {
        return orderFulfillTimer;
    }


}