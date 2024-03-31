using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
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
    [SerializeField] GameState gameState;

    [SerializeField] private Camera talentCamera;
    [SerializeField] private GameObject closeButton;

    private TalentRotatorRefrence refrence;
    private TalentManagerRefrence talentManagerRefrence;
    private GameStateManagerRefrence gameStateManagerRefrence;
    private OrderManagerRefrence orderManagerRefrence;

    private bool isUnlocked = false;
    [SerializeField] private GameObject unlockPanelGO;

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
        orderManagerRefrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        gameStateManagerRefrence.val.ChangeStateAddListener(OnChangeScene);
    }

    private void OnDisable()
    {
        if (gameStateManagerRefrence != null)
        {
            gameStateManagerRefrence.val.ChangeStateRemoveListener(OnChangeScene);
        }
    }

    private void OnChangeScene()
    {
        //SetCameraRefrence();
        if (gameStateManagerRefrence.val.GetGameState() == GameState.InGame)
        {
            isUnlocked = false;
            unlockPanelGO.SetActive(true);
        }
    }

    public void SetCameraRefrence()
    {
        talentCamera = GameObject.FindGameObjectWithTag("Talent Camera").GetComponent<Camera>();
        Debug.Log("Got CHanged");
    }

    public void SetCameraAndSelfDisabled()
    {
        //if (talentCamera == null)
        //    SetCameraRefrence();

        talentCamera.gameObject.SetActive(false);
        closeButton.SetActive(false);
        enabled = false;
        if (gameStateManagerRefrence.val.GetGameState() == GameState.InGame)
        {
            FindSORefrence<RaycastMovement>.FindScriptableObject
                ("Raycast Movement Refrence").val.transform.parent.gameObject.SetActive(true);
        }
    }

    

    public void SetCameraAndSelfEnabled()
    {
        if (!isUnlocked && gameState != GameState.OnMenu)
            unlockPanelGO.SetActive(true);
        talentCamera.gameObject.SetActive(true);
        closeButton.SetActive(true);
        enabled = true;
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
        Ray ray = talentCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
        {
            talentManagerRefrence.val.SetTargetNode(hit.collider.GetComponentInParent<NodePassive>());
        }
    }

    public void RandomPurchaseClicked()
    {
        isUnlocked = true;
        unlockPanelGO.SetActive(false);
        talentManagerRefrence.val.PurchaseRandomQubit();
        orderManagerRefrence.val.StartGame();
    }

}
