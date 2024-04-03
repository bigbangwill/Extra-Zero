using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeSceneButton()
    {
        StartCoroutine(LoadNextScene());

        //SceneManager.LoadScene("Scene one", LoadSceneMode.Single);
    }
    public void BackToMenuScene()
    {
        SceneManager.LoadScene("Menu Scene", LoadSceneMode.Single);
    }

    private IEnumerator LoadNextScene()
    {
        UnityEngine.AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene one");
        asyncLoad.allowSceneActivation = true;
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }
    }


}