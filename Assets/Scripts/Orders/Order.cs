using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private List<ItemBehaviour> orderItems;
    private List<ItemBehaviour> fulfilledItems = new();

    private OrderPost relatedPost;


    public Order(List<ItemBehaviour> orders,OrderPost post)
    {
        orderItems = orders;
        relatedPost = post;
    }

    public int OrderItemCount()
    {
        return orderItems.Count;
    }

    public int TotalItemCount()
    {
        return orderItems.Count + fulfilledItems.Count;
    }

    public bool ItemIsEqual(ItemBehaviour item,int slotNumber)
    {
        foreach (ItemBehaviour orderItem in orderItems)
        {
            if (orderItem.Equals(item))
            {
                TryToFill(item,orderItem, slotNumber);
                return true;
            }
        }
        return false;
    }



    public void TryToFill(ItemBehaviour inventoryItem,ItemBehaviour orderItem,int slotNumber)
    {
        int inventoryStack = inventoryItem.CurrentStack();
        int orderStack = orderItem.CurrentStack();
        PlayerInventory.Instance.RemoveFromSlotNumber(slotNumber, orderStack);
        if (inventoryStack >= orderStack)
        {
            ItemMatched(orderItem);
        }
        else
        {
            orderItem.SetCurrentStack(orderStack - inventoryStack);
        }
    }



    private void ItemMatched(ItemBehaviour orderItem)
    {
        orderItems.Remove(orderItem);
        fulfilledItems.Add(orderItem);
        if (orderItems.Count == 0)
        {
            Fullfilled();
        }

    }

    private void Fullfilled()
    {
        relatedPost.CurrentOrderFullfilled();
    }



}