using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameStateSetter : MonoBehaviour
{

    public GameState gameState;

    private GameStateManagerRefrence gameStateManagerRefrence;

    private void LoadSORefrence()
    {
        gameStateManagerRefrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
    }

    private void Start()
    {
        LoadSORefrence();
        Debug.Log("here");
        gameStateManagerRefrence.val.SetGameState(gameState);
    }
}
