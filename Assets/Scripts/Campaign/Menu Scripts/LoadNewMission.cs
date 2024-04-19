using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNewMission : MonoBehaviour
{

    [SerializeField] ProgressionScript progressionScript;

    public void LoadFirstLevel()
    {
        progressionScript.SetGameModeState();
        SceneManager.LoadScene("Scene One");
    }

    public void LoadSecondLevel()
    {

    }
    
    
}