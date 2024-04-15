using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameEconomyCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quantumQuartersCurrencyStack;
    [SerializeField] private TextMeshProUGUI inGameCurrencyStack;

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
        string outGameCurrentStack = economyManagerRefrence.val.QuantumQuartersCurrentStack.ToString();
        string outGameMaxStack = economyManagerRefrence.val.QuantumQuarterssMaxStack.ToString();
        quantumQuartersCurrencyStack.text = outGameCurrentStack + " / " + outGameMaxStack;

        string inGameCurrentStack = economyManagerRefrence.val.InGameCurrencyCurrentStack.ToString();
        string inGameMaxStack = economyManagerRefrence.val.InGameCurrencyMaxStack.ToString();
        inGameCurrencyStack.text = inGameCurrentStack + " / " + inGameMaxStack;
    }
}
