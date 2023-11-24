using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementActions : MonoBehaviour
{


    // A checker to see if the invetory is currently opened or closed
    private bool inventoryIsOpen = false;

    [SerializeField] private GameObject inventoryCanvas;

    private void Start()
    {

        // To automatically set the value of isOpen based on the current active 
        // state of the inventory canvas.
        if (inventoryCanvas.activeSelf)
        {
            inventoryIsOpen = true;
            //CloseInventory();
        }
        else
            inventoryIsOpen = false;
    }

    // The method that will be added as listener
    private void InventoryClicked()
    {
        if (!inventoryIsOpen)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        inventoryIsOpen = true;
        inventoryCanvas.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryIsOpen = false;
        inventoryCanvas.SetActive(false);
    }















    private void OnEnable()
    {
        BasementManager.Instance.InventoryEventAddListener(InventoryClicked);
    }

    private void OnDisable()
    {
        if(BasementManager.Instance != null)
            BasementManager.Instance.InventoryEventRemoveListener(InventoryClicked);
    }
}