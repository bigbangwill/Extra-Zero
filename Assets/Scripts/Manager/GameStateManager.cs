using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { OnMenu,InGame}
public class GameStateManager : MonoBehaviour
{

    private GameState currentGameState;


    private static event Action ChangeState;


    private GameStateManagerRefrence refrence;
    private static GameStateManager instance = null;


    private void SetRefrence()
    {
        refrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SetRefrence();
    }


    public void ChangeStateAddListener(Action listener)
    {
        ChangeState += listener;
        listener();
    }

    public void ChangeStateRemoveListener(Action listener)
    {
        ChangeState -= listener;
    }

    private void ChangeStateInvoke()
    {
        if (ChangeState != null)
        {            
            ChangeState.Invoke();
        }
        else
            Debug.LogWarning("IS EMPTY");
    }


    public void SetGameState(GameState state)
    {
        currentGameState = state;
        ChangeStateInvoke();
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

}
