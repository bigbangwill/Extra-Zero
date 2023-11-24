using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour, IPauseable
{

    private Rigidbody2D rb;



    private bool isPaused = false;


    private void Start()
    {
        // to set the rigidbody component
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(BinderManager.Instance.CheckBinderIsSet("Safehouse Movement", true));
    }


    private void Update()
    {
        if(isPaused) return;

        Vector2 desiredVelocity = MovementManager.Instance.MovementInput();
        rb.velocity = desiredVelocity;
    }


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