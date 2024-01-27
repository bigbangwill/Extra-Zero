using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPost : MonoBehaviour
{

    private Order currentOrder;



    private void Start()
    {
        List<ItemBehaviour> orderItem = new()
        {
            new MaterialItem.Plastic(3)
        };
        currentOrder = new(orderItem, this);
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