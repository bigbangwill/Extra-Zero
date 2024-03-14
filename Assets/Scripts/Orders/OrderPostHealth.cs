using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OrderPost))]
public class OrderPostHealth : MonoBehaviour, IRepairable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private GameObject healingBoarder;
    [SerializeField] private GameObject noHealingBoarder;

    private List<ItemBehaviour>repairTargetItemsList = new();

    private int healthUnlocked;

    private OrderPost orderPostScript;
    private int currentHealth;

    private OrderPostHealthImageSetter healthImageSetter;
    [SerializeField] private SpriteRenderer image;

    private HealthRecoverReceipe[] recoverReceipeList;

    private bool targeted = false;
    public bool Targeted { get => targeted; set => targeted = value; }


    private PlayerInventoryRefrence inventoryRefrence;
    private UsableCanvasManagerRefrence usableRefrence;


    private void LoadSORefrence()
    {
        usableRefrence = (UsableCanvasManagerRefrence)FindSORefrence<UseableItemCanvasScript>.FindScriptableObject("Usable Manager Refrence");
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
    }
    


    private void Start()
    {
        LoadSORefrence();
        healthImageSetter = GetComponent<OrderPostHealthImageSetter>();
        orderPostScript = GetComponent<OrderPost>();
        Init();
    }

    public bool NeedsRepair()
    {
        if (currentHealth >= healthUnlocked) return false;
        else return true;
    }

    public IEnumerable<ItemBehaviour> RepairMaterials()
    {
        return recoverReceipeList[currentHealth].GetItems();
    }

    public bool Repair()
    {
        repairTargetItemsList.Clear();
        Debug.Log(currentHealth);
        if(currentHealth >= healthUnlocked)
        {
            Debug.Log("nothing to repair");
            return false;
        }
        foreach (var item in recoverReceipeList[currentHealth].GetItems())
        {
            Debug.Log(item.GetName() + ": " + item.CurrentStack());
        }


        foreach (ItemBehaviour item in recoverReceipeList[currentHealth].GetItems())
        {
            repairTargetItemsList.Add(item);
            if (!inventoryRefrence.val.HaveItemInInventory(item,false))
            {
                Debug.Log("need more of " + item.GetName() + item.CurrentStack());
                return false;
            }
        }
        Debug.Log(repairTargetItemsList.Count + "kjhqlkjh");
        foreach (var item in repairTargetItemsList)
        {
            int savedStack = item.CurrentStack();
            inventoryRefrence.val.HaveItemInInventory(item,true);
            item.SetCurrentStack(savedStack);
        }
        if (currentHealth == 0)
        {
            // Implement the restore the shield  back up.
        }
        currentHealth++;
        Debug.Log($"Repaired and current health is {currentHealth}");
        TurnUIUXOn();
        SetHealthImage();
        return true;
    }


    public void TakeDamage()
    {
        currentHealth--;
        if(currentHealth <= 0 )
        {
            PostZeroHealth();
        }
        SetHealthImage();
        if (targeted)
        {
            usableRefrence.val.CallRepair();
        }
        Debug.Log(usableRefrence.val.IsOnRepairMode + "    bool cheker");
        if (usableRefrence.val.IsOnRepairMode)
        {
            TurnUIUXOn();
        }
    }


    private void SetHealthImage()
    {
        image.sprite = healthImageSetter.SetHealthImage(currentHealth);
    }

    private void PostZeroHealth()
    {
        currentHealth = 0;
    }
    private void Init()
    {
        healthUnlocked = 3;
        currentHealth = healthUnlocked;
        recoverReceipeList = new HealthRecoverReceipe[maxHealth];
        SetHealthImage();
        for (int i = 0; i < maxHealth; i++)
        {
            recoverReceipeList[i] = new HealthRecoverReceipe(i);
        }
    }

    public void RepairAmountMinusAmount(int amount)
    {
        foreach (var item in recoverReceipeList)
        {
            foreach (var mat in item.GetItems())
            {
                mat.SetCurrentStack(mat.CurrentStack() - amount);
            }
        }
    }


    public void SetUnlockedHealth(int amount)
    {
        healthUnlocked = amount;
    }

    public void TurnUIUXOn()
    {
        if (NeedsRepair())
        {
            noHealingBoarder.SetActive(false);
            healingBoarder.SetActive(true);
        }
        else
        {
            noHealingBoarder.SetActive(true);
            healingBoarder.SetActive(false);
        }
    }
    public void TurnUIUXOff()
    {
        if(healingBoarder.activeSelf)
            healingBoarder.SetActive(false);
        if(noHealingBoarder.activeSelf)
            noHealingBoarder.SetActive(false);
    }

    public Transform GetReachingTransfrom()
    {
        return orderPostScript.GetReachingTransfrom();
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