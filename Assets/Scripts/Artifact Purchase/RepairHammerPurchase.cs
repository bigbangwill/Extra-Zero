using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairHammerPurchase : MonoBehaviour
{

    [SerializeField] private Button purchaseButton;
    [SerializeField] private int purhcaseCost;


    private bool isPurchased = false;


    private EconomyManager economyManager;
    private PlayerInventory playerInventory;
    private EventTextManager eventTextManager;

    private void LoadSORefrence()
    {
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
        playerInventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
    }


    private void OnEnable()
    {
        LoadSORefrence();
        if (economyManager.QuantumQuartersCurrentStack >= purhcaseCost && !isPurchased)
            purchaseButton.interactable = true;
        else
            purchaseButton.interactable = false;
    }



    public void PurchaseButtonClicked()
    {
        if (playerInventory.HaveEmptySlot(new CraftedItem.RepairHammer(), true))
        {
            isPurchased = true;
            economyManager.QuantumQuartersCurrentStack -= purhcaseCost;
            eventTextManager.CreateNewText("Purchased!", TextType.Information);
            gameObject.SetActive(false);
        }
        else
        {
            eventTextManager.CreateNewText("Inventory is full!", TextType.Error);
        }

    }





}
