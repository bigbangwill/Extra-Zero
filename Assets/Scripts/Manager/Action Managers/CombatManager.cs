using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CombatManager : MonoBehaviour
{
    //#region Singleton Manager
    //public static CombatManager Instance
    //{
    //    get { return ((CombatManager)_Instance); }
    //    set { _Instance = value; }
    //}
    //#endregion
    private PlayerInput _PlayerInput;


    public UnityEvent shootEvent;

    private void Start()
    {
        // To set the refrence of Player Input Component
        _PlayerInput = GetComponent<PlayerInput>();
    }

    private void OnShoot()
    {
        shootEvent.Invoke();
        Debug.Log("Shooted");
    }

}