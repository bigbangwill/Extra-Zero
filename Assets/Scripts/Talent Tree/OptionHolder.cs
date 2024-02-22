using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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


    private void Start()
    {
        SetQubitMax(2);
        SetGateMax(0);
        SetEntangleMax(0);
    }



    public void SetQubitMax(int value)
    {
        qubitMaxCount = value;
    }

    public void SetQubitCurrent(int value)
    {
        qubitCurrentCount = value;
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
    }
    
    public void SetGateCurrent(int value)
    {
        gateCurrentCount = value;
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
    }

    public void SetEntangleCurrent(int value)
    {
        entangleCurrentCount = value;
    }

    public bool CanAddEntangle()
    {
        if(entangleCurrentCount < entangleMaxCount)
            return true;
        return false;
    }

    
}