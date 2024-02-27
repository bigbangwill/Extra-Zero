using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public delegate void PotionEffectDelegate();

public static class PotionLibrary
{
    private static Dictionary<PotionCombination, PotionEffect> potionEffectDictionary = new();

    public static void Initialize()
    {
        potionEffectDictionary.Clear();
        AddCombination(new Herb.Sage(), new Herb.Sage(), new Herb.Sage(), new PotionEffect.FirstEffect());
        AddCombination(new Herb.Lavender(), new Herb.Lavender(), new Herb.Lavender(), new PotionEffect.SecondEffect());
        AddCombination(new Herb.Chamomile(), new Herb.Chamomile(), new Herb.Chamomile(), new PotionEffect.ThirdEffect());
        AddCombination(new Herb.Sage(), new Herb.Chamomile(), new Herb.Lavender(), new PotionEffect.ForthEffect());
    }

    // to add the related combination herbs and potion effect to the dictionary.
    private static void AddCombination(Herb first,Herb second,Herb third, PotionEffect potionEffectDelegate)
    {
        Herb[] herbsArray = new Herb[3];
        herbsArray[0] = first;
        herbsArray[1] = second;
        herbsArray[2] = third;
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
public class PotionEffect
{
    public enum SideEffect { Necrotic, Regenerative, Natural};
    public SideEffect sideEffect;
    public string name;
    public string specificAddress;
    public PotionEffectDelegate effect;
    public Sprite sprite;

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

    protected void LoadIcon()
    {
        specificAddress = "Potion Effect/" + name;
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(specificAddress);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            sprite = handle.Result;
            Addressables.Release(handle);
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
            effect = EffectVoid;
            name = "Empty";
            potionPriority = int.MaxValue;
            sideEffect = SideEffect.Natural;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("Empty");
        }

    }

    public class FirstEffect : PotionEffect
    {
        public FirstEffect()
        {
            effect = EffectVoid;
            name = "Speed";
            sideEffect = SideEffect.Regenerative;
            potionPriority = 0;
            LoadIcon();
        }

        public void EffectVoid()
        {
            Debug.Log("First Effect");
        }
    }

    public class SecondEffect : PotionEffect
    {
        public SecondEffect()
        {
            effect = EffectVoid;
            name = "Health";
            sideEffect = SideEffect.Regenerative;
            potionPriority = 1;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("Second Effect");
        }
    }
    public class ThirdEffect : PotionEffect
    {
        public ThirdEffect()
        {
            effect = EffectVoid;
            name = "Mana";
            sideEffect = SideEffect.Regenerative;
            potionPriority = 3;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("Third Effect");
        }
    }

    public class ForthEffect : PotionEffect
    {
        public ForthEffect()
        {
            effect = EffectVoid;
            name = "Random";
            potionPriority = 4;
            sideEffect = SideEffect.Necrotic;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("forth Effect");
        }
    }

    public class FifthEffect : PotionEffect
    {
        public FifthEffect()
        {
            effect = EffectVoid;
            name = "FifthEffect";
            potionPriority = 5;
            sideEffect = SideEffect.Necrotic;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("FifthEffect");
        }
    }


    public class SixthEffect : PotionEffect
    {
        public SixthEffect()
        {
            effect = EffectVoid;
            name = "SixthEffect";
            potionPriority = 6;
            sideEffect = SideEffect.Necrotic;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("SixthEffect");
        }
    }


    public class SeventhEffect : PotionEffect
    {
        public SeventhEffect()
        {
            effect = EffectVoid;
            name = "SeventhEffect";
            potionPriority = 7;
            sideEffect = SideEffect.Necrotic;
            LoadIcon();
        }
        public void EffectVoid()
        {
            Debug.Log("SeventhEffect");
        }
    }
}