using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGameStateSetter : MonoBehaviour
{

    public GameState gameState;
    private void Start()
    {
        GameStateManager.Instance.SetGameState(gameState);
    }
}
