using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelStationTalent : LevelStationsInteract
{


    private TalentRotator talentRotator;

    private void LoadSORefrence()
    {
        basementManagerRefrence = (BasementManagerRefrence)FindSORefrence<BasementManager>.FindScriptableObject("Basement Manager Refrence");
        talentRotator = ((TalentRotatorRefrence)FindSORefrence<TalentRotator>.FindScriptableObject("Basement Manager Refrence")).val;
    }

    void Start()
    {
        LoadSORefrence();
    }

    //public override void Interact()
    //{
    //    Debug.Log("Here");
    //    if (lastActive != null)
    //    {
    //        lastActive.SetActive(true);
    //        return;
    //    }
    //    canvas.SetActive(true);
    //}

    

}
