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
    private int _CurrentHealth { get { return currentHealth; } set { currentHealth = value; /*SetAnimation();*/ }  }

    private OrderPostHealthImageSetter healthImageSetter;
    [SerializeField] private SpriteRenderer image;

    private HealthRecoverReceipe[] recoverReceipeList;

    private bool targeted = false;
    public bool Targeted { get => targeted; set => targeted = value; }

    [SerializeField] private CurrentGameStateSetter currentGameStateSetter;

    [SerializeField] private Animator animator;
    [SerializeField] private Animator damageAnimtor;
    [SerializeField] private Animator healAnimtor;


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
        SetAnimation();
    }

    public bool NeedsRepair()
    {
        if (_CurrentHealth >= healthUnlocked) return false;
        else return true;
    }

    public IEnumerable<ItemBehaviour> RepairMaterials()
    {
        return recoverReceipeList[_CurrentHealth].GetItems();
    }

    public bool Repair()
    {
        repairTargetItemsList.Clear();
        Debug.Log(_CurrentHealth);
        if(_CurrentHealth >= healthUnlocked)
        {
            Debug.Log("nothing to repair");
            return false;
        }
        foreach (var item in recoverReceipeList[_CurrentHealth].GetItems())
        {
            Debug.Log(item.GetName() + ": " + item.CurrentStack());
        }


        foreach (ItemBehaviour item in recoverReceipeList[_CurrentHealth].GetItems())
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
        if (_CurrentHealth == 0)
        {
            // Implement the restore the shield  back up.
        }
        _CurrentHealth++;
        Debug.Log($"Repaired and current health is {_CurrentHealth}");
        TurnUIUXOn();
        StartCoroutine(HealingCou());
        //SetHealthImage();
        return true;
    }


    public void TakeDamage()
    {
        _CurrentHealth--;
        if(_CurrentHealth < 0 )
        {
            PostZeroHealth();
        }
        else
        {
            StartCoroutine(DamageCou());
        }
        //SetHealthImage();
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

    private IEnumerator DamageCou()
    {
        while(true)
        {
            damageAnimtor.SetTrigger("Damaged");
            yield return new WaitForSeconds(0.7f);
            SetAnimation();
            yield break;
        }
    }

    private IEnumerator HealingCou()
    {
        while (true)
        {
            damageAnimtor.SetTrigger("Healing");
            yield return new WaitForSeconds(0.7f);
            SetAnimation();
            yield break;
        }
    }



    private void SetHealthImage()
    {
        image.sprite = healthImageSetter.SetHealthImage(_CurrentHealth);
    }

    private void PostZeroHealth()
    {
        _CurrentHealth = 0;
        currentGameStateSetter.GameIsLost();
    }
    private void Init()
    {
        healthUnlocked = 3;
        _CurrentHealth = healthUnlocked;
        recoverReceipeList = new HealthRecoverReceipe[maxHealth];
        //SetHealthImage();
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


    

    private void SetAnimation()
    {
        switch (_CurrentHealth)
        {
            case 0: animator.SetTrigger("Transparent Trigger"); break;
            case 1: animator.SetTrigger("Red Trigger"); break;
            case 2: animator.SetTrigger("Orange Trigger"); break;
            case 3: animator.SetTrigger("Yellow Trigger"); break;
            case 4: animator.SetTrigger("Green Trigger"); break;
            case 5: animator.SetTrigger("Blue Trigger"); break;
            default: animator.SetTrigger("Transparent Trigger");Debug.LogWarning("CHECK HERE ASAP"); break;
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
            default: itemList.Add(new MaterialItem.AluminumAlloy(10)); Debug.LogWarning("Might need to check here"); break;
        }
    }

    public IEnumerable<ItemBehaviour> GetItems()
    {
        return itemList;
    }
}