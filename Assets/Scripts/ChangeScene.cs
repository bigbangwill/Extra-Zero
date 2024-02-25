using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneButton()
    {

        SceneManager.LoadScene("Scene one", LoadSceneMode.Single);
    }
    public void BackToMenuScene()
    {
        SceneManager.LoadScene("Menu Scene", LoadSceneMode.Single);
    }


}