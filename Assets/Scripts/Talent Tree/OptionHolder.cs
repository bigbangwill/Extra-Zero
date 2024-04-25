using System;
using UnityEngine;

public class OptionHolder : MonoBehaviour
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

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        SetQubitMax(0);
        SetGateMax(0);
        SetEntangleMax(0);
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
}