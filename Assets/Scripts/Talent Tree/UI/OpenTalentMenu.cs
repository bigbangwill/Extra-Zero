using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenTalentMenu : MonoBehaviour
{

    private TalentRotator rotator;


    private void LoadSORefrence()
    {
        rotator = ((TalentRotatorRefrence)FindSORefrence<TalentRotator>.FindScriptableObject("Talent Rotator Refrence")).val;
    }


    private void Start()
    {
        LoadSORefrence();
    }


    public void TurnOnTalent()
    {
        if(rotator != null)
        {
            rotator.SetCameraAndSelfEnabled();
        }
        else
        {
            LoadSORefrence();
            rotator.SetCameraAndSelfEnabled();
        }
    }





}
