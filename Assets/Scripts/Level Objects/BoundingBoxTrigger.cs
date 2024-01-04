using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxTrigger : MonoBehaviour
{
    private IInteractable interactable;

    private void Start()
    {
        interactable = GetComponentInParent<IInteractable>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactable.EnteredBox();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            interactable.ExitBox();
        }
    }


}