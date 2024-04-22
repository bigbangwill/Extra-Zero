using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{


    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }



    public void ContinueClicked()
    {
        Time.timeScale = 1.0f;
        GameModeState.IsFinished = true;
        SceneManager.LoadScene("Menu Scene");
    }


}