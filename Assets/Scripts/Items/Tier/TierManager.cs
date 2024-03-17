using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public class TierManager : MonoBehaviour
{

    private List<CraftedItem> firstTierCraftedItems = new();
    private List<CraftedItem> secondTierCraftedItems = new();
    private List<CraftedItem> thirdTierCraftedItems = new();
    private List<CraftedItem> forthTierCraftedItems = new();


    private void Start()
    {
        InitTierCraftedList();
    }

    private void InitTierCraftedList()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(BluePrintItem))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(BluePrintItem))).ToList();

        foreach (var item in craftedItems)
        {
            BluePrintItem target = (BluePrintItem)Activator.CreateInstance(item);
            foreach (var material in target.materialsList)
            {
                switch (material.GetItemTier())
                {
                    case 1: firstTierCraftedItems.Add(target.CraftedItemReference()); break;
                    case 2: secondTierCraftedItems.Add(target.CraftedItemReference()); break;
                    case 3: thirdTierCraftedItems.Add(target.CraftedItemReference()); break;
                    case 4: forthTierCraftedItems.Add(target.CraftedItemReference()); break;
                    default: Debug.LogWarning("CHECK HERE ASAP"); break;
                }
            }
        }


    }


}
