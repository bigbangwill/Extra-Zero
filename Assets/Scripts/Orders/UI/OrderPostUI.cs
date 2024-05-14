using System.Collections.Generic;
using UnityEngine;

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
            order.GetOrderItems()[i].SetOrderShowSlot(i);
            tupleList.Add((order.GetOrderItems()[i], orderSlotShows[i]));
        }
        RefreshUI(order);
    }

    public void RefreshUI(Order order)
    {
        foreach (var item in order.GetOrderItems())
        {
            orderSlotShows[item.GetOrderShowSlot()].SetOrder(item, OrderSlotShow.OrderState.notFilled);
        }

        foreach (var item in order.GetFilledItems())
        {
            orderSlotShows[item.GetOrderShowSlot()].SetOrder(item, OrderSlotShow.OrderState.filled);
        }
    }

    public void SetUnfullfilledOrderImage(Order order)
    {
        foreach (OrderSlotShow show in orderSlotShows)
        {
            show.Reset();
        }

        foreach (var item in order.GetOrderItems())
        {
            orderSlotShows[item.GetOrderShowSlot()].SetOrder(item, OrderSlotShow.OrderState.failed);
        }

        foreach (var item in order.GetFilledItems())
        {
            orderSlotShows[item.GetOrderShowSlot()].SetOrder(item, OrderSlotShow.OrderState.filled);
        }
    }
}