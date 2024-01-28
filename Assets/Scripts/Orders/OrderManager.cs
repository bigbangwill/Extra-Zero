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


    private int currentWave = 1;

    [SerializeField] private float waveMaxTimer;
    private float waveCurrentTimer = 0;

    private bool isWaitingForNextWave = false;

    private void Start()
    {
        Init();
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


    public void PostOrderCompleted()
    {
        if (IsWaveComplete())
        {

        }
    }




    public void StartNextWave()
    {
        isWaitingForNextWave = false;
        currentWave++;

        foreach(OrderPost post in postList)
        {
            post.CreateOrderQue(1,1,1);
        }
    }


    public void CheckIfAllIsDone()
    {
        foreach (OrderPost post in postList)
        {
            if (!post.OrdersAreComplete())
                return;
        }
        StartWaveTimer();
    }


    private void StartWaveTimer()
    {
        isWaitingForNextWave = true;
    }

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
