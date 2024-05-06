using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CampaignCurrencyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyCurrencyStack;

    private EconomyManagerRefrence economyManagerRefrence;
    private bool isInitialized = false;

    private void LoadSORefrence()
    {
        economyManagerRefrence = (EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence");
    }

    private void OnEnable()
    {
        if (isInitialized)
        {
            LoadSORefrence();
            economyManagerRefrence.val.AddListener(RefreshUI);
        }
    }

    private void Start()
    {
        isInitialized = true;
        OnEnable();
        RefreshUI();
    }

    private void OnDisable()
    {
        if (economyManagerRefrence.val != null)
            economyManagerRefrence.val.RemoveListener(RefreshUI);
    }

    public void RefreshUI()
    {
        string energyCurrenctStack = economyManagerRefrence.val.CampaignEnergyCurrentStack.ToString();
        string energyMaxStack = economyManagerRefrence.val.CampaignEnergyMaxStack.ToString();
        energyCurrencyStack.text = energyCurrenctStack + " / " + energyMaxStack;
    }
}
