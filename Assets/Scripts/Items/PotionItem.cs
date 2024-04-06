using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PotionItem : ItemBehaviour
{

    private bool isDrinkable = false;

    public PotionEffect firstEffect;
    public PotionEffect secondEffect;
    public PotionEffect thirdEffect;
    public PotionEffect fourthEffect;

    public string GetSpecificName()
    {
        return specificName;
    }

    public bool IsDrinkable { get => isDrinkable; }

    protected override void LoadIcon()
    {
        string address = "Potion Effect/" + firstEffect.name;
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(address);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            itemIcon = handle.Result;
            //Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load the asset");
        }
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
        if (firstEffect.Equals(new PotionEffect.DrinkingPotionEffect()))
        {
            isDrinkable = true;
            isConsumable = true;
        }
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
        if (IsDrinkable)
        {
            firstEffect?.PlayerEffectVoid();
            secondEffect?.PlayerEffectVoid();
            thirdEffect?.PlayerEffectVoid();
            fourthEffect?.PlayerEffectVoid();
        }
        else
        {
            firstEffect?.BotsEffectVoid();
            secondEffect?.BotsEffectVoid();
            thirdEffect?.BotsEffectVoid();
            fourthEffect?.BotsEffectVoid();
        }
    }

    public override bool Equals(object obj)
    {
        return obj is PotionItem item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               specificName == item.specificName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(is_Usable, itemType, is_Stackable, maxStack, specificName);
    }
}