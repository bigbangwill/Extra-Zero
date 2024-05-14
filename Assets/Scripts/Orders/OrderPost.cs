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



    [SerializeField] private OrderPointer pointer;


    private List<WalkingOrder> walkingOrdersList = new();

    [SerializeField] private float orderMaxTimer;
    private float currentTimer = 0;
    private OrderPostHealth postHealthScript;
    private float fasterTimeValue = 0f;
    private float walkingOrderFasterValue = 0f;

    private bool isPrecisionPotionMarked = false;

    private bool isCampaign = false;

    private int currentFullCounter = 0;
    private int maxFullCounter = 10;

    private OrderManagerRefrence orderManagerRefrence;
    private NewTierManager tierManager;
    private EconomyManager economyManager;
    private EventTextManager eventTextManager;
    private RewardBarRefrence rewardBar;

    private void LoadSORefrence()
    {
        orderManagerRefrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
        tierManager = ((NewTierManagerRefrence)FindSORefrence<NewTierManager>.FindScriptableObject("New Tier Manager Refrence")).val;
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
        rewardBar = (RewardBarRefrence)FindSORefrence<BarRewardManager>.FindScriptableObject("Reward Bar Refrence");
    }



    private void Start()
    {
        LoadSORefrence();
        postHealthScript = GetComponent<OrderPostHealth>();
        tierManager.TierChangeAddListener(GetNewOrderList);
        isCampaign = GameModeState.IsCampaignMode;
    }

    private float fadeTimer = 0;
    private int fadeCounter = 2;
    [SerializeField] private int fadeCounterSafe = 15;

    private void Update()
    {
        if (isReady)
        {
            currentTimer += Time.deltaTime + fasterTimeValue;
            fadeTimer += Time.deltaTime;
            float orderTimer = currentOrder.GetOrderTimer();
            float percent = currentTimer / orderTimer;
            float currentFade = fadeTimer * fadeCounter;
            if (currentFade > orderTimer)
            {
                fadeTimer = 0;
                fadeCounter += 2;
                if(fadeCounterSafe > fadeCounter)
                    fadeCounter = fadeCounterSafe;
                pointer.Fade();
            }

            clock.fillAmount = percent;
            if (currentOrder.GetOrderTimer() < currentTimer)
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
            if (isPrecisionPotionMarked || currentFullCounter >= maxFullCounter)
            {
                item = tierManager.GetRandomCurrentMilestoneItem();
                currentFullCounter = 0;
                isPrecisionPotionMarked = false;
            }
            else
            {
                item = orderableItems[random];
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
                    postUI.RefreshUI(currentOrder);
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
        postUI.RefreshUI(currentOrder);
    }


    


    /// <summary>
    /// Gets called from order class its self to mark the current order to finish.
    /// </summary>
    public void CurrentOrderFullfilled()
    {
        postUI.RefreshUI(currentOrder);
        tierManager.MilestoneCheckItem(currentOrder.GetFilledItems());
        currentFullCounter += currentOrder.TotalItemCount();
        Debug.Log("Fullfilled");
        economyManager.QuantumQuartersCurrentStack += 1 * currentOrder.TotalItemCount();
        if(!isCampaign)
            rewardBar.val.AddToBar(1 * currentOrder.TotalItemCount());
        RemoveTopWalkingOrder();
        
    }

    private void CouldnotFullfill()
    {
        postHealthScript.TakeDamage();
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
        pointer.Hide();
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
            fadeCounter = 3;
            pointer.Show();
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