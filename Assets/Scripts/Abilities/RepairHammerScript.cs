using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RepairHammerScript : MonoBehaviour
{

    public Type relatedInterface;
    public Transform iconParent;


    private void OnEnable()
    {
        UseableItemCanvasScript.Instance.SetDelegate(RepairHammerUsed,OverlayState.RepairMode,transform,iconParent);
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