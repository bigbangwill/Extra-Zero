using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BasicMaterialScript : MonoBehaviour, IPointerUpHandler,IPointerDownHandler,IPointerClickHandler
{

    [SerializeField] private TextMeshProUGUI currentTreshText;

    private Image farmIcon;

    private MaterialItem setItem;
    private PlayerInventory playerInventory;
    private int maxThreshold;
    private int currentThreshold;

    public void Init(MaterialItem item,PlayerInventory inventory)
    {
        farmIcon = GetComponent<Image>();
        setItem = item;
        playerInventory = inventory;
        maxThreshold = item.GetMaxThreshold();
        currentThreshold = 0;
        farmIcon.sprite = setItem.GetFarmIcon();
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    private void Pickaxed()
    {
        currentThreshold++;
        if (currentThreshold >= maxThreshold)
        {
            GiveReward();
            currentThreshold = 0;
        }
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
    }

    // should set it to add to stash and then the player can pick them up.
    private void GiveReward()
    {
        if (playerInventory.HaveEmptySlot(setItem, true))
        {
            Debug.Log("Gave the reward");
        }
        else
        {
            Debug.Log("Is full");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Here");
        Pickaxed();
    }
}