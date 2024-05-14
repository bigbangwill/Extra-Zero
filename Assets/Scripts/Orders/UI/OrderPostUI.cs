using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class OrderPostUI : MonoBehaviour
{
    [SerializeField] private List<OrderSlotShow> orderSlotShows = new();

    private List<ItemBehaviour> holdingList = new();

    private List<(ItemBehaviour, OrderSlotShow)> tupleList = new();



    /// <summary>
    /// To create the related prefab in the world canvas and set their icons.
    /// </summary>
    /// <param name="order"></param>
    public void SetOrderImage(Order order)
    {
        holdingList.Clear();
        foreach(OrderSlotShow show in orderSlotShows)
        {
            show.Reset();
        }
        foreach (ItemBehaviour item in order.GetOrderItems())
        {
            holdingList.Add(item);
        }
        for (int i = 0; i < order.GetOrderItems().Count; i++)
        {
            Debug.Log(i);
            Debug.Log(order.GetOrderItems()[i]);
            Debug.Log(orderSlotShows[i]);
            tupleList.Add((order.GetOrderItems()[i], orderSlotShows[i]));
        }
        RefreshUI(order);
    }



    public void RefreshUI(Order order)
    {
        foreach (var item in order.GetOrderItems())
        {
            foreach (var orderSlot in orderSlotShows)
            {
                foreach (var tuple in tupleList)
                {
                    if ((item, orderSlot).Equals(tuple))
                    {
                        tuple.Item2.SetOrder(item,OrderSlotShow.OrderState.notFilled);
                    }
                }
            }
        }

        foreach (var item in order.GetFilledItems())
        {
            foreach (var orderSlot in orderSlotShows)
            {
                foreach (var tuple in tupleList)
                {
                    if ((item, orderSlot).Equals(tuple))
                    {
                        tuple.Item2.SetOrder(item, OrderSlotShow.OrderState.filled);
                    }
                }
            }
        }
    }



    public void OldRefreshUI(Order order)
    {
        foreach (OrderSlotShow show in orderSlotShows)
        {
            show.Reset();
        }
        foreach (ItemBehaviour item in order.GetOrderItems())
        {
            for (int i = 0; i < holdingList.Count; i++)
            {
                if (item.Equals(holdingList[i]))
                {
                    orderSlotShows[i].SetOrder(item, OrderSlotShow.OrderState.notFilled);
                    //break;
                }
            }
        }
        foreach (ItemBehaviour item in order.GetFilledItems())
        {
            for (int i = 0; i < holdingList.Count; i++)
            {
                if (item.Equals(holdingList[i]))
                {
                    orderSlotShows[i].SetOrder(item, OrderSlotShow.OrderState.filled);
                    //break;
                }
            }
        }
    }


    public void SetUnfullfilledOrderImage(Order order)
    {
        foreach (OrderSlotShow show in orderSlotShows)
        {
            show.Reset();
        }
        foreach (ItemBehaviour item in order.GetOrderItems())
        {
            for (int i = 0; i < holdingList.Count; i++)
            {
                if (item.Equals(holdingList[i]))
                {
                    orderSlotShows[i].SetOrder(item, OrderSlotShow.OrderState.notFilled);
                    //break;
                }
            }
        }
        foreach (ItemBehaviour item in order.GetFilledItems())
        {

            for (int i = 0; i < holdingList.Count; i++)
            {
                if (item.Equals(holdingList[i]))
                {
                    orderSlotShows[i].SetOrder(item, OrderSlotShow.OrderState.filled);
                    //break;
                }
            }
        }
    }
    
}