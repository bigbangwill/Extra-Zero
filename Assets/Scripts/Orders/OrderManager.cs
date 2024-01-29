using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

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
    [SerializeField] private float waveMaxTimer;



    private int currentWave = 1;
    private float waveCurrentTimer = 0;
    private bool isWaitingForNextWave = false;


    private void Start()
    {
        Init();
        StartWaveTimer();
    }

    private void Update()
    {
        if (isWaitingForNextWave)
        {
            waveCurrentTimer += Time.deltaTime;
            if (waveCurrentTimer > waveMaxTimer)
            {
                waveCurrentTimer = 0;
                StartNextWave();
            }
        }
    }

    /// <summary>
    /// To get called from the post to do a check if all the posts are completed.
    /// </summary>
    public void PostOrderCompleted()
    {
        if (IsWaveComplete())
        {
            StartWaveTimer();
        }
    }



    /// <summary>
    /// Method to start the next wave.
    /// </summary>
    public void StartNextWave()
    {
        isWaitingForNextWave = false;
        currentWave++;

        foreach(OrderPost post in postList)
        {
            post.CreateOrderQue(1);
        }
    }


    // To start the timer between each wave.
    private void StartWaveTimer()
    {
        isWaitingForNextWave = true;
    }

    // checker to check if all of the posts are done.
    private bool IsWaveComplete()
    {
        bool checker = true;

        foreach (OrderPost post in postList)
        {
            if (!post.OrdersAreComplete())
            {
                checker = false;
            }
        }
        return checker;
    }



    // To add all of the createable items to the list.
    private void Init()
    {
        List<Type> childTypesList = Assembly.GetAssembly(typeof(ItemBehaviour))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType != typeof(PotionItem)
        && !typeof(BluePrintItem).IsAssignableFrom(TheType) && TheType != typeof(EmptyItem)
        && TheType.IsSubclassOf(typeof(ItemBehaviour))).ToList();

        List<ItemBehaviour> orderableItemsList = new();

        foreach (var item in childTypesList)
        {
            ItemBehaviour createdItem = Activator.CreateInstance(item) as ItemBehaviour;
            orderableItemsList.Add(createdItem);
        }

        foreach (var post in postList)
        {
            post.InitList(orderableItemsList);
        }
    }




}
