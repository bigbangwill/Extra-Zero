using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFarmPouch : MonoBehaviour
{
    
    private PlayerInventory inventory;

    private void LoadSoRefrence()
    {
        inventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
    }

    private void Start()
    {
        LoadSoRefrence();
    }


    public void HitWithSeed(Seed seed)
    {
        if (inventory.HaveEmptySlot(seed, true))
        {
            Debug.Log("Added Seed" + seed.GetName() + "Current Stack" + seed.CurrentStack());
        }
        else
        {
            Debug.Log("Not enough space");
        }
    }
}
