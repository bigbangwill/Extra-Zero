using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotReaderHandlePressMiniGame : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private SlotReaderMiniGame miniGame;

    Quaternion startRot = Quaternion.identity;

    private void Start()
    {
        miniGame = GetComponentInParent<SlotReaderMiniGame>();
        startRot = Quaternion.identity;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //transform.Rotate(new Vector3 (90,0,0));
    }

    public void OnDrag(PointerEventData eventData)
    {
        miniGame.MouseIsPressed(eventData);
        transform.rotation = startRot;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.rotation = startRot;
    }

}