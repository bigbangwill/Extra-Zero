using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public delegate void PotionEffectDelegate();

public static class PotionLibrary
{
    private static bool isInited = false;

    private static Dictionary<PotionCombination, PotionEffect> potionEffectDictionary = new();

    public readonly static List<PotionEffect> firstTierBasePotion = new();
    public readonly static List<PotionEffect> secondTierBasePotion = new();
    public readonly static List<PotionEffect> thirdTierBasePotion = new();
    public readonly static List<PotionEffect> forthTierBasePotion = new();



    private static BuffSystemManager buffSystemManager;
    private static PlayerMovement playerMovement;
    private static OrderPostsUpgradeManager orderPostsUpgradeManager;
    private static AlchemyPost alchemyPost;
    private static MaterialFarmManager materialFarmManager;


    public static BuffSystemManager BuffSystemManager { get => buffSystemManager; }
    public static PlayerMovement PlayerMovement { get => playerMovement; }
    public static OrderPostsUpgradeManager OrderPostsUpgradeManager { get => orderPostsUpgradeManager; }
    public static AlchemyPost AlchemyPost { get => alchemyPost; }
    public static MaterialFarmManager MaterialFarmManager { get => materialFarmManager; }


    private static void LoadSORefrence()
    {
        buffSystemManager = ((BuffSystemManagerRefrence)FindSORefrence<BuffSystemManager>.FindScriptableObject("Buff System Manager Refrence")).val;
        playerMovement = ((PlayerMovementRefrence)FindSORefrence<PlayerMovement>.FindScriptableObject("Player Movement Refrence")).val;
        orderPostsUpgradeManager = ((OrderPostsUpgradeManagerRefrence)FindSORefrence<OrderPostsUpgradeManager>.FindScriptableObject("Order Posts Upgrade Manager Refrence")).val;
        alchemyPost = ((AlchemyPostRefrence)FindSORefrence<AlchemyPost>.FindScriptableObject("Alchemy Post Refrence")).val;
        materialFarmManager = ((MaterialFarmManagerRefrence)FindSORefrence<MaterialFarmManager>.FindScriptableObject("Materialfarm Manager Refrence")).val;
    }


    public static void Initialize()
    {
        LoadSORefrence();
        if (isInited)
            return;
        potionEffectDictionary.Clear();
        List<Type> potionEffects = Assembly.GetAssembly(typeof(PotionEffect))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(PotionEffect))).ToList();

        foreach (var effect in potionEffects)
        {
            PotionEffect target = (PotionEffect)Activator.CreateInstance(effect);
            target.AddRecepieToCombination();
            if (target.isBase)
            {
                int highestTier = 0;
                foreach (Herb herb in target.HerbReceipe())
                {
                    if (herb.GetHerbTier() > highestTier)
                    {
                        highestTier = herb.GetHerbTier();
                    }
                }
                switch (highestTier) 
                {
                    case 1: firstTierBasePotion.Add(target); break;
                    case 2: secondTierBasePotion.Add(target); break;
                    case 3: thirdTierBasePotion.Add(target); break;
                    case 4: forthTierBasePotion.Add(target); break;
                    default: Debug.LogWarning("CHECK HERE ASAP"); break;
                }
            }
        }
        isInited = true;
    }

    // to add the related combination herbs and potion effect to the dictionary.
    public static void AddCombination(List<Herb> herbs, PotionEffect potionEffectDelegate)
    {
        Herb[] herbsArray = new Herb[3];
        herbsArray[0] = herbs[0];
        herbsArray[1] = herbs[1];
        herbsArray[2] = herbs[2];
        Array.Sort(herbsArray);
        PotionCombination combination = new(herbsArray[0], herbsArray[1], herbsArray[2]);
        potionEffectDictionary.Add(combination, potionEffectDelegate);
    }


    public static PotionEffect FindEffect(Herb first,Herb second,Herb third)
    {
        Herb[] herbsArray = new Herb[3];
        herbsArray[0] = first;
        herbsArray[1] = second;
        herbsArray[2] = third;
        Array.Sort(herbsArray);
        PotionCombination target = new(herbsArray[0], herbsArray[1], herbsArray[2]);
        if (potionEffectDictionary.ContainsKey(target))
            return potionEffectDictionary[target];
        else 
            return new PotionEffect.EmptyEffect();
    }
}

/// <summary>
/// The class to put each combination of the herbs in and turn them into string by their name to make it 
/// ready for the dictionary to give the related potion effect.
/// </summary>
public class PotionCombination
{
    public string first, second, third;
    public PotionCombination(Herb first, Herb second, Herb third)
    {
        this.first = first.GetName();
        this.second = second.GetName();
        this.third = third.GetName();
    }

    public override bool Equals(object obj)
    {
        return obj is PotionCombination combination &&
               first == combination.first &&
               second == combination.second &&
               third == combination.third;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(first, second, third);
    }
}

