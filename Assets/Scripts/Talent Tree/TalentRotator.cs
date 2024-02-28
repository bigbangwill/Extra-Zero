using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TalentRotator : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    [SerializeField] private LayerMask targetMask;

    [SerializeField] private Transform TalentTree;
    [SerializeField] private Vector2 slowdown;
    [SerializeField] private float fastdown;


    public Camera talentCamera;

    private TalentRotatorRefrence refrence;
    private TalentManagerRefrence talentManagerRefrence;
    private GameStateManagerRefrence gameStateManagerRefrence;


    private static TalentRotator instance;

    private void SetRefrence()
    {
        refrence = (TalentRotatorRefrence)FindSORefrence<TalentRotator>.FindScriptableObject("Talent Rotator Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }


    private void LoadSORefrence()
    {
        talentManagerRefrence = (TalentManagerRefrence)FindSORefrence<TalentManager>.FindScriptableObject("Talent Manager Refrence");
        gameStateManagerRefrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.parent.gameObject);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        gameStateManagerRefrence.val.ChangeStateAddListener(SetCameraRefrence);
    }

    public void SetCameraRefrence()
    {
        talentCamera = GameObject.FindGameObjectWithTag("Talent Camera").GetComponent<Camera>();
    }

    public void SetCameraAndSelfDisabled()
    {
        talentCamera.gameObject.SetActive(false);
        enabled = false;
        Debug.Log("Got Disableed");
    }


    public void OnDrag(PointerEventData eventData)
    {
        //TalentTree.Rotate(new Vector3(eventData.delta.y, eventData.delta.x , 0) * slowdown,fastdown,Space.World);
        float rotationX = eventData.delta.y * slowdown.x;
        float rotationY = -eventData.delta.x * slowdown.y;

        TalentTree.rotation = Quaternion.identity;
        // Rotate the camera around the talent tree's position
        talentCamera.transform.RotateAround(TalentTree.position, Vector3.up, rotationY);
        talentCamera.transform.RotateAround(TalentTree.position, talentCamera.transform.right, rotationX);
        TalentTree.rotation = Quaternion.identity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Start");
        Ray ray = talentCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
        {
            Debug.Log("inside if");
            talentManagerRefrence.val.SetTargetNode(hit.collider.GetComponentInParent<NodePassive>());
        }
    }
}
