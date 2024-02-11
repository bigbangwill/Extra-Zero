using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Contracts;

public class RepairHammerScript : MonoBehaviour
{

    public Type relatedInterface;
    public Transform iconParent;
    public Button repairButton;


    private void OnEnable()
    {
        UseableItemCanvasScript.Instance.SetDelegate(RepairHammerUsed,OverlayState.RepairMode,transform,iconParent,repairButton);
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