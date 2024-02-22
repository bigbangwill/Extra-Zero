using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class MenuPurchaseScript : MonoBehaviour
{
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject purchasePrefab;

    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private List<PurchasableLibrary> purchasablesList = new();
    private PurchasableScript activePurchasable;



    private void Start()
    {
        Init();
    }


    private void Init()
    {

        List<Type> purchasables = Assembly.GetAssembly(typeof(PurchasableLibrary)).GetTypes().Where
            (TheType => TheType.IsClass && !TheType.IsAbstract &&
            TheType.IsSubclassOf(typeof(PurchasableLibrary))).ToList();
        foreach (var purchasable in purchasables)
        {
            PurchasableLibrary targetPurchasable = (PurchasableLibrary)Activator.CreateInstance(purchasable);
            purchasablesList.Add(targetPurchasable);
        }
        for(int i = 0; i < purchasablesList.Count; i++)
        {
            GameObject purchasableGO = Instantiate(purchasePrefab, scrollViewContent);
            purchasableGO.GetComponent<PurchasableScript>().SetupPurchasables(purchasablesList[i],this);
        }
    }
    

    public void SetPurchasableActive(PurchasableScript purchasableScript)
    {
        if(activePurchasable != null)
        {
            activePurchasable.SetActiveIndicator(false);
        }
        activePurchasable = purchasableScript;
        activePurchasable.SetActiveIndicator(true);
        SetButtonState();
        descriptionText.text = purchasableScript.Purchasable.PurchasableDescription;
    }

    public void PurchaseButtonClicked()
    {
        Debug.Log("Clicked");
        activePurchasable.Purchasable.purchasedMethod();
        EconomyManager.Instance.OutGameCurrencyCurrentStack -= activePurchasable.Purchasable.Cost;
        SetButtonState();
    }
    private void SetButtonState()
    {
        if (activePurchasable.Purchasable.Cost <= EconomyManager.Instance.OutGameCurrencyCurrentStack)
        {
            purchaseButton.interactable = true;
        }
        else
        {
            purchaseButton.interactable = false;
        }
    }
}