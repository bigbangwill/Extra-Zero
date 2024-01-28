using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    private List<ItemBehaviour> orderItems = new();
    private List<ItemBehaviour> fulfilledItems = new();

    private OrderPost relatedPost;


    public Order(List<ItemBehaviour> orders,OrderPost post)
    {
        relatedPost = post;
        foreach (ItemBehaviour item in orders)
        {
            orderItems.Add(item);
        }
    }

    public IEnumerable<ItemBehaviour> GetOrderItems()
    {
        if (orderItems == null)
            Debug.Log("KJBHOIJKQHGWRKIJHBQEKJRHBQWK:JRHQWKL:JHRELKJQWHBE");
        return orderItems;
    }
    
    public IEnumerable<ItemBehaviour> GetFilledItems()
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