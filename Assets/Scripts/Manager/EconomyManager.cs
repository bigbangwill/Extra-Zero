using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{

    [SerializeField] private int inGameCurrencyCurrentStack = 5;
    [SerializeField] private int inGameCurrencyMaxStack = 10;
    [SerializeField] private int outGameCurrencyCurrentStack = 5;
    [SerializeField] private int outGameCurrencyMaxStack = 10;
    [SerializeField] private int quantumQuartersCurrentStack = 5;
    [SerializeField] private int quantumQuartersMaxStack = 10;

    private EconomyManagerRefrence refrence;
    private static EconomyManager instance;

    private void LoadSORefrence()
    {

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

    public int InGameCurrencyCurrentStack 
    { 
        get => inGameCurrencyCurrentStack;
        set
        {
            if (value > inGameCurrencyMaxStack) return;
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
            if (value > outGameCurrencyMaxStack) return;
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
            if (value > quantumQuartersMaxStack) return;
            ValuesChanged();
        }
    }

    public int QuantumQuarterssMaxStack
    {
        get => quantumQuartersMaxStack;
        set
        {
            quantumQuartersMaxStack = value;
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
}