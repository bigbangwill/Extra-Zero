using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TalentRotator : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    [SerializeField] private Camera talentCamera;
    [SerializeField] private LayerMask targetMask;

    [SerializeField] private Transform TalentTree;
    [SerializeField] private Vector2 slowdown;
    [SerializeField] private float fastdown;



    

    public void OnDrag(PointerEventData eventData)
    {
        TalentTree.Rotate(new Vector3(eventData.delta.y, eventData.delta.x , 0) * slowdown,fastdown,Space.World);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Start");
        Ray ray = talentCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetMask))
        {
            Debug.Log("inside if");
            TalentManager.Instance.SetTargetNode(hit.collider.GetComponentInParent<NodePassive>());
        }
    }
}
