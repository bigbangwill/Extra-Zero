using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastMovement : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private PlayerMovement playerMovementScript;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D[] raycastHits = Physics2D.RaycastAll(targetPos, Vector2.zero);
        Debug.Log(raycastHits.Length);
        foreach (RaycastHit2D hit in raycastHits)
        {
            Debug.Log(hit.collider.name);
            IReacheable reacheable = hit.collider.GetComponent<IReacheable>();
            if (reacheable != null)
            {
                NavmeshReachableInformation navInfo = reacheable.ReachAction();
                playerMovementScript.MovetoTarget(navInfo.GetDistination());
                Debug.Log("YAHOOOOOOOO");
                return;
            }
        }

        playerMovementScript.MovetoTarget(targetPos);
    }
}