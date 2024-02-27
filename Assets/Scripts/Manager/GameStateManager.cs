using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { OnMenu,InGame}
public class GameStateManager : MonoBehaviour
{

    //#region Singleton
    //public static GameStateManager Instance
    //{
    //    get { return (GameStateManager)_Instance; }
    //    set { _Instance = value; }
    //}
    //#endregion

    private GameState currentGameState;


    private event Action ChangeState;

    //private static GameStateManager instance;


    private GameStateManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        Debug.Log("We did find it");
        refrence.val = this;
    }


    private void Awake()
    {
        SetRefrence();
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}
        //else if (instance != this)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}
    }


    public void ChangeStateAddListener(Action listener)
    {
        ChangeState += listener;
    }

    public void ChangeStateRemoveListener(Action listener)
    {
        ChangeState -= listener;
    }

    private void ChangeStateInvoke()
    {
        ChangeState?.Invoke();
    }


    public void SetGameState(GameState state)
    {
        currentGameState = state;
        ChangeStateInvoke();
    }

    public GameState GetGameState()
    {
        Debug.Log("We Returned it" + currentGameState);
        return currentGameState;
    }

}
