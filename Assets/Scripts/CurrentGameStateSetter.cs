using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CurrentGameStateSetter : MonoBehaviour
{

    public GameState gameState;

    private GameStateManagerRefrence gameStateManagerRefrence;

    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject lostMenuCanvas;
    [SerializeField] private Button watchAdButton;

    private bool watchedAd = false;



    private void LoadSORefrence()
    {
        gameStateManagerRefrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
    }

    private void Start()
    {
        LoadSORefrence();
        gameStateManagerRefrence.val.SetGameState(gameState);
    }


    public void PauseGame()
    {
        pauseMenuCanvas.SetActive(true);
        gameStateManagerRefrence.val.PauseGame();
    }

    public void ResumeGame()
    {
        gameStateManagerRefrence.val.ResumeGame();
    }

    public void GameIsLost()
    {
        lostMenuCanvas.SetActive(true);
        if (!watchedAd)
        {
            watchedAd = true;
            watchAdButton.interactable = true;
        }
        else
        {
            watchAdButton.interactable = false;
        }
        gameStateManagerRefrence.val.PauseGame();
    }

    public void BackToStartMenu()
    {
        gameStateManagerRefrence.val.ResumeGame();
        SceneManager.LoadScene("Menu Scene");
    }

    public void GotBackInTheGame()
    {
        // ADD THE REPAIR THE POSTS BACK UP HERE.


        gameStateManagerRefrence.val.ResumeGame();
    }
}
