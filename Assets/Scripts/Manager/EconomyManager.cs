using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : SingletonComponent<EconomyManager>
{
    #region Singleton
    public static EconomyManager Instance
    {
        get { return (EconomyManager)_Instance; }
        set { _Instance = value; }
    }
    #endregion

    private int inGameCurrencyCurrentStack = 5;
    private int inGameCurrencyMaxStack = 10;

    private static EconomyManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
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