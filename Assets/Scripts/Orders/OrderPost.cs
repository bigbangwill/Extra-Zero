using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;

public class OrderPost : MonoBehaviour
{

    private Order currentOrder;



    private void Start()
    {
        List<Type> childTypesList = Assembly.GetAssembly(typeof(ItemBehaviour))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType != typeof(PotionItem) && TheType != typeof(BluePrintItem) &&  TheType.IsSubclassOf(typeof(ItemBehaviour))).ToList();

        foreach (var item in childTypesList)
        {
            ItemBehaviour items = Activator.CreateInstance(item) as ItemBehaviour;
            Debug.Log(items.GetName());

        }
    }



    public void CreateOrderQue(float speed, int count, int combinationCount)
    {
        




    }




    public void InsertingItem(ItemBehaviour item,int slotNumber)
    {
        if (currentOrder.ItemIsEqual(item,slotNumber))
        {
            Debug.Log("Matched");
        }
        else
        {
            Debug.Log("Doesnt match");
        }
    }
    
    public void CurrentOrderFullfilled()
    {
        Debug.Log("Fullfilled");
    }
}