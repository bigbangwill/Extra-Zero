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

    private List<ItemBehaviour> orderableItems = new();

    private List<WalkingOrder> activeWalkingOrders = new();

    private int currentWave = 1;
    private float waveCurrentTimer = 0;


    private void Start()
    {
        Init();
        //StartWaveTimer();
        activeWalkingOrders.Add(postList[0].CreateWalkingOrder());
        activeWalkingOrders.Add(postList[0].CreateWalkingOrder());
        activeWalkingOrders.Add(postList[0].CreateWalkingOrder());
        activeWalkingOrders.Add(postList[0].CreateWalkingOrder());

    }


    public void FinishedWalkingOrder(WalkingOrder walkingOrder)
    {
        activeWalkingOrders.Remove(walkingOrder);
        if(activeWalkingOrders.Count == 0 )
        {
            Debug.Log("Wave Finished");
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

    private void InitWaveSystem()
    {
        currentWave = 1;
        waveCurrentTimer = 0;
    }




}
