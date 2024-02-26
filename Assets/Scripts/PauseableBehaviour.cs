using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseableBehaviour : MonoBehaviour
{
    protected bool isPaused = false;

    private EventManagerRefrence eventManagerRefrence;

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
    }


    public void OnEnable()
    {
        LoadSORefrence();
        eventManagerRefrence.val._PauseEvent.AddListener(Pause);
        eventManagerRefrence.val._ResumeEvent.AddListener(Resume);
    }

    public void OnDisable()
    {
        if (eventManagerRefrence.val != null)
        {
            eventManagerRefrence.val._PauseEvent.RemoveListener(Pause);
            eventManagerRefrence.val._ResumeEvent.RemoveListener(Resume);
        }
    }

    public void OnDestroy()
    {
        if (eventManagerRefrence.val != null)
        {
            eventManagerRefrence.val._PauseEvent.RemoveListener(Pause);
            eventManagerRefrence.val._ResumeEvent.RemoveListener(Resume);
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