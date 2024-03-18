using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Rendering.Universal;

public class TierManager : MonoBehaviour
{
    private int unlockedTier = 0;

    private List<CraftedItem> firstTierCraftedItems = new();
    private List<CraftedItem> secondTierCraftedItems = new();
    private List<CraftedItem> thirdTierCraftedItems = new();
    private List<CraftedItem> forthTierCraftedItems = new();

    private List<PotionEffect> firstTierPotionEffects = new();
    private List<PotionEffect> secondTierPotionEffects = new();
    private List<PotionEffect> thirdTierPotionEffects = new();
    private List<PotionEffect> forthTierPotionEffects = new();


    private event Action OnTierChangedEvent;

    private TierManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (TierManagerRefrence)FindSORefrence<TierManager>.FindScriptableObject("Tier Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        InitTierCraftedList();
        SetPotionEffectTiers();
        UnlockNewTier(1);
    }

    private void InitTierCraftedList()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(BluePrintItem))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(BluePrintItem))).ToList();

        foreach (var item in craftedItems)
        {
            BluePrintItem target = (BluePrintItem)Activator.CreateInstance(item);
            int highestTier = 0;
            foreach (var material in target.materialsList)
            {
                if (material.GetItemTier() > highestTier)
                {
                    highestTier = material.GetItemTier();
                }
            }
            switch (highestTier)
            {
                case 1: firstTierCraftedItems.Add(target.CraftedItemReference()); break;
                case 2: secondTierCraftedItems.Add(target.CraftedItemReference()); break;
                case 3: thirdTierCraftedItems.Add(target.CraftedItemReference()); break;
                case 4: forthTierCraftedItems.Add(target.CraftedItemReference()); break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
        }
    }

    private void SetPotionEffectTiers()
    {
        PotionLibrary.Initialize();
        firstTierPotionEffects = PotionLibrary.firstTierBasePotion;
        secondTierPotionEffects = PotionLibrary.secondTierBasePotion;
        thirdTierPotionEffects = PotionLibrary.thirdTierBasePotion;
        forthTierPotionEffects = PotionLibrary.forthTierBasePotion;
    }

    private void UnlockNewTier(int tierNumber)
    {
        unlockedTier = tierNumber;
        OnTierChangedEvent?.Invoke();
    }


    public List<ItemBehaviour> GetNewTierCraftedItemList()
    {
        List<ItemBehaviour> targetList = new();
        if (unlockedTier == 1)
        {
            targetList.AddRange(firstTierCraftedItems);
            AddPotionsToList(firstTierPotionEffects, targetList);
        }
        else if (unlockedTier == 2)
        {
            targetList.AddRange(firstTierCraftedItems);
            targetList.AddRange(secondTierCraftedItems);
            AddPotionsToList(firstTierPotionEffects, targetList);
            AddPotionsToList(secondTierPotionEffects,targetList);
        }
        else if (unlockedTier == 3)
        {
            targetList.AddRange(firstTierCraftedItems);
            targetList.AddRange(secondTierCraftedItems);
            targetList.AddRange(thirdTierCraftedItems);
            AddPotionsToList(firstTierPotionEffects, targetList);
            AddPotionsToList(secondTierPotionEffects, targetList);
            AddPotionsToList(thirdTierPotionEffects, targetList);
        }
        else if (unlockedTier == 4)
        {
            targetList.AddRange(firstTierCraftedItems);
            targetList.AddRange(secondTierCraftedItems);
            targetList.AddRange(thirdTierCraftedItems);
            targetList.AddRange(forthTierPotionEffects);
            AddPotionsToList(firstTierPotionEffects, targetList);
            AddPotionsToList(secondTierPotionEffects, targetList);
            AddPotionsToList(thirdTierPotionEffects, targetList);
            AddPotionsToList(forthTierPotionEffects, targetList);
        }
        return targetList;
    }

    private void AddPotionsToList(List<PotionEffect> potion, List<ItemBehaviour> target)
    {
        foreach (var effect in potion)
        {
            target.Add(new PotionItem(effect));
        }
    }


    public void TierChangeAddListener(Action action)
    {
        OnTierChangedEvent += action;
    }

    public void TierChangeRemoveListener(Action action)
    {
        OnTierChangedEvent -= action;
    }
}
