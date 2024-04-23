using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueTouchLayer : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private DialogueManager manager;
    public void OnPointerClick(PointerEventData eventData)
    {
        manager.Skip();
    }
}