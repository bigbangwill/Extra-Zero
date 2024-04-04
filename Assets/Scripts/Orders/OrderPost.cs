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
    [SerializeField] private Transform WalkingOrderDeathSpot;


    [SerializeField] private Transform reaching;


    private List<WalkingOrder> walkingOrdersList = new();

    [SerializeField] private float orderMaxTimer;
    private float currentTimer = 0;
    private OrderPostHealth postHealthScript;
    private float fasterTimeValue = 0f;
    private float walkingOrderFasterValue = 0f;

    private bool isPrecisionPotionMarked = false;

    private OrderManagerRefrence orderManagerRefrence;
    private NewTierManager tierManager;
    private EconomyManager economyManager;
    private EventTextManager eventTextManager;

    private void LoadSORefrence()
    {
        orderManagerRefrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
        tierManager = ((NewTierManagerRefrence)FindSORefrence<NewTierManager>.FindScriptableObject("New Tier Manager Refrence")).val;
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
    }



    private void Start()
    {
        LoadSORefrence();
        postHealthScript = GetComponent<OrderPostHealth>();
        tierManager.TierChangeAddListener(GetNewOrderList);
    }


    private void Update()
    {
        if (isReady)
        {
            currentTimer += Time.deltaTime + fasterTimeValue;
            float percent = currentTimer / currentOrder.GetOrderTimer();
            clock.fillAmount = percent;
            if(currentOrder.GetOrderTimer() < currentTimer )
            {
                TimerHit();
            }
        }
    }


    public void StrenghtTimeAmount(float speed)
    {
        fasterTimeValue = speed;
    }

    public void StrenghtTimeReset()
    {
        fasterTimeValue = 0;
    }

    public void SpeedTimeAmount(float speed)
    {
        walkingOrderFasterValue = speed;
    }

    public void SpeedTimeReset()
    {
        walkingOrderFasterValue = 0;
    }

    public void PrecisionBuff()
    {
        isPrecisionPotionMarked = true;
    }



    /// <summary>
    /// To get called from the order manager to set it's createable items list.
    /// </summary>
    /// <param name="orderableList"></param>
    public void InitList(List<ItemBehaviour> orderableList)
    {
        orderableItems = orderableList;
    }

    public void GetNewOrderList()
    {
        orderableItems = tierManager.GetNewTierCraftedItemList();
    }

    public WalkingOrder CreateWalkingOrder(int combinationCount,float walkingOrderSpeed,float orderTimer)
    {
        
        GameObject walkingOrderGO = Instantiate(walkingOrderPrefab);
        WalkingOrder walkingOrder = walkingOrderGO.GetComponent<WalkingOrder>();
        walkingOrder.Init(
            CreateOrder(combinationCount, orderTimer),
            this,
            walkingOrderSpeed + walkingOrderFasterValue,
            quePosList.LastOrDefault().position);
        return walkingOrder;
    }

    

    // for creating a single order.
    private Order CreateOrder(int combinationCount, float orderTimer)
    {
        List<ItemBehaviour> targetItems = new();
        for (int i = 0; i < combinationCount; i++)
        {
            int random = UnityEngine.Random.Range(0, orderableItems.Count);
            ItemBehaviour item; 
            if (!isPrecisionPotionMarked)
            {
                item = orderableItems[random];
            }
            else
            {
                item = tierManager.GetRandomCurrentMilestoneItem();
                isPrecisionPotionMarked = false;
            }

            item.Load();
            if (item.IsStackable())
            {
                item.SetCurrentStack(3);
            }
            targetItems.Add(orderableItems[random]);
        }
        Order creatingOrder = new(targetItems, this,orderTimer);
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
                {
                    postUI.SetOrderImage(currentOrder);
                    return;
                }
            }
            else
            {
                eventTextManager.CreateNewText("Doesn't match the order", TextType.Error);
            }
        }
    }

    public void RefreshUI()
    {
        postUI.SetOrderImage(currentOrder);
    }
    
    /// <summary>
    /// Gets called from order class its self to mark the current order to finish.
    /// </summary>
    public void CurrentOrderFullfilled()
    {
        postUI.SetOrderImage(currentOrder);
        tierManager.MilestoneCheckItem(currentOrder.GetFilledItems());
        Debug.Log("Fullfilled");
        economyManager.QuantumQuartersCurrentStack += 1 * currentOrder.TotalItemCount();
        RemoveTopWalkingOrder();
        
    }

    private void CouldnotFullfill()
    {
        //postHealthScript.TakeDamage();
        postUI.SetUnfullfilledOrderImage(currentOrder);        
        RemoveTopWalkingOrder();
    }

    private void RemoveTopWalkingOrder()
    {
        currentOrder = null;
        isReady = false;
        currentTimer = 0;
        WalkingOrder targetWalkingOrder = walkingOrdersList[0];
        orderManagerRefrence.val.FinishedWalkingOrder(targetWalkingOrder);
        walkingOrdersList.RemoveAt(0);
        targetWalkingOrder.WalkToDeath(WalkingOrderDeathSpot.position);
        if(walkingOrdersList.Count > 0 )
        {
            MoveNext();
        }
    }

    public Order GetCurrentOrder()
    {
        return currentOrder;
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

    public Transform GetReachingTransfrom()
    {
        return reaching;
    }
}