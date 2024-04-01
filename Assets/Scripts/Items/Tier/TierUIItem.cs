using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TierUIItem : MonoBehaviour
{
    [SerializeField] private GameObject checkObject;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    private ItemBehaviour holdingItem;
    public void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
        SetState(false);
    }

    public void SetItem(ItemBehaviour item)
    {
        holdingItem = item;
        icon.sprite = item.IconRefrence();
        itemName.text = item.GetName();
        SetState(false);
    }

    public void SetState(bool isDone)
    {
        if (isDone)
            checkObject.SetActive(true);
        else
            checkObject.SetActive(false);
    }
}