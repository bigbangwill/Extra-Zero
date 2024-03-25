using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using Unity.VisualScripting;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopItemPrefab;

    [SerializeField] private Transform blueprintParentTransform;
    [SerializeField] private BluePrintInfo bluePrintInfo;
    [SerializeField] private GameObject infoParent;

    private ItemBehaviour holdingItem;

    private List<BluePrintItem> bluePrintList = new();

    private void Start()
    {
        Init();
        CreateBlueprintShoppingList();
    }

    private void Init()
    {
        List<Type> types = Assembly.GetAssembly(typeof(BluePrintItem)).GetTypes().ToList().
            Where(Thetype => Thetype.IsClass &&
            !Thetype.IsAbstract && Thetype.IsSubclassOf(typeof(BluePrintItem))).ToList();
        foreach (var item in types)
        {
            BluePrintItem target = (BluePrintItem)Activator.CreateInstance(item);
            bluePrintList.Add(target);
        }

    }


    public void CreateBlueprintShoppingList()
    {
        foreach (Transform child in  blueprintParentTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in bluePrintList)
        {
            GameObject target = Instantiate(shopItemPrefab,blueprintParentTransform);
            target.GetComponent<ShopItemScript>().SetItem(item.PurchaseCost, item, this);
        }
    }
    
    public void ItemClicked(ItemBehaviour clickedItem)
    {
        if (clickedItem.ItemTypeValue() == ItemType.bluePrint)
        {
            infoParent.SetActive(true);
            holdingItem = clickedItem;
            bluePrintInfo.gameObject.SetActive(true);
            bluePrintInfo.SetItem((BluePrintItem)clickedItem);
        }
    }

    public void Purchase()
    {
        Debug.LogWarning("Purchased!");
    }
    
}