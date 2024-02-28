using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelStationTalent : LevelStationsInteract, IReacheable
{

    private TalentRotator talentRotator;
    [SerializeField] private GameObject navmeshMovement;

    private void LoadSORefrence()
    {
        basementManagerRefrence = (BasementManagerRefrence)FindSORefrence<BasementManager>.FindScriptableObject("Basement Manager Refrence");
        talentRotator = ((TalentRotatorRefrence)FindSORefrence<TalentRotator>.FindScriptableObject("Talent Rotator Refrence")).val;
    }

    void Start()
    {
        LoadSORefrence();
    }

    public override void Interact()
    {
        talentRotator.SetCameraAndSelfEnabled();
        navmeshMovement.SetActive(false);
    }

    public override NavmeshReachableInformation ReachAction()
    {
        NavmeshReachableInformation value = new(reachingTransfrom.position, Interact);
        return value;
    }


}