/// <summary>
/// A class for each of the effects that are in gonna be in the game.
/// </summary>
public abstract class PotionEffect
{
    public enum SideEffect { Necrotic, Regenerative, Natural};
    public SideEffect sideEffect;
    public string name;
    public string specificAddress;
    public PotionEffectDelegate effect;
    public Sprite sprite;
    public bool isBase;
    private List<Herb> receipeList = new();

    protected int potionPriority;

    public override bool Equals(object obj)
    {
        return obj is PotionEffect effect &&
               name == effect.name &&
               specificAddress == effect.specificAddress &&
               potionPriority == effect.potionPriority;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, specificAddress, potionPriority);
    }

    public int Priority()
    {
        return potionPriority;
    }

    public SideEffect GetSideEffect()
    {
        return sideEffect;
    }

    public string GetEffectName()
    {
        return name;
    }

    public List<Herb> HerbReceipe()
    {
        return receipeList;
    }

    public abstract void BotsEffectVoid();

    public abstract void PlayerEffectVoid();

    public abstract void AddRecepieToCombination();

    public abstract void AddHerbToReceipeList();


    protected void LoadIcon()
    {
        specificAddress = "Potion Effect/" + name;
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(specificAddress);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            sprite = handle.Result;
            //Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load the asset");
        }
    }


    public class EmptyEffect : PotionEffect
    {
        public EmptyEffect()
        {
            effect = BotsEffectVoid;
            name = "Empty";
            potionPriority = int.MaxValue;
            sideEffect = SideEffect.Natural;
            isBase = false;
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            Debug.LogWarning("CHECK HERE ASAP");
        }

        public override void AddRecepieToCombination()
        {
            Debug.Log("Empty");
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("Empty");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("Empty");
        }
    }



    public class MineralOilEffect : PotionEffect
    {
        public MineralOilEffect()
        {
            effect = BotsEffectVoid;
            name = "Mineral Oil";
            sideEffect = SideEffect.Natural;
            potionPriority = 0;
            isBase = true;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Sage());
            receipeList.Add(new Herb.Sage());
            receipeList.Add(new Herb.Lavender());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("Base Effect");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("Base Effect");
        }
    }

    public class SyntheticOilEffect : PotionEffect
    {
        public SyntheticOilEffect()
        {
            effect = BotsEffectVoid;
            name = "Synthetic Oil";
            sideEffect = SideEffect.Natural;
            potionPriority = 0;
            isBase = true;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Lavender());
            receipeList.Add(new Herb.Lavender());
            receipeList.Add(new Herb.Chamomile());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("Base Effect");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("Base Effect");
        }
    }

    public class DrinkingPotionEffect : PotionEffect
    {
        public DrinkingPotionEffect()
        {
            effect = PlayerEffectVoid;
            name = "Drinking Potion";
            sideEffect = SideEffect.Natural;
            potionPriority = 0;
            isBase = true;
            AddHerbToReceipeList();
            LoadIcon();
        }

        //Chamomile + Chamomile + Lavender
        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Lavender());
            receipeList.Add(new Herb.Chamomile());
            receipeList.Add(new Herb.Chamomile());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("Base Effect");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("Base Effect");
        }
    }

    public class SpeedEffect : PotionEffect
    {
        public SpeedEffect()
        {
            effect = BotsEffectVoid;
            name = "Speed";
            sideEffect = SideEffect.Regenerative;
            potionPriority = 1;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {            
            receipeList.Add(new Herb.Sage());
            receipeList.Add(new Herb.Sage());
            receipeList.Add(new Herb.Sage());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            PotionLibrary.BuffSystemManager.AddBuffEffect(BotsStartEffect, BotsEndEffect, 5);
            Debug.Log(name);
        }

        public override void PlayerEffectVoid()
        {
            PotionLibrary.BuffSystemManager.AddBuffEffect(PlayerStartEffect, PlayerEndEffect, 5);
            Debug.Log(name + "Player");
        }

        private void BotsStartEffect()
        {
            PotionLibrary.OrderPostsUpgradeManager.SpeedPotionBuff(10f);
        }

        private void BotsEndEffect()
        {
            PotionLibrary.OrderPostsUpgradeManager.SpeedPotionReset();
        }

        private void PlayerStartEffect()
        {
            PotionLibrary.PlayerMovement.MovementSpeedBuff(20);
        }

        private void PlayerEndEffect()
        {
            PotionLibrary.PlayerMovement.MovementSpeedReset();
        }
    }

    public class SecondEffect : PotionEffect
    {
        public SecondEffect()
        {
            effect = BotsEffectVoid;
            name = "Lucky";
            sideEffect = SideEffect.Regenerative;
            potionPriority = 2;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Lavender());
            receipeList.Add(new Herb.Lavender());
            receipeList.Add(new Herb.Lavender());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("Second Effect");
        }

        public override void PlayerEffectVoid()
        {
            PotionLibrary.BuffSystemManager.AddBuffEffect(PlayerStartEffect, PlayerEndEffect, 15);
            Debug.Log(name + "PLayer");
        }

        private void PlayerStartEffect()
        {
            PotionLibrary.AlchemyPost.LuckyPotionBuff(10);
        }

        private void PlayerEndEffect()
        {
            PotionLibrary.AlchemyPost.LuckyPotionReset();
        }



    }
    public class ThirdEffect : PotionEffect
    {
        public ThirdEffect()
        {
            effect = BotsEffectVoid;
            name = "Strenght";
            sideEffect = SideEffect.Regenerative;
            potionPriority = 3;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Chamomile());
            receipeList.Add(new Herb.Chamomile());
            receipeList.Add(new Herb.Chamomile());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            PotionLibrary.BuffSystemManager.AddBuffEffect(BotsStartEffect, BotsEndEffect, 10);
            Debug.Log(name);
        }

        public override void PlayerEffectVoid()
        {
            PotionLibrary.BuffSystemManager.AddBuffEffect(PlayerStartEffect, PlayerEndEffect, 10);
            Debug.Log(name + "Player");
        }


        private void PlayerStartEffect()
        {
            PotionLibrary.MaterialFarmManager.StrenghtPotionBuff();
        }

        private void PlayerEndEffect()
        {
            PotionLibrary.MaterialFarmManager.StrenghtPotionReset();
        }

        private void BotsStartEffect()
        {
            PotionLibrary.OrderPostsUpgradeManager.StrenghtPotionBuff(0.1f);
        }

        private void BotsEndEffect()
        {
            PotionLibrary.OrderPostsUpgradeManager.StrenghtPotionReset();
        }


    }

    public class ForthEffect : PotionEffect
    {
        public ForthEffect()
        {
            effect = BotsEffectVoid;
            name = "Precision";
            potionPriority = 4;
            sideEffect = SideEffect.Necrotic;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Sage());
            receipeList.Add(new Herb.Chamomile());
            receipeList.Add(new Herb.Lavender());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            PotionLibrary.OrderPostsUpgradeManager.PrecisionPotionBuff();
            Debug.Log("Precision");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("Precision");
        }
    }

    public class FifthEffect : PotionEffect
    {
        public FifthEffect()
        {
            effect = BotsEffectVoid;
            name = "FifthEffect";
            potionPriority = 5;
            sideEffect = SideEffect.Necrotic;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Patchouli());
            receipeList.Add(new Herb.Patchouli());
            receipeList.Add(new Herb.Patchouli());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("FifthEffect");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("FifthEffect");
        }
    }


    public class SixthEffect : PotionEffect
    {
        public SixthEffect()
        {
            effect = BotsEffectVoid;
            name = "SixthEffect";
            potionPriority = 6;
            sideEffect = SideEffect.Necrotic;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Hellebore());
            receipeList.Add(new Herb.Hellebore());
            receipeList.Add(new Herb.Hellebore());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("SixthEffect");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("SixthEffect");
        }
    }


    public class SeventhEffect : PotionEffect
    {
        public SeventhEffect()
        {
            effect = BotsEffectVoid;
            name = "SeventhEffect";
            potionPriority = 7;
            sideEffect = SideEffect.Necrotic;
            isBase = false;
            AddHerbToReceipeList();
            LoadIcon();
        }

        public override void AddHerbToReceipeList()
        {
            receipeList.Add(new Herb.Lavender());
            receipeList.Add(new Herb.Sage());
            receipeList.Add(new Herb.Patchouli());
        }

        public override void AddRecepieToCombination()
        {
            PotionLibrary.AddCombination(receipeList, this);
        }

        public override void BotsEffectVoid()
        {
            Debug.Log("SeventhEffect");
        }

        public override void PlayerEffectVoid()
        {
            Debug.Log("SeventhEffect");
        }
    }
}





//Chamomile + Chamomile + Sage
//Chamomile + Chamomile + Patchouli
//Chamomile + Chamomile + Hellebore
//Chamomile + Lavender + Sage
//Chamomile + Lavender + Patchouli
//Chamomile + Lavender + Hellebore
//Chamomile + Sage + Sage
//Chamomile + Sage + Patchouli
//Chamomile + Sage + Hellebore
//Chamomile + Patchouli + Patchouli
//Chamomile + Patchouli + Hellebore
//Chamomile + Hellebore + Hellebore
//Lavender + Lavender + Sage
//Lavender + Lavender + Patchouli
//Lavender + Lavender + Hellebore
//Lavender + Sage + Hellebore
//Lavender + Patchouli + Patchouli
//Lavender + Patchouli + Hellebore
//Lavender + Hellebore + Hellebore
//Sage + Sage + Patchouli
//Sage + Sage + Hellebore
//Sage + Patchouli + Patchouli
//Sage + Patchouli + Hellebore
//Sage + Hellebore + Hellebore
//Patchouli + Patchouli + Hellebore
//Patchouli + Hellebore + Hellebore