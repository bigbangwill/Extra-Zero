using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState { OnMenu,InGame}
public class GameStateManager : SingletonComponent<GameStateManager>
{

    #region Singleton
    public static GameStateManager Instance
    {
        get { return (GameStateManager)_Instance; }
        set { _Instance = value; }
    }
    #endregion

    private GameState currentGameState;


    private event Action ChangeState;

    private static GameStateManager instance;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
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
        return currentGameState;
    }

}
