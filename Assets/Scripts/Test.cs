using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour, IPauseable
{

    private Rigidbody2D rb;



    private bool isPaused = false;

    private MovementManagerRefrence movementManagerRefrece;

    private EventManagerRefrence eventManagerRefrence;

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
        movementManagerRefrece = (MovementManagerRefrence)FindSORefrence<MovementManager>.FindScriptableObject("Movement Manager Refrence");
    }


    private void Start()
    {
        LoadSORefrence();
        // to set the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        //Debug.Log(BinderManager.Instance.CheckBinderIsSet("Safehouse Movement", true));
    }


    private void Update()
    {
        if(isPaused) return;

        Vector2 desiredVelocity = movementManagerRefrece.val.MovementInput();
        rb.velocity = desiredVelocity;
    }


    public void OnEnable()
    {
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