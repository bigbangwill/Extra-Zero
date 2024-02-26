using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasementActions : MonoBehaviour
{


    // A checker to see if the invetory is currently opened or closed
    private bool inventoryIsOpen = false;

    [SerializeField] private GameObject inventoryCanvas;
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














    //private void OnEnable()
    //{
    //    basementManagerRefrence.val.InventoryEventAddListener(InventoryClicked);
    //}

    //private void OnDisable()
    //{
    //    if(basementManagerRefrence.val != null)
    //        basementManagerRefrence.val.InventoryEventRemoveListener(InventoryClicked);
    //}
}