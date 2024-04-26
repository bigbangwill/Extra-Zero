using System;
using UnityEngine;

public class OptionHolder : MonoBehaviour, ISaveable
{

    private int qubitMaxCount;
    public int QubitMaxCount {  get { return qubitMaxCount; } }
    private int qubitCurrentCount = 0;
    public int QubitCurrentCount {  get { return qubitCurrentCount; } }

    private int gateMaxCount;
    public int GateMaxCount { get { return gateMaxCount; } }
    private int gateCurrentCount = 0;
    public int GateCurrentCount { get { return gateCurrentCount; } }

    private int entangleMaxCount;
    public int EntangleMaxCount {  get { return entangleMaxCount; } }
    private int entangleCurrentCount = 0;
    public int EntangleCurrentCount {  get { return entangleCurrentCount; } }


    public event Action ValuesChanged;

    private OptionHolderRefrence refrence;
    private SaveClassManager saveClassManager;


    private void SetRefrence()
    {
        refrence = (OptionHolderRefrence)FindSORefrence<OptionHolder>.FindScriptableObject("Option Holder Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void LoadSORefrence()
    {
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        SetQubitMax(0);
        SetGateMax(0);
        SetEntangleMax(0);
        AddISaveableToDictionary();
    }

    public void AddListener(Action action)
    {
        ValuesChanged += action;
    }

    public void RemoveListener(Action action)
    {
        ValuesChanged -= action;
    }



    public void SetQubitMax(int value)
    {
        qubitMaxCount = value;
        ValuesChanged?.Invoke();
    }

    public void SetQubitCurrent(int value)
    {
        qubitCurrentCount = value;
        ValuesChanged?.Invoke();
    }

    public void AddQubitCurrent(int value)
    {
        qubitCurrentCount += value;
        ValuesChanged?.Invoke();
    }

    public void AddQubitMax(int value)
    {
        qubitMaxCount += value;
        ValuesChanged?.Invoke();
    }

    public bool CanAddQubit()
    {
        if (qubitCurrentCount < qubitMaxCount)
            return true;
        return false;
    }

    public void SetGateMax(int value)
    {
        gateMaxCount = value;
        ValuesChanged?.Invoke();
    }
    
    public void SetGateCurrent(int value)
    {
        gateCurrentCount = value;
        ValuesChanged?.Invoke();
    }
    public void AddGateCurrent(int value)
    {
        gateCurrentCount += value;
        ValuesChanged?.Invoke();
    }

    public void AddGateMax(int value)
    {
        gateMaxCount += value;
        ValuesChanged?.Invoke();
    }

    public bool CanAddGate()
    {
        if (gateCurrentCount < gateMaxCount)
            return true;     
        return false;
    }

    public void SetEntangleMax(int value)
    {
        entangleMaxCount = value;
        ValuesChanged?.Invoke();
    }

    public void SetEntangleCurrent(int value)
    {
        entangleCurrentCount = value;
        ValuesChanged?.Invoke();
    }

    public void AddEntangleCurrent(int value)
    {
        entangleCurrentCount += value;
        ValuesChanged?.Invoke();
    }

    public void AddEntangleMax(int value)
    {
        entangleMaxCount += value;
        ValuesChanged?.Invoke();
    }

    public bool CanAddEntangle()
    {
        if(entangleCurrentCount < entangleMaxCount)
            return true;
        return false;
    }

    public void AddISaveableToDictionary()
    {
        saveClassManager.AddISaveableToDictionary(GetName(), this, 3);
    }

    public object Save()
    {
        SaveClassesLibrary.OptionHolderSave saveData = 
            new(qubitMaxCount, gateMaxCount, entangleMaxCount,qubitCurrentCount,gateCurrentCount,entangleCurrentCount);
        return saveData;
    }

    public void Load(object savedData)
    {
        SaveClassesLibrary.OptionHolderSave data = (SaveClassesLibrary.OptionHolderSave)savedData;
        qubitMaxCount = data.qubitMax;
        gateMaxCount = data.gateMax;
        entangleMaxCount = data.entangleMax;
        qubitCurrentCount = data.qubitCurrent;
        gateCurrentCount = data.gateCurrent;
        entangleCurrentCount = data.entangleCurrent;
    }

    public string GetName()
    {
        return "OptionHolder";
    }
}