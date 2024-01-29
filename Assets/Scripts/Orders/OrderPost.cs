using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;
using Unity.VisualScripting;

public class OrderPost : MonoBehaviour
{

    private Order currentOrder;
    private Order fullfilledOrder;

    private List<ItemBehaviour> orderableItems;
    

    private bool isReady = false;
    private bool isFullfilled = false;

    [SerializeField] private OrderPostUI postUI;
    [SerializeField] private Image clock;

    [SerializeField] private Transform targetPos;
    [SerializeField] private List<Transform> quePosList = new();

    private float timeBetweenOrders = 0;
    private float currentTimer = 0;

    private bool isFinished = false;


    private void Update()
    {
        if (isReady)
        {
            currentTimer += Time.deltaTime;
            float percent = currentTimer / timeBetweenOrders;
            clock.fillAmount = percent;
            if(timeBetweenOrders < currentTimer )
            {
                TimerHit();
            }
        }
    }

    /// <summary>
    /// To get called from the order manager to set it's createable items list.
    /// </summary>
    /// <param name="orderableList"></param>
    public void InitList(List<ItemBehaviour> orderableList)
    {
        orderableItems = orderableList;

    }

    /// <summary>
    /// To start the creating of orders and put them in the que. gets called from order manager.
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    /// <param name="combinationCount"></param>
    public void CreateOrderQue(int combinationCount)
    {
        isFinished = false;
        currentOrder = CreateOrder(combinationCount);
        isReady = true;
        isFullfilled = false;
        postUI.SetOrderImage(currentOrder);
    }

    // for creating a single order.
    private Order CreateOrder(int combinationCount)
    {
        List<ItemBehaviour> targetItems = new();
        for (int i = 0; i < combinationCount; i++)
        {
            int random = UnityEngine.Random.Range(0, orderableItems.Count);
            ItemBehaviour item = orderableItems[random];
            item.Load();
            if (item.IsStackable())
            {
                item.SetCurrentStack(3);
            }
            targetItems.Add(orderableItems[random]);
        }
        Order creatingOrder = new(targetItems, this);
        return creatingOrder;
    }

    // method to trigger when the timer hit the max time.
    private void TimerHit()
    {
        currentTimer = 0;
        if (currentOrder != null)
        {
            CouldnotFullfill();
        }
        else
        {
            Debug.Log("Filled Alll");
            isReady = false;
        }
    }


    /// <summary>
    /// Method for raycast answer.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotNumber"></param>
    public void InsertingItem(ItemBehaviour item,int slotNumber)
    {
        if (!isFullfilled && !isFinished)
        {
            if (currentOrder.ItemIsEqual(item,slotNumber))
            {
                if (currentOrder == null)
                    postUI.SetOrderImage(fullfilledOrder);
                else
                    postUI.SetOrderImage(currentOrder);
            }
            else
            {
                Debug.Log("Doesnt match");
            }
        }
        else
        {
            postUI.SetOrderImage(fullfilledOrder);
        }
    }
    
    /// <summary>
    /// Gets called from order class its self to mark the current order to finish.
    /// </summary>
    public void CurrentOrderFullfilled()
    {
        fullfilledOrder = currentOrder;
        isFullfilled = true;
        currentOrder = null;
        isFinished = true;
        OrderManager.Instance.PostOrderCompleted();
        

    }

    /// <summary>
    /// gets called from the ordermanager to check if all of the posts are finished.
    /// </summary>
    /// <returns></returns>
    public bool OrdersAreComplete()
    {
        return isFinished;
    }


    // to implement later to punish the player!.
    private void CouldnotFullfill()
    {
        isFinished = true;
        OrderManager.Instance.PostOrderCompleted();
    }

    private void RewardforFullfilling()
    {

    }

}