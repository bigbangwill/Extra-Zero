using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairHammerScript : MonoBehaviour
{

    public Type relatedInterface;


    private void OnEnable()
    {
        //UseableItemCanvasScript.Instance.SetDelegate<IRepairable>(Repair);
    }

    private void OnDisable()
    {
        UseableItemCanvasScript.Instance.RemoveDelegate();
    }

    public void Repair()
    {
        
    }

}