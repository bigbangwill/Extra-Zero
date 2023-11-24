using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseableBehaviour : MonoBehaviour
{
    protected bool isPaused = false;


    public void OnEnable()
    {
        EventManager.Instance._PauseEvent.AddListener(Pause);
        EventManager.Instance._ResumeEvent.AddListener(Resume);
    }

    public void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance._PauseEvent.RemoveListener(Pause);
            EventManager.Instance._ResumeEvent.RemoveListener(Resume);
        }
    }

    public void OnDestroy()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance._PauseEvent.RemoveListener(Pause);
            EventManager.Instance._ResumeEvent.RemoveListener(Resume);
        }
    }

    public void Pause()
    {
        isPaused = true;
    }

    public void Resume()
    {
        isPaused = false;
    }


}