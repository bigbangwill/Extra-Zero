using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    //#region Singleton
    //public static EconomyManager Instance
    //{
    //    get { return (EconomyManager)_Instance; }
    //    set { _Instance = value; }
    //}
    //#endregion

    private int inGameCurrencyCurrentStack = 5;
    private int inGameCurrencyMaxStack = 10;


    private EconomyManagerRefrence refrence;

    private void LoadSORefrence()
    {

    }

    private void SetRefrence()
    {
        refrence = (EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrecne");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        Debug.Log("We did find it");
        refrence.val = this;
    }

    private void Awake()
    {
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

    private event Action ValuesChangedEvent;


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