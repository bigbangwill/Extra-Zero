using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CoverPickUpItem : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] ItemPrinter itemPrinter;

    public void OnPointerDown(PointerEventData eventData)
    {
        itemPrinter.CoverClicked();
    }
}