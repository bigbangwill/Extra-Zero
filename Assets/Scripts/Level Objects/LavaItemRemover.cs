using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaItemRemover : MonoBehaviour
{

    [SerializeField] private Transform reaching;

    private PlayerInventory inventory;

    private void LoadSORefrence()
    {
        inventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
        Debug.Log("Start");
    }

    private void Start()
    {
        LoadSORefrence();
    }

    public Transform GetReachingTransform()
    {
        return reaching;
    }

    public void InsertItem(int ItemSlot)
    {
        if(inventory == null)
            Debug.Log("Here");
        inventory.RemoveItemFromInventory(ItemSlot);
    }
    
    
}