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

    [SerializeField] private int firstTierCount = 2;
    [SerializeField] private int secondTierCount = 3;
    [SerializeField] private int thirdTierCount = 5;
    [SerializeField] private int forthTierCount = 7;

    [SerializeField] private TierUI tierUI;

    private int unlockedTier = 1;

    private List<ItemBehaviour> firstTierItems = new();
    private List<ItemBehaviour> secondTierItems = new();
    private List<ItemBehaviour> thirdTierItems = new();
    private List<ItemBehaviour> forthTierItems = new();


    private List<ItemBehaviour> milestoneFirstTier = new();
    private List<ItemBehaviour> milestoneSecondTier = new();
    private List<ItemBehaviour> milestoneThirdTier = new();
    private List<ItemBehaviour> milestoneForthTier = new();

    private List<ItemBehaviour> currentActiveMilestone;




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
        InitTierCraftedList();
        SetPotionEffectTiers();
        SetupMilestoneTierLists();
    }

    private void Start()
    {
        //SetDefault();
        UnlockNewTier(1);
        tierUI.SetFirstTier(milestoneFirstTier);
        tierUI.SetSecondTier(milestoneSecondTier);
        tierUI.SetThirdTier(milestoneThirdTier);
        tierUI.SetForthTier(milestoneForthTier);
    }

    private void SetDefault()
    {
        firstTierItems = new();
        secondTierItems = new();
        thirdTierItems = new();
        forthTierItems = new();
        milestoneFirstTier = new();
        milestoneSecondTier = new();
        milestoneThirdTier = new();
        milestoneForthTier = new();
        currentActiveMilestone = new();
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
                case 1: firstTierItems.Add(target.CraftedItemReference()); break;
                case 2: secondTierItems.Add(target.CraftedItemReference()); break;
                case 3: thirdTierItems.Add(target.CraftedItemReference()); break;
                case 4: forthTierItems.Add(target.CraftedItemReference()); break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
        }

        craftedItems.Clear();
        craftedItems = Assembly.GetAssembly(typeof(MaterialItem))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(MaterialItem))).ToList();

        foreach(var item in craftedItems) 
        {
            MaterialItem target = (MaterialItem)Activator.CreateInstance(item);
            firstTierItems.Add(target);
        }
        craftedItems.Clear();
        craftedItems = Assembly.GetAssembly(typeof(Herb))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(Herb))).ToList();

        foreach (var item in craftedItems)
        {
            Herb target = (Herb)Activator.CreateInstance(item);
            firstTierItems.Add(target);
        }

        craftedItems.Clear();
        craftedItems = Assembly.GetAssembly(typeof(Seed))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(Seed))).ToList();

        foreach (var item in craftedItems)
        {
            Seed Target = (Seed)Activator.CreateInstance(item);
            firstTierItems.Add(Target);
        }
    }

    private void SetPotionEffectTiers()
    {
        PotionLibrary.Initialize();

        AddPotionsToList(PotionLibrary.firstTierBasePotion, firstTierItems);
        AddPotionsToList(PotionLibrary.secondTierBasePotion, secondTierItems);
        AddPotionsToList(PotionLibrary.thirdTierBasePotion, thirdTierItems);
        AddPotionsToList(PotionLibrary.forthTierBasePotion, forthTierItems);
    }

    private void SetupMilestoneTierLists()
    {
        List<ItemBehaviour> firstNonMaterialList = new();
        List<ItemBehaviour> secondNonMaterialList = new();
        List<ItemBehaviour> thirdNonMaterialList = new();
        List<ItemBehaviour> forthNonMaterialList = new();
        foreach (var item in firstTierItems)
        {
            if (item.ItemTypeValue() == ItemType.craftedItem || item.ItemTypeValue() == ItemType.potion)
            {
                firstNonMaterialList.Add(item);
            }
        }
        foreach(var item in secondTierItems)
        {
            secondNonMaterialList.Add(item);
        }
        foreach (var item in thirdTierItems)
        {
            thirdNonMaterialList.Add(item);
        }
        foreach (var item in forthTierItems)
        {
            forthNonMaterialList.Add(item);
        }

        int safeChecker = firstNonMaterialList.Count;
        if (safeChecker >= firstTierCount)
            safeChecker = firstTierCount;
        for(int i = 0; i <safeChecker; i++)
        {
            ItemBehaviour target = firstNonMaterialList[UnityEngine.Random.Range(0, firstNonMaterialList.Count)];
            firstNonMaterialList.Remove(target);
            milestoneFirstTier.Add(target);
        }
        safeChecker = secondNonMaterialList.Count;
        if(safeChecker >= secondTierCount)
            safeChecker = secondTierCount;
        for (int i = 0; i < safeChecker; i++)
        {
            ItemBehaviour target = secondNonMaterialList[UnityEngine.Random.Range(0, secondNonMaterialList.Count)];
            secondNonMaterialList.Remove(target);
            milestoneSecondTier.Add(target);
        }
        safeChecker = thirdNonMaterialList.Count;
        if (safeChecker >= thirdTierCount)
            safeChecker = thirdTierCount;
        for (int i = 0; i < safeChecker; i++)
        {
            ItemBehaviour target = thirdNonMaterialList[UnityEngine.Random.Range(0,thirdNonMaterialList.Count)];
            thirdNonMaterialList.Remove(target);
            milestoneThirdTier.Add(target);
        }
        safeChecker = forthNonMaterialList.Count;
        if (safeChecker >= forthNonMaterialList.Count)
            safeChecker = forthTierCount;
        for (int i = 0; i < safeChecker; i++)
        {
            ItemBehaviour target = forthNonMaterialList[UnityEngine.Random.Range(0, forthNonMaterialList.Count)];
            forthNonMaterialList.Remove(target);
            milestoneForthTier.Add(target);
        }
    }


    public void MilestoneCheckItem(List<ItemBehaviour> holdingItems)
    {
        foreach (var item in holdingItems)
        {
            foreach (var mileItem in currentActiveMilestone.ToList())
            {
                if (mileItem.Equals(item))
                {
                    currentActiveMilestone.Remove(mileItem);
                    tierUI.CheckItemInDictionary(mileItem);
                    if (currentActiveMilestone.Count == 0)
                    {
                        int newTier = unlockedTier + 1;
                        UnlockNewTier(newTier);
                        return;
                    }
                }
            }
        }
    }

    private void UnlockNewTier(int tierNumber)
    {
        unlockedTier = tierNumber;
        switch (unlockedTier)
        {
            case 1: currentActiveMilestone = milestoneFirstTier; break;
            case 2: currentActiveMilestone = milestoneSecondTier; break;
            case 3: currentActiveMilestone = milestoneThirdTier; break;
            case 4:currentActiveMilestone = milestoneForthTier; break;
            default: Debug.LogWarning("Check here asap"); break;
        }

        OnTierChangedEvent?.Invoke();
    }


    public List<ItemBehaviour> GetNewTierCraftedItemList()
    {
        List<ItemBehaviour> targetList = new();
        if (unlockedTier == 1)
        {
            targetList.AddRange(firstTierItems);
        }
        else if (unlockedTier == 2)
        {
            targetList.AddRange(firstTierItems);
            targetList.AddRange(secondTierItems);
        }
        else if (unlockedTier == 3)
        {
            targetList.AddRange(firstTierItems);
            targetList.AddRange(secondTierItems);
            targetList.AddRange(thirdTierItems);
        }
        else if (unlockedTier == 4)
        {
            targetList.AddRange(firstTierItems);
            targetList.AddRange(secondTierItems);
            targetList.AddRange(thirdTierItems);
            targetList.AddRange(forthTierItems);
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
