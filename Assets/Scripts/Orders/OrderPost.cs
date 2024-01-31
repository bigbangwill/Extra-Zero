using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;

public class OrderPost : MonoBehaviour
{

    private Order currentOrder;

    private List<ItemBehaviour> orderableItems;
    

    private bool isReady = false;

    [SerializeField] private OrderPostUI postUI;
    [SerializeField] private Image clock;

    [SerializeField] private List<Transform> quePosList = new();
    [SerializeField] private GameObject walkingOrderPrefab;

    private List<WalkingOrder> walkingOrdersList = new();

    [SerializeField] private float orderMaxTimer;
    private float currentTimer = 0;



    private void Update()
    {
        if (isReady)
        {
            currentTimer += Time.deltaTime;
            float percent = currentTimer / orderMaxTimer;
            clock.fillAmount = percent;
            if(orderMaxTimer < currentTimer )
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

    public WalkingOrder CreateWalkingOrder(int combinationCount,float walkingOrderSpeed)
    {
        GameObject walkingOrderGO = Instantiate(walkingOrderPrefab);
        WalkingOrder walkingOrder = walkingOrderGO.GetComponent<WalkingOrder>();
        walkingOrder.Init(CreateOrder(combinationCount), this, walkingOrderSpeed, quePosList.LastOrDefault().position);
        return walkingOrder;
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
        if (currentOrder != null)
        {
            if (currentOrder.ItemIsEqual(item,slotNumber))
            {
                if (currentOrder != null)
                    postUI.SetOrderImage(currentOrder);
            }
            else
            {
                Debug.Log("Doesnt match");
            }
        }
    }
    
    /// <summary>
    /// Gets called from order class its self to mark the current order to finish.
    /// </summary>
    public void CurrentOrderFullfilled()
    {
        postUI.SetOrderImage(currentOrder);
        RemoveTopWalkingOrder();
    }

    // to implement later to punish the player!.
    private void CouldnotFullfill()
    {
        postUI.SetUnfullfilledOrderImage(currentOrder);
        //Punish Here.
        
        RemoveTopWalkingOrder();


    }

    private void RemoveTopWalkingOrder()
    {
        currentOrder = null;
        isReady = false;
        currentTimer = 0;
        WalkingOrder targetWalkingOrder = walkingOrdersList[0];
        GameObject targetGO = targetWalkingOrder.gameObject;
        OrderManager.Instance.FinishedWalkingOrder(targetWalkingOrder);
        walkingOrdersList.RemoveAt(0);
        Destroy(targetGO);
        if(walkingOrdersList.Count > 0 )
        {
            MoveNext();
        }
    }


    public void AddWalkingOrder(WalkingOrder walkingOrder)
    {
        walkingOrdersList.Add(walkingOrder);
        if (walkingOrdersList.Count < quePosList.Count)
        {
            walkingOrder.SetNextPos(quePosList[walkingOrdersList.Count - 1].position);
        }
        else
        {
            Debug.LogWarning("Check here asap");
        }
    }


    public void WalkingOrderReachedPoint(WalkingOrder walkingOrder)
    {
        if (walkingOrder == walkingOrdersList[0])
        {
            currentOrder = walkingOrder.GetHoldingOrder();
            postUI.SetOrderImage(currentOrder);
            isReady = true;
        }
    }

    public void MoveNext()
    {
        for (int i = 0; i < walkingOrdersList.Count && i < quePosList.Count; i++)
        {
            walkingOrdersList[i].SetNextPos(quePosList[i].position);
        }
    }


    private void RewardforFullfilling()
    {



    }

}