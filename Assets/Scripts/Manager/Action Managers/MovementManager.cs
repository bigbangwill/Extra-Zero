using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MovementManager : SingletonComponent<MovementManager>
{
    #region Singleton Manager
    public static MovementManager Instance
    {
        get { return ((MovementManager)_Instance); }
        set { _Instance = value; }
    }
    #endregion

    private PlayerInput _PlayerInput;
    private InputAction move;

    

    private void Start()
    {
        // To set the refrence of Player Input Component
        _PlayerInput = GetComponent<PlayerInput>();
    }

    // To get called from other scripts to read the movement input value
    public Vector2 MovementInput()
    {
        move = _PlayerInput.actions["Move"];
        Vector2 value = move.ReadValue<Vector2>();
        return value;
    }


}