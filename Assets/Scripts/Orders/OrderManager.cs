using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public class OrderManager : MonoBehaviour
{


    [SerializeField] private List<OrderPost> postList = new();
    [SerializeField] private int maxOrderCombination;

    [SerializeField] private WaveChosingUI waveChosingUI;

    [SerializeField] private WaveDifficultySO defaultWave;
    private List<WalkingOrder> activeWalkingOrders = new();
    private WaveDifficultySO currentWaveDifficulty;
    private WaveDifficultySO nextWaveDifficulty;
    private int currentWaveNumber = 1;
    private float waveCurrentTimer = 0;
    private float currentWaveMaxTimer;
    private bool isWaveSpawnTime = false;
    private bool isNightTime = false;
    private float nightCurrentTimer = 0;
    private float nightMaxTimer;

    private int walkingOrdersSummoned = 0;

    private Coroutine currentPendingCoroutine;
    


    private List<ItemBehaviour> orderableItems = new();

    //For later gameplay system :D nice naming btw
    private float timeElapsedBetweenNightAndDay;

    private OrderManagerRefrence refrence;

    private PlayerInventoryRefrence inventoryRefrence;
    private UsableCanvasManagerRefrence usableRefrence;
    private TierManager tierManager;

    private void SetRefrence()
    {
        refrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
        usableRefrence = (UsableCanvasManagerRefrence)FindSORefrence<UseableItemCanvasScript>.FindScriptableObject("Usable Manager Refrence");
        tierManager = ((TierManagerRefrence)FindSORefrence<TierManager>.FindScriptableObject("Tier Manager Refrence")).val;
    }


    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        Init();
        StartNewWave(defaultWave);
    }

    private void Update()
    {
        if (isWaveSpawnTime)
        {
            waveCurrentTimer += Time.deltaTime;
            if(waveCurrentTimer >= currentWaveMaxTimer)
            {
                FinishWave();
            }
            else if (waveCurrentTimer >= (currentWaveDifficulty.GetOrderFrequency() * walkingOrdersSummoned))
            {
                SummonWalkingOrder();
            }
        }
        else if (isNightTime)
        {
            nightCurrentTimer += Time.deltaTime;
            if (nightCurrentTimer >= nightMaxTimer)
            {
                FinishNightTime();
            }
        }
    }


    /// <summary>
    /// This method only gets called when the first wave of the game should spawn.
    /// </summary>
    /// <param name="waveDifficulty"></param>
    public void StartNewWave(WaveDifficultySO waveDifficulty)
    {
        //Debug.Log("It's Day Time");
        currentWaveDifficulty = waveDifficulty;
        currentWaveNumber++;
        waveCurrentTimer = 0;
        currentWaveMaxTimer = currentWaveDifficulty.GetTimerOfWave();
        isWaveSpawnTime = true;
        walkingOrdersSummoned = 0;
        SummonWalkingOrder();
    }

    // method to get called to summon walking order.
    private void SummonWalkingOrder()
    {
        int orderCombination = currentWaveDifficulty.GetOrderCombination();
        float walkingSpeed = currentWaveDifficulty.GetWalkingOrderSpeed();
        int randomPost = UnityEngine.Random.Range(0, postList.Count);

        WalkingOrder targetOrder = postList[randomPost].CreateWalkingOrder(
            orderCombination,
            walkingSpeed,
            currentWaveDifficulty.GetOrderFulfillTimer());
        activeWalkingOrders.Add(targetOrder);
        walkingOrdersSummoned++;
    }


    private void FinishWave()
    {
        isWaveSpawnTime = false;
        currentPendingCoroutine = StartCoroutine(WaitForPendingOrders());
    }

    private void StartNightTime()
    {
        if (currentPendingCoroutine != null)
        {
            StopCoroutine(currentPendingCoroutine);
        }
        isNightTime = true;
        nightCurrentTimer = 0;
        nightMaxTimer = currentWaveDifficulty.GetNightMaxTime();
        waveChosingUI.CreateWaveOptionUI();
        //Debug.Log("It's Night TIME ");
    }

    private void FinishNightTime()
    {
        isNightTime = false;
        waveChosingUI.GetNextWaveAndExecuteEffects();
    }

    public void FinishedWalkingOrder(WalkingOrder walkingOrder)
    {
        activeWalkingOrders.Remove(walkingOrder);
        if(activeWalkingOrders.Count == 0 )
        {
            //Debug.Log("Wave Finished");
        }
    }


    private IEnumerator WaitForPendingOrders()
    {
        while (true)
        {
            timeElapsedBetweenNightAndDay += Time.deltaTime;
            if (activeWalkingOrders.Count == 0)
            {
                timeElapsedBetweenNightAndDay = 0;
                StartNightTime();
            }
            yield return null;
        }
    }

    // To add all of the createable items to the list.
    private void Init()
    {
        foreach (var post in postList)
        {
            post.InitList(tierManager.GetNewTierCraftedItemList());
        }

    }


    public int GetMaxOrderCombination()
    {
        return maxOrderCombination;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Add Hammer"))
        {
            inventoryRefrence.val.HaveEmptySlot(new CraftedItem.RepairHammer(usableRefrence.val.transform),true);
        }
    }

}
