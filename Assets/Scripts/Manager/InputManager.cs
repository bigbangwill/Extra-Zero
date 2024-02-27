using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.Rendering;
using System.Linq;

public class InputManager : MonoBehaviour
{
    //#region Singleton Manager
    //public static InputManager Instance
    //{
    //    get { return ((InputManager)_Instance); }
    //    set { _Instance = value; }
    //}
    //#endregion

    [SerializeField] private PlayerInput _PlayerInput;
    private InputAction move;

    private void Start()
    {
        foreach (var action in _PlayerInput.actions)
        {
            Debug.Log(action.name);
        }
    }

    public Vector2 MovementInput()
    {
        move = _PlayerInput.actions["Move"];
        Vector2 value = move.ReadValue<Vector2>();
        return value;
    }
}