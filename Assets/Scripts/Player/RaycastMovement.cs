using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaycastMovement : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private PlayerMovement playerMovementScript;

    private RaycastMovementRefrence refrence;

    private bool canMove = true;

    private void Awake()
    {
        refrence = (RaycastMovementRefrence)FindSORefrence<RaycastMovement>.FindScriptableObject("Raycast Movement Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (canMove)
        {            
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(eventData.position);
            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(targetPos, Vector2.zero);
        
            Debug.Log(raycastHits.Length);
            foreach (RaycastHit2D hit in raycastHits)
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.TryGetComponent<IReacheable>(out var reacheable))
                {
                    NavmeshReachableInformation navInfo = reacheable.ReachAction();
                    playerMovementScript.MovetoTarget(navInfo);
                    Debug.Log("We are in");
                    return;
                }
            }

            playerMovementScript.MovetoTarget(targetPos);
        }
    }


    public void ShouldMove(bool shouldMove)
    {
        canMove = shouldMove;
    }

}