using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairHammerScript : MonoBehaviour
{

    public Type relatedInterface;


    private void OnEnable()
    {
        UseableItemCanvasScript.Instance.SetDelegate(RepairHammerUsed,OverlayState.RepairMode);
    }

    private void OnDisable()
    {
        UseableItemCanvasScript.Instance.RemoveDelegate(RepairHammerUsed);
    }

    public void RepairHammerUsed()
    {
        Debug.Log("used the hammer");
    }

}