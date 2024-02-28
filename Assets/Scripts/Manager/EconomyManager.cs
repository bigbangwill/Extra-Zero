using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{

    private int inGameCurrencyCurrentStack = 5;
    private int inGameCurrencyMaxStack = 10;


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

    private int outGameCurrencyCurrentStack = 5;
    private int outGameCurrencyMaxStack = 10;

    public int OutGameCurrencyCurrentStack
    {
        get => outGameCurrencyCurrentStack;
        set
        {
            outGameCurrencyCurrentStack = value;
            ValuesChanged();
        }
    }

    public int OutGameCurrencyMaxStack
    {
        get => outGameCurrencyMaxStack;
        set
        {
            if (value > outGameCurrencyMaxStack) return;
            outGameCurrencyMaxStack = value;
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
}