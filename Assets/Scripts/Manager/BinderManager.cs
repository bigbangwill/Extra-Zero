using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem.Samples.RebindUI;

public class BinderManager : SingletonComponent<BinderManager>
{
    //#region Sinleton
    //public static BinderManager Instance
    //{
    //    get { return ((BinderManager)_Instance); }
    //    set { _Instance = value; }
    //}
    //#endregion
    // For fast finding the required prefab for rebinding
    private Dictionary<string, GameObject> prefabFinder = new();

    // For checking if the bind is ready to use or not
    private Dictionary<string, bool> rebindCheckDic = new();


    // Prefabs for rebind system
    [Header("Prefab for binding")]
    [SerializeField] private GameObject movementRebindPrefab;
    [SerializeField] private GameObject interactRebindPrefab;


    //Variables need to instantiate the rebnindPrefab at a good position
    [Header("Rebind Prefab infos")]
    [SerializeField] private Transform rebindParentGO;
    [SerializeField] private GameObject blockInputOverlay;
    [SerializeField] private TextMeshProUGUI overlayText;
    [SerializeField] private GameObject BinderGameobject; // to setactive when the bind should happen.

    private EventManagerRefrence eventManagerRefrence;

    private void LoadSORefrence()
    {
        eventManagerRefrence = (EventManagerRefrence)FindSORefrence<EventManager>.FindScriptableObject("Event Manager Refrence");
    }

    private void Awake()
    {
        InitDictionary();
        InitRebindCheckDic();
    }

    private void Start()
    {
        LoadSORefrence();
    }

    // To set all of the strings to the related prefab with the use of dictionary
    private void InitDictionary()
    {
        prefabFinder.Add("Safehouse Movement", movementRebindPrefab);
        prefabFinder.Add("Safehouse Interact", interactRebindPrefab);
    }

    // To set the default value for rebind Check Dic
    private void InitRebindCheckDic()
    {
        foreach(var key in  prefabFinder)
        {
            rebindCheckDic.Add(key.Key, false);
        }
    }

    // Gets called from outside that will check if the related rebind has be set
    // before or not. and has a bool to see if it should be set right now or not.
    public bool CheckBinderIsSet(string rebindString, bool shouldSet)
    {
        if (!shouldSet)
            return rebindCheckDic[rebindString];
        else
        {
            if (!rebindCheckDic[rebindString])
            {
                rebindCheckDic[rebindString] = true;
                RebindSetter(rebindString);
                return rebindCheckDic[rebindString];
            }
            else
                return rebindCheckDic[rebindString];
        }
    }


    // Method to get called when a certain cirmustance gets meet to 
    // call the binder canvas
    private void RebindSetter(string rebindString)
    {
        eventManagerRefrence.val.Pause();
        BinderGameobject.SetActive(true);
        GameObject rebindPrefab = prefabFinder[rebindString];
        GameObject rebinder = Instantiate(rebindPrefab);
        rebinder.transform.SetParent(rebindParentGO);
        rebinder.transform.localPosition = Vector3.zero;
        RebindActionUI rebindActionUI = rebinder.GetComponent<RebindActionUI>();
        rebindActionUI.rebindOverlay = blockInputOverlay;
        rebindActionUI.rebindPrompt = overlayText;
    }



}