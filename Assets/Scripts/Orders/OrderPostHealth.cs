using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(OrderPost))]
public class OrderPostHealth : MonoBehaviour, IRepairable
{
    [SerializeField] private int maxHealth;

    private List<ItemBehaviour>repairTargetItemsList = new();

    private int healthUnlocked;

    private int currentHealth;


    private HealthRecoverReceipe[] recoverReceipeList;


    private bool isAtFullHealth;

    private void Start()
    {
        Init();
    }

    public bool NeedsRepair()
    {
        return isAtFullHealth;
    }

    public void Repair()
    {
        if(currentHealth >= healthUnlocked)
        {
            Debug.Log("nothing to repair");
            return;
        }

        bool playerHasItems = true;
        foreach (ItemBehaviour item in recoverReceipeList[currentHealth].GetItems())
        {
            repairTargetItemsList.Add(item);
            if (!PlayerInventory.Instance.HaveItemInInventory(item,false))
            {
                playerHasItems = false;
            }
        }
        if (playerHasItems)
        {
            foreach (var item in repairTargetItemsList)
            {
                PlayerInventory.Instance.HaveItemInInventory(item,true);
            }
            if (currentHealth == 0)
            {
                // Implement the restore the shield  back up.
            }
            currentHealth++;
            if (currentHealth == healthUnlocked)
            {
                isAtFullHealth = true;
            }
        }
    }


    private void TakeDamage()
    {
        currentHealth--;
        isAtFullHealth = false;
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
        for(int i = 0; i < maxHealth; i++)
        {
            recoverReceipeList[i] = new HealthRecoverReceipe(i);
        }


    }
}


public class HealthRecoverReceipe
{
    private List<ItemBehaviour> itemList = new();


    public HealthRecoverReceipe(int stage)
    {
        switch (stage)
        {
            case 0: itemList.Add(new MaterialItem.Ceramic(5)); break;
            case 1: itemList.Add(new MaterialItem.AluminumAlloy(5)); break;
            case 2: itemList.Add(new MaterialItem.Plastic(5)); break;
            case 3: itemList.Add(new MaterialItem.StainlessSteel(5)); break;
            case 4: itemList.Add(new MaterialItem.TitaniumAlloy(5)); break;
            default: itemList.Add(new MaterialItem.AluminumAlloy(10)); break;
        }
    }

    public IEnumerable<ItemBehaviour> GetItems()
    {
        return itemList;
    }
}