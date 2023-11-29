using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PotionEffectDelegate();

public static class PotionLibrary
{
    private static Dictionary<PotionCombination, PotionEffect> potionEffectDictionary;

    public static void Initialize()
    {
        AddCombination(new Herb.Sage(), new Herb.Sage(), new Herb.Sage(), new PotionEffect.FirstEffect());
        AddCombination(new Herb.Lavender(), new Herb.Lavender(), new Herb.Lavender(), new PotionEffect.SecondEffect());
        AddCombination(new Herb.Chamomile(), new Herb.Chamomile(), new Herb.Chamomile(), new PotionEffect.ThirdEffect());

    }

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
        if(potionEffectDictionary.ContainsKey(target))
            return potionEffectDictionary[target];
        else 
            return new PotionEffect.EmptyEffect();
    }
}


public class PotionCombination
{
    public string first, second, third;
    public PotionCombination(Herb first, Herb second, Herb third)
    {
        this.first = first.GetName();
        this.second = second.GetName();
        this.third = third.GetName();
    }
}

public class PotionEffect
{
    public string name;
    public PotionEffectDelegate effect;

    public class EmptyEffect : PotionEffect
    {
        public EmptyEffect()
        {
            effect = EffectVoid;
            name = "Empty";
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
            name = "health";
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
            name = "mana";
        }
        public void EffectVoid()
        {
            Debug.Log("Third Effect");
        }
    }
}