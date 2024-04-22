using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeTabletPurchase : MonoBehaviour
{


    [SerializeField] private Button purchaseButton;
    [SerializeField] private int purhcaseCost;
    [SerializeField] private GameObject purchasePanel;
    [SerializeField] private GameObject informationPanel;


    private bool isPurchased = false;


    private EconomyManager economyManager;
    private EventTextManager eventTextManager;

    private void LoadSORefrence()
    {
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
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
        isPurchased = true;
        economyManager.QuantumQuartersCurrentStack -= purhcaseCost;
        eventTextManager.CreateNewText("Purchased!", TextType.Information);
        purchasePanel.SetActive(false);
        informationPanel.SetActive(true);
    }





}
