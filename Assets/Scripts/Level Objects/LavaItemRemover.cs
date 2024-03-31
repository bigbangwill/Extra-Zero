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
        inventory.RemoveItemFromInventory(ItemSlot);
    }
    
    
}