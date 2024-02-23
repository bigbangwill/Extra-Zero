using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class OptionHolder : SingletonComponent<OptionHolder>
{
    #region Singleton
    public static OptionHolder Instance
    {
        get { return (OptionHolder) _Instance; }
        set { _Instance = value; }
    }
    #endregion

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



    private void Start()
    {
        SetQubitMax(2);
        SetGateMax(2);
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
    public void AddOneQubitMax()
    {
        qubitMaxCount++;
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

    public void AddOneGateMax()
    {
        gateMaxCount++;
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

    public void AddOneEntangleMax()
    {
        entangleMaxCount++;
        ValuesChanged?.Invoke();
    }

    public bool CanAddEntangle()
    {
        if(entangleCurrentCount < entangleMaxCount)
            return true;
        return false;
    }

    
}