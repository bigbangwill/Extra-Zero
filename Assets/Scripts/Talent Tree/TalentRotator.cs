using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TalentRotator : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    [SerializeField] private Transform TalentTree;

    [SerializeField] private Vector2 slowdown;
    [SerializeField] private float fastdown;


    

    public void OnDrag(PointerEventData eventData)
    {
        TalentTree.Rotate(new Vector3(eventData.delta.y, eventData.delta.x , 0) * slowdown,fastdown,Space.World);
        //TalentTree.rotation *= Quaternion.AngleAxis(fastdown * Time.deltaTime, eventData.delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }
}
