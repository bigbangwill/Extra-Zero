using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSystemManager : MonoBehaviour
{

    private BuffSystemManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (BuffSystemManagerRefrence)FindSORefrence<BuffSystemManager>.FindScriptableObject("Buff System Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
        }
        refrence.val = this;
    }


    private void Awake()
    {
        SetRefrence();
    }




    public void AddBuffEffect(Action startMethod, Action endMethod, float duration)
    {
        startMethod();
        StartCoroutine(StartEffect(endMethod, duration));
    }



    private IEnumerator StartEffect(Action endMethod,float duration)
    {
        yield return new WaitForSeconds(duration);
        endMethod();
    }



}
