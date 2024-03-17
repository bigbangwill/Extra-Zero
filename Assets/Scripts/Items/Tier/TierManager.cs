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

    private List<CraftedItem> firstTierCraftedItems = new();
    private List<CraftedItem> secondTierCraftedItems = new();
    private List<CraftedItem> thirdTierCraftedItems = new();
    private List<CraftedItem> forthTierCraftedItems = new();

    private List<PotionEffect> firstTierPotionEffects = new();
    private List<PotionEffect> secondTierPotionEffects = new();
    private List<PotionEffect> thirdTierPotionEffects = new();
    private List<PotionEffect> forthTierPotionEffects = new();



    private void Start()
    {
        InitTierCraftedList();
        SetPotionEffectTiers();
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


}
