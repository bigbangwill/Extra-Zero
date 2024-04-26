using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour, ISaveable
{

    [SerializeField] private int inGameCurrencyCurrentStack = 5;
    [SerializeField] private int inGameCurrencyMaxStack = 10;
    [SerializeField] private int outGameCurrencyCurrentStack = 5;
    [SerializeField] private int outGameCurrencyMaxStack = 10;
    [SerializeField] private int quantumQuartersCurrentStack = 1;
    [SerializeField] private int quantumQuartersMaxStack = 10;
    [SerializeField] private int campaignEnergyCurrentStack = 0;
    [SerializeField] private int campaignEnergyMaxStack = 10;
    private EconomyManagerRefrence refrence;
    private static EconomyManager instance;


    private SaveClassManager saveClassManager;

    private void LoadSORefrence()
    {
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
    }

    private void SetRefrence()
    {
        refrence = (EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        AddISaveableToDictionary();
    }


    public int InGameCurrencyCurrentStack 
    { 
        get => inGameCurrencyCurrentStack;
        set
        {
            if (value > inGameCurrencyMaxStack)
            {
                InGameCurrencyCurrentStack = inGameCurrencyMaxStack;
                ValuesChanged();
                return;
            }
            inGameCurrencyCurrentStack = value;
            ValuesChanged();
        }
    }

    public int InGameCurrencyMaxStack
    {
        get => inGameCurrencyMaxStack;
        set
        {
            inGameCurrencyMaxStack = value;
            ValuesChanged();
        }
    }

    

    public int OutGameCurrencyCurrentStack
    {
        get => outGameCurrencyCurrentStack;
        set
        {
            if (value > outGameCurrencyMaxStack)
            {
                OutGameCurrencyCurrentStack = outGameCurrencyMaxStack;
                ValuesChanged();
                return;
            }
            outGameCurrencyCurrentStack = value;
            ValuesChanged();
        }
    }

    public int OutGameCurrencyMaxStack
    {
        get => outGameCurrencyMaxStack;
        set
        {
            outGameCurrencyMaxStack = value;
            ValuesChanged();
        }
    }

    

    public int QuantumQuartersCurrentStack
    {
        get => quantumQuartersCurrentStack;
        set
        {
            if (value > quantumQuartersMaxStack)
            {
                quantumQuartersCurrentStack = quantumQuartersMaxStack;
                ValuesChanged();
                return;
            }

            quantumQuartersCurrentStack = value;
            ValuesChanged();
        }
    }

    public int QuantumQuarterssMaxStack
    {
        get => quantumQuartersMaxStack;
        set
        {
            quantumQuartersMaxStack = value;
            ValuesChanged();
        }
    }



    public int CampaignEnergyCurrentStack
    {
        get => campaignEnergyCurrentStack;
        set
        {
            if (value > campaignEnergyMaxStack)
            {
                campaignEnergyCurrentStack = campaignEnergyMaxStack;
                ValuesChanged();
                return;
            }
            campaignEnergyCurrentStack = value;
            ValuesChanged();
        }
    }

    public int CampaignEnergyMaxStack
    {
        get => campaignEnergyMaxStack;
        set
        {
            campaignEnergyMaxStack = value;
            ValuesChanged();
        }
    }


    private static event Action ValuesChangedEvent;


    public void AddListener(Action listener)
    {
        ValuesChangedEvent += listener;
    }

    public void RemoveListener(Action listener)
    {
        ValuesChangedEvent -= listener;
    }

    private void ValuesChanged()
    {
        ValuesChangedEvent?.Invoke();
    }

    public void AddISaveableToDictionary()
    {
        saveClassManager.AddISaveableToDictionary(GetName(), this, 2);
    }

    public object Save()
    {
        SaveClassesLibrary.EconomyManagerSave economyManagerSave = new(this);
        return economyManagerSave;
    }

    public void Load(object savedData)
    {
        SaveClassesLibrary.EconomyManagerSave data = (SaveClassesLibrary.EconomyManagerSave)savedData;
        OutGameCurrencyCurrentStack = data.outGameCurrent;
        OutGameCurrencyMaxStack = data.outGameMax;
        CampaignEnergyCurrentStack = data.campaignEnergyCurrent;
        CampaignEnergyMaxStack = data.campaignEnergyMax;
    }

    public string GetName()
    {
        return "EconomyManager";
    }
}