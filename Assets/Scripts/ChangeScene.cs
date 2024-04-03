using System.Collections;
using System.Collections.Generic;
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
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene one");
        while (!asyncLoad.isDone)
        {
            Debug.Log("Loading progress: " + (asyncLoad.progress * 100) + "%");
            yield return null;
        }
    }


}