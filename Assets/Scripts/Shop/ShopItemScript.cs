using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCost;

    private ItemBehaviour holdingItem;
    private ShopManager shopScript;


    public void SetItem(int cost,ItemBehaviour target,ShopManager shop)
    {
        icon.sprite = target.IconRefrence();
        this.itemName.text = target.GetName();
        itemCost.text = cost.ToString();
        holdingItem = target;
        shopScript = shop;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        shopScript.ItemClicked(holdingItem);
    }
    
}