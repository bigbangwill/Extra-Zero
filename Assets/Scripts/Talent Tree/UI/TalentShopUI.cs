using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class TalentShopUI : MonoBehaviour
{


    [SerializeField] private Button qubitPurchase;
    [SerializeField] private Button gatePurchase;
    [SerializeField] private Button entanglePurchase;

    [SerializeField] private int qubitPrice;
    [SerializeField] private int gatePrice;
    [SerializeField] private int entanglePrice;

    [SerializeField] private OptionHolder optionHolder;


    private EconomyManagerRefrence economyManagerRefrence;


    private void LoadSoRefrence()
    {
        economyManagerRefrence = (EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence");
    }


    private void OnEnable()
    {
        LoadSoRefrence();
        RefreshUIButtons();
    }

    private void RefreshUIButtons()
    {
        if (economyManagerRefrence.val.OutGameCurrencyCurrentStack >= qubitPrice)
        {
            qubitPurchase.interactable = true;
        }
        if (economyManagerRefrence.val.OutGameCurrencyCurrentStack >= gatePrice)
        {
            gatePurchase.interactable = true;
        }
        if (economyManagerRefrence.val.OutGameCurrencyCurrentStack >= entanglePrice)
        {
            entanglePurchase.interactable = true;
        }
    }

    public void PurchaseQubit()
    {
        economyManagerRefrence.val.OutGameCurrencyCurrentStack -= qubitPrice;
        optionHolder.AddQubitMax(1);
        RefreshUIButtons();
    }

    public void PurchaseGate()
    {
        economyManagerRefrence.val.OutGameCurrencyCurrentStack -= gatePrice;
        optionHolder.AddGateMax(1);
        RefreshUIButtons();
    }

    public void PurchaseEntangle()
    {
        economyManagerRefrence.val.OutGameCurrencyCurrentStack -= entanglePrice;
        optionHolder.AddEntangleMax(1);
        RefreshUIButtons();
    }



}
