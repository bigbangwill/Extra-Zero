using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStashLevelObject : MonoBehaviour
{
    [SerializeField] private ItemStash relatedItemStash;
    [SerializeField] private Transform reachingTransform;

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
        return reachingTransform;
    }


    public void AddItemToStash(ItemBehaviour item, int slot)
    {        
        if (relatedItemStash.HaveEmptySlot(item, true))
        {
            inventory.RemoveItemFromInventory(slot);
        }
    }
    
    
}