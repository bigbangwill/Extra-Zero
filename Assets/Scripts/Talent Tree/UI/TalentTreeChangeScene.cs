using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTreeChangeScene : MonoBehaviour
{
    [SerializeField] private TalentRotator rotator;
    [SerializeField] private Camera talentCam;


    private GameStateManager stateManager;
    private TalentRotator talentRotator;

    private void LoadSORefrence()
    {
        stateManager = ((GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence")).val;
        talentRotator = ((TalentRotatorRefrence)FindSORefrence<TalentRotator>.FindScriptableObject("Talent Rotator Refrence")).val;

    }

    private void Start()
    {
        LoadSORefrence();
        Debug.Log(stateManager);
        stateManager.ChangeStateAddListener(OnChangeScene);
    }


    private void OnChangeScene()
    {
        talentRotator.SetCameraAndSelfDisabled();
    }
}
