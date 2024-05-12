using NavMeshPlus.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;

public class NewTierManager : MonoBehaviour
{
    [SerializeField] private int firstTierCount = 2;
    [SerializeField] private int secondTierCount = 3;
    [SerializeField] private int thirdTierCount = 5;
    [SerializeField] private int forthTierCount = 7;

    [SerializeField] private TierUI tierUI;

    [SerializeField] private GameObject winScreenCanvas;
    [SerializeField] private NavMeshSurface surface;

    private int unlockedTier = 1;

    public int UnlockedTier { get => unlockedTier; }

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


    private EconomyManager economyManager;
    private EventTextManager eventTextManager;

    private NewTierManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (NewTierManagerRefrence)FindSORefrence<NewTierManager>.FindScriptableObject("New Tier Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
        }
        refrence.val = this;
    }

    private void LoadSORefrence()
    {
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;

    }


    private void Awake()
    {
        SetRefrence();
        //if (GameModeState.IsCampaignMode)
        //{
        //    InitNewTierCraftedList();
        //    SetupCampaignMilestone();
        //}
        //else
        //{
        //    InitTierCraftedList();        
        //    SetPotionEffectTiers();
        //    SetupMilestoneTierLists();
        //}

        InitNewTierCraftedList();
        
        SetupCampaignMilestone();
    }

    private void Start()
    {
        //SetDefault();
        LoadSORefrence();
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

    private void InitNewTierCraftedList()
    {

        //For blueprints
        if (GameModeState.ShopStationIsUnlocked)
        {
            AddShopStation();
        }

        //For crafting items
        if (GameModeState.ComputerIsUnlocked && GameModeState.ScannerIsUnlocked && GameModeState.PrinterIsUnlocked)
        {
            AddCraftedItems();
        }

        //For mineral
        if (GameModeState.MaterialFarmIsUnlocked)
        {
            AddMaterialFarm();
        }

        //For Seeds
        if (GameModeState.SeedFarmIsUnlocked)
        {
            AddSeedFarm();
        }

        //For Herbs
        if (GameModeState.HerbalismPostIsUnlocked)
        {
            AddHerbs();
        }

        if (GameModeState.AlchemyPostIsUnlocked)
        {
            SetPotionEffectTiers();
        }

        if (GameModeState.MilestoneReward != CampaignRewardEnum.none && !CheckIsUnlocked())
        {
            AddMileStone();
        }

    }

    private bool CheckIsUnlocked()
    {
        switch (GameModeState.MilestoneReward)
        {
            case CampaignRewardEnum.shopStation: if (GameModeState.ShopStationIsUnlocked) return true; break;
            case CampaignRewardEnum.materialFarm: if (GameModeState.MaterialFarmIsUnlocked) return true; break;
            case CampaignRewardEnum.seedFarm: if (GameModeState.SeedFarmIsUnlocked) return true; break;
            case CampaignRewardEnum.herbalismPost: if (GameModeState.HerbalismPostIsUnlocked) return true; break;
            case CampaignRewardEnum.alchemyPost: if (GameModeState.AlchemyPostIsUnlocked) return true; break;
            case CampaignRewardEnum.computer: if (GameModeState.ComputerIsUnlocked) return true; break;
            case CampaignRewardEnum.scanner: if(GameModeState.ScannerIsUnlocked) return true; break;
            case CampaignRewardEnum.printer: if (GameModeState.PrinterIsUnlocked) return true; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); return true;
        }
        return false;
    }


    private void AddMileStone()
    {
        CampaignRewardEnum milestone = GameModeState.MilestoneReward;
        switch (milestone)
        {
            case CampaignRewardEnum.materialFarm: AddMaterialFarm(); break;
            case CampaignRewardEnum.seedFarm: AddSeedFarm(); break;
            case CampaignRewardEnum.herbalismPost: AddHerbs(); break;
            case CampaignRewardEnum.shopStation: AddShopStation(); break;
            case CampaignRewardEnum.alchemyPost:
                if (GameModeState.HerbalismPostIsUnlocked)
                {
                    SetPotionEffectTiers();
                }
                break;
            case CampaignRewardEnum.computer:
                if (GameModeState.ComputerIsUnlocked && GameModeState.ScannerIsUnlocked && GameModeState.PrinterIsUnlocked)
                {
                    AddCraftedItems();
                }
                break;
            case CampaignRewardEnum.scanner:
                if (GameModeState.ComputerIsUnlocked && GameModeState.ScannerIsUnlocked && GameModeState.PrinterIsUnlocked)
                {
                    AddCraftedItems();
                }
                break;
            case CampaignRewardEnum.printer:
                if (GameModeState.ComputerIsUnlocked && GameModeState.ScannerIsUnlocked && GameModeState.PrinterIsUnlocked)
                {
                    AddCraftedItems();
                }
                break;
            default: break;
        }
    }

    private void AddShopStation()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(BluePrintItem)).GetTypes().ToList().
                Where(Thetype => Thetype.IsClass &&
                !Thetype.IsAbstract && Thetype.IsSubclassOf(typeof(BluePrintItem))).ToList();

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
                case 1: firstTierItems.Add(target); break;
                case 2: secondTierItems.Add(target); break;
                case 3: thirdTierItems.Add(target); break;
                case 4: forthTierItems.Add(target); break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
        }
    }

    private void AddCraftedItems()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(BluePrintItem)).GetTypes().ToList().
            Where(Thetype => Thetype.IsClass &&
            !Thetype.IsAbstract && Thetype.IsSubclassOf(typeof(BluePrintItem))).ToList();

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
    }

    private void AddMaterialFarm()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(MaterialItem))
            .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(MaterialItem))).ToList();
        foreach (var item in craftedItems)
        {
            MaterialItem target = (MaterialItem)Activator.CreateInstance(item);
            switch (target.GetItemTier())
            {
                case 1: firstTierItems.Add(target); break;
                case 2: secondTierItems.Add(target); break;
                case 3: thirdTierItems.Add(target); break;
                case 4: forthTierItems.Add(target); break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
            //firstTierItems.Add(target);
        }
    }

    private void AddSeedFarm()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(Seed))
            .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(Seed))).ToList();
        foreach (var item in craftedItems)
        {
            Seed target = (Seed)Activator.CreateInstance(item);
            switch (target.GetSeedTier())
            {
                case 1: firstTierItems.Add(target); break;
                case 2: secondTierItems.Add(target); break;
                case 3: thirdTierItems.Add(target); break;
                case 4: forthTierItems.Add(target); break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
            //firstTierItems.Add(Target);
        }
    }

    private void AddHerbs()
    {
        List<Type> craftedItems = Assembly.GetAssembly(typeof(Herb))
            .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(Herb))).ToList();
        foreach (var item in craftedItems)
        {
            Herb target = (Herb)Activator.CreateInstance(item);
            switch (target.GetHerbTier())
            {
                case 1: firstTierItems.Add(target); break;
                case 2: secondTierItems.Add(target); break;
                case 3: thirdTierItems.Add(target); break;
                case 4: forthTierItems.Add(target); break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
            //firstTierItems.Add(target);
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
            if (item.GetItemTypeValue() == ItemType.craftedItem || item.GetItemTypeValue() == ItemType.potion)
            {
                firstNonMaterialList.Add(item);
            }
        }
        foreach (var item in secondTierItems)
        {
            if (item.GetItemTypeValue() == ItemType.craftedItem || item.GetItemTypeValue() == ItemType.potion)
            {
                secondNonMaterialList.Add(item);
            }
        }
        foreach (var item in thirdTierItems)
        {
            if (item.GetItemTypeValue() == ItemType.craftedItem || item.GetItemTypeValue() == ItemType.potion)
            {
                thirdNonMaterialList.Add(item);
            }
        }
        foreach (var item in forthTierItems)
        {
            if (item.GetItemTypeValue() == ItemType.craftedItem || item.GetItemTypeValue() == ItemType.potion)
            {
                forthNonMaterialList.Add(item);
            }
        }

        int safeChecker = firstNonMaterialList.Count;
        if (safeChecker >= firstTierCount)
            safeChecker = firstTierCount;
        for (int i = 0; i < safeChecker; i++)
        {
            ItemBehaviour target = firstNonMaterialList[UnityEngine.Random.Range(0, firstNonMaterialList.Count)];
            firstNonMaterialList.Remove(target);
            milestoneFirstTier.Add(target);
        }
        safeChecker = secondNonMaterialList.Count;
        if (safeChecker >= secondTierCount)
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
            ItemBehaviour target = thirdNonMaterialList[UnityEngine.Random.Range(0, thirdNonMaterialList.Count)];
            thirdNonMaterialList.Remove(target);
            milestoneThirdTier.Add(target);
        }
        safeChecker = forthNonMaterialList.Count;
        if (safeChecker >= forthTierCount)
            safeChecker = forthTierCount;
        for (int i = 0; i < safeChecker; i++)
        {
            ItemBehaviour target = forthNonMaterialList[UnityEngine.Random.Range(0, forthNonMaterialList.Count)];
            forthNonMaterialList.Remove(target);
            milestoneForthTier.Add(target);
        }
    }

    private void SetupCampaignMilestone()
    {
        List<ItemBehaviour> firstNonMaterialList = new();
        List<ItemBehaviour> secondNonMaterialList = new();
        List<ItemBehaviour> thirdNonMaterialList = new();
        List<ItemBehaviour> forthNonMaterialList = new();
        foreach (var item in firstTierItems)
        {
            firstNonMaterialList.Add(item);
        }
        foreach (var item in secondTierItems)
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
        for (int i = 0; i < safeChecker; i++)
        {
            ItemBehaviour target = firstNonMaterialList[UnityEngine.Random.Range(0, firstNonMaterialList.Count)];
            firstNonMaterialList.Remove(target);
            milestoneFirstTier.Add(target);
        }
        safeChecker = secondNonMaterialList.Count;
        if (safeChecker >= secondTierCount)
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
            ItemBehaviour target = thirdNonMaterialList[UnityEngine.Random.Range(0, thirdNonMaterialList.Count)];
            thirdNonMaterialList.Remove(target);
            milestoneThirdTier.Add(target);
        }
        safeChecker = forthNonMaterialList.Count;
        if (safeChecker >= forthTierCount)
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
                        GiveTierUnlockReward();
                        int newTier = unlockedTier + 1;
                        UnlockNewTier(newTier);
                        return;
                    }
                }
            }
        }
    }

    private void GiveTierUnlockReward()
    {
        switch (unlockedTier)
        {
            case 0: economyManager.OutGameCurrencyCurrentStack += 1; break;
            case 1: economyManager.OutGameCurrencyCurrentStack += 2; break;
            case 2: economyManager.OutGameCurrencyCurrentStack += 3; break;
            case 3: economyManager.OutGameCurrencyCurrentStack += 4; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }


    public ItemBehaviour GetRandomCurrentMilestoneItem()
    {
        ItemBehaviour target = currentActiveMilestone[UnityEngine.Random.Range(0, currentActiveMilestone.Count)];
        return target;
    }

    private void UnlockNewTier(int tierNumber)
    {
        unlockedTier = tierNumber;
        switch (unlockedTier)
        {
            case 1: currentActiveMilestone = milestoneFirstTier; break;
            case 2: currentActiveMilestone = milestoneSecondTier; eventTextManager.CreateNewText("Tier Upgraded", TextType.Information); break;
            case 3: currentActiveMilestone = milestoneThirdTier; eventTextManager.CreateNewText("Tier Upgraded", TextType.Information);break;
            case 4: currentActiveMilestone = milestoneForthTier; eventTextManager.CreateNewText("Tier Upgraded", TextType.Information); break;
            case 5: winScreenCanvas.gameObject.SetActive(true); break;
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