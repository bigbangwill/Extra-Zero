using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(OrderPost))]
public class OrderPostHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;

    private int healthUnlocked;

    private int currentHealth;


    private HealthRecoverReceipe[] recoverReceipeList;


    private bool isAtFullHealth;

    public bool NeedsRepair()
    {
        return isAtFullHealth;
    }

    public void Repair()
    {

    }


    private void TakeDamage()
    {
        currentHealth--;
        if(currentHealth <= 0 )
        {
            PostZeroHealth();
        }
    }

    private void PostZeroHealth()
    {

    }
    private void Init()
    {
        currentHealth = maxHealth;
        healthUnlocked = 3;
        recoverReceipeList = new HealthRecoverReceipe[6];
        recoverReceipeList[0] = new HealthRecoverReceipe(new MaterialItem.Plastic(10));
        recoverReceipeList[1] = new HealthRecoverReceipe(new MaterialItem.Ceramic(10));
        recoverReceipeList[3] = new HealthRecoverReceipe(new MaterialItem.AluminumAlloy(10));
        recoverReceipeList[4] = new HealthRecoverReceipe(new MaterialItem.TitaniumAlloy(10));
        recoverReceipeList[5] = new HealthRecoverReceipe(new MaterialItem.Ceramic(5), new MaterialItem.Plastic(3));


    }
}


public class HealthRecoverReceipe
{
    private List<ItemBehaviour> itemList = new();

    public HealthRecoverReceipe(params ItemBehaviour[] items)
    {
        if (items == null || items.Length == 0)
        {
            throw new ArgumentException("At least one item must be provided.");
        }

        itemList.AddRange(items);
    }


    public HealthRecoverReceipe(int stage)
    {

    }

    public IEnumerable<ItemBehaviour> GetItems()
    {
        return itemList;
    }
}