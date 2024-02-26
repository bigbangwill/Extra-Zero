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

    private UsableCanvasManagerRefrence usableRefrence;

    private void LoadSORefrence()
    {
        usableRefrence = (UsableCanvasManagerRefrence)FindSORefrence<UseableItemCanvasScript>.FindScriptableObject("Usable Manager Refrence");
    }


    private void OnEnable()
    {
        LoadSORefrence();
        usableRefrence.val.SetDelegate(RepairHammerUsed,OverlayState.RepairMode,transform,iconParent,repairButton);
    }

    private void OnDisable()
    {
        usableRefrence.val.RemoveDelegate(RepairHammerUsed);
    }

    public void RepairHammerUsed()
    {
        Debug.Log("used the hammer");
    }

}