using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public class OrderManager : SingletonComponent<OrderManager>
{
    #region Sinleton
    public static OrderManager Instance
    {
        get { return ((OrderManager)_Instance); }
        set { _Instance = value; }
    }
    #endregion


    [SerializeField] private List<OrderPost> postList = new();
    [SerializeField] private int maxOrderCombination;

    [SerializeField] private List<WaveDifficultySO> waveDifficultyList = new();
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

    private void Start()
    {
        Init();
        SetNextWaveDifficulty();
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
            else if (waveCurrentTimer >= (currentWaveDifficulty.GetOrderFrequency(currentWaveNumber) * walkingOrdersSummoned))
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



    private void SetNextWaveDifficulty()
    {
        nextWaveDifficulty = waveDifficultyList[UnityEngine.Random.Range(0, waveDifficultyList.Count)];
    }

    public void StartNewWave()
    {
        Debug.Log("It's Day Time");
        currentWaveDifficulty = nextWaveDifficulty;
        SetNextWaveDifficulty();
        currentWaveNumber++;
        waveCurrentTimer = 0;
        currentWaveMaxTimer = currentWaveDifficulty.GetTimerOfWave(currentWaveNumber);
        isWaveSpawnTime = true;
        walkingOrdersSummoned = 0;
        SummonWalkingOrder();
    }

    private void SummonWalkingOrder()
    {
        int orderCombination = currentWaveDifficulty.GetOrderCombination();
        float walkingSpeed = currentWaveDifficulty.GetWalkingOrderSpeed();
        int randomPost = UnityEngine.Random.Range(0, postList.Count);

        WalkingOrder targetOrder = postList[randomPost].CreateWalkingOrder(
            orderCombination,
            walkingSpeed,
            currentWaveDifficulty.GetOrderFulfillTimer(currentWaveNumber));
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
        Debug.Log("It's Night TIME ");
    }

    private void FinishNightTime()
    {
        isNightTime = false;
        StartNewWave();
    }

    public void FinishedWalkingOrder(WalkingOrder walkingOrder)
    {
        activeWalkingOrders.Remove(walkingOrder);
        if(activeWalkingOrders.Count == 0 )
        {
            Debug.Log("Wave Finished");
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
        List<Type> childTypesList = Assembly.GetAssembly(typeof(ItemBehaviour))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType != typeof(PotionItem)
        && !typeof(BluePrintItem).IsAssignableFrom(TheType) && TheType != typeof(EmptyItem)
        && TheType.IsSubclassOf(typeof(ItemBehaviour))).ToList();

        

        foreach (var item in childTypesList)
        {
            ItemBehaviour createdItem = Activator.CreateInstance(item) as ItemBehaviour;
            orderableItems.Add(createdItem);
        }

        foreach (var post in postList)
        {
            post.InitList(orderableItems);
        }

    }


    public int GetMaxOrderCombination()
    {
        return maxOrderCombination;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "StartWave"))
        {
            StartNewWave();
        }
    }

}
