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

    private Queue<Order> orderQue = new();
    

    private bool isReady = false;
    private bool isFullfilled = false;

    [SerializeField] private OrderPostUI postUI;
    [SerializeField] private Image clock;

    private float timeBetweenOrders = 0;
    private float currentTimer = 0;

    private bool isFinished = false;



    private void Start()
    {
        

        CreateOrderQue(10, 5, 1);
    }

    public void InitList(List<ItemBehaviour> orderableList)
    {
        orderableItems = orderableList;

    }


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


    public void CreateOrderQue(float timer, int count, int combinationCount)
    {
        timeBetweenOrders = timer;
        isFinished = false;
        for(int i = 0; i < count; i++)
        {
            orderQue.Enqueue(CreateOrder(combinationCount));
        }
        currentOrder = orderQue.Dequeue();
        isReady = true;
        isFullfilled = false;
        postUI.SetOrderImage(currentOrder);
    }

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

    private void TimerHit()
    {
        currentTimer = 0;
        if (currentOrder != null)
        {
            CouldnotFullfill();
        }
        if (orderQue.Count > 0)
        {
            currentOrder = orderQue.Dequeue();
            postUI.SetOrderImage(currentOrder);
            isFullfilled = false;
        }
        else
        {
            Debug.Log("Filled Alll");
            isReady = false;
        }
    }



    public void InsertingItem(ItemBehaviour item,int slotNumber)
    {
        if (!isFullfilled)
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
    
    public void CurrentOrderFullfilled()
    {
        fullfilledOrder = currentOrder;
        isFullfilled = true;
        currentOrder = null;
        if (orderQue.Count == 0)
        {
            isFinished = true;
        }
        OrderManager.Instance.CheckIfAllIsDone();

    }

    public bool OrdersAreComplete()
    {
        return isFinished;
    }

    private void CouldnotFullfill()
    {
        Debug.Log("Couldnt fullfill");
    }
}