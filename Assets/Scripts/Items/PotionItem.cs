using System;
using UnityEngine;

public class PotionItem : ItemBehaviour
{

    public PotionEffect firstEffect;
    public PotionEffect secondEffect;
    public PotionEffect thirdEffect;
    public PotionEffect fourthEffect;

    public string GetSpecificName()
    {
        return specificName;
    }


    public PotionItem(PotionEffect first)
    {
        firstEffect = first;
        secondEffect = null;
        thirdEffect = null;
        fourthEffect = null;
        specificName = first.name + " Potion";
        Load();
    }
    public PotionItem(PotionEffect first, PotionEffect second)
    {
        firstEffect = first;
        secondEffect = second;
        thirdEffect = null;
        fourthEffect = null;
        specificName = first.name + " & " + second.name + " Potion";
        Load();
    }
    public PotionItem(PotionEffect first, PotionEffect second, PotionEffect third)
    {
        firstEffect = first;
        secondEffect = second;
        thirdEffect = third;
        fourthEffect = null;
        specificName = first.name + " & " + second.name + " & " + third.name + " Potion";
        Load();
    }
    /// <summary>
    /// Might need to remove this.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="third"></param>
    /// <param name="fourth"></param>
    public PotionItem(PotionEffect first, PotionEffect second, PotionEffect third, PotionEffect fourth)
    {
        firstEffect = first;
        secondEffect = second;
        thirdEffect = third;
        fourthEffect = fourth;
        specificName = first.name + " & " + second.name + " & " + third.name + " & " + fourth.name +  " Potion";
        Load();
    }

    public void SetNextEffect(PotionEffect effect)
    {
        if (secondEffect == null)
        {
            secondEffect = effect;
            specificName = firstEffect.name + " & " + secondEffect.name + " Potion";
        }
        else if (thirdEffect == null)
        {
            thirdEffect = effect;
            specificName = firstEffect.name + " & " + secondEffect.name + " & " + thirdEffect.name + " Potion";
        }
        else if (fourthEffect == null)
        {
            fourthEffect = effect;
            specificName = firstEffect.name + " & " + secondEffect.name + " & " +
                thirdEffect.name + " & " + fourthEffect.name + " Potion";
        }
        else
            Debug.LogWarning("Check here ASAP");
        Debug.Log(specificName);
    }

    public override void Load()
    {
        is_Usable = true;
        is_Stackable = true;
        useDelegate = Use;
        maxStack = 5;
        itemType = ItemType.potion;
        itemName = "Potion";
        currentStack = 1;
        specificAddress = "Potion Effect/InventoryPotion.jpg[Sprite]";
        LoadIcon();
    }



    public override void Use()
    {
        firstEffect?.effect();
        secondEffect?.effect();
        thirdEffect?.effect();
        fourthEffect?.effect();
    }

    public override bool Equals(object obj)
    {
        return obj is PotionItem item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               maxStack == item.maxStack &&
               specificName == item.specificName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(is_Usable, itemType, is_Stackable, maxStack, specificName);
    }
}