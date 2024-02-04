using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UIElements;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public delegate void UseDelegate();

public enum ItemType {potion,bluePrint,material,questItem,empty,seed,herb,craftedItem};

public abstract class ItemBehaviour : IComparable<ItemBehaviour>
{
    // All of these items will be given the value in their child classes.

    protected bool is_Usable;
    // Used to call the related use method of the item
    protected UseDelegate useDelegate;
    protected ItemType itemType;
    protected bool is_Stackable;
    protected int maxStack, currentStack;
    protected string itemName;
    protected Sprite itemIcon;
    protected Sprite itemSprite;

    protected bool is_Activeable = false;


    // For loading the icon
    protected string specificName;
    protected string specificAddress;
    protected AsyncOperationHandle<Sprite> AsyncHandle;



    // Used on the inventory system when it's called by the player
    public abstract void Use();

    // Used on creating items with activator.createinstance to load the needed stuff.
    public abstract void Load();

    public virtual void OnActive(){ }

    public virtual void OnDeactive(){ }

    /// <summary>
    /// Used localy to load the needed icon
    /// </summary>
    protected void LoadIcon()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(specificAddress);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            itemIcon = handle.Result;
            Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load the asset");
        }
    }

    public int CompareTo(ItemBehaviour other)
    {
        if(other == null) return 1;
        return string.Compare(specificName, other.specificName, StringComparison.Ordinal);
    }

    /// <summary>
    /// Used to return the related Icon for inventory UI
    /// </summary>
    /// <returns></returns>
    public Sprite IconRefrence()
    {
        return itemIcon;
    }
    /// <summary>
    /// To return the name of the item
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return specificName;
    }
    /// <summary>
    /// To check if the item is stackable
    /// </summary>
    /// <returns></returns>
    public bool IsStackable()
    {
        return is_Stackable;
    }
    /// <summary>
    /// To return the number of the current stack of the item
    /// </summary>
    /// <returns></returns>
    public int CurrentStack()
    {
        if (is_Stackable)
            return currentStack;
        else return 0;
    }
    /// <summary>
    /// To return the max stack value
    /// </summary>
    /// <returns></returns>
    public int MaxStack()
    {
        if (is_Stackable)
            return maxStack;
        else return 0;
    }
    /// <summary>
    /// To set new current stack value
    /// </summary>
    /// <param name="current"></param>
    public void SetCurrentStack(int current)
    {
        if (!is_Stackable)
            return;
        currentStack = current;
    }

    /// <summary>
    /// To return the useable value
    /// </summary>
    /// <returns></returns>
    public bool IsUseable()
    {
        return is_Usable;
    }

    /// <summary>
    /// To return the item type only
    /// </summary>
    /// <returns></returns>
    public ItemType ItemTypeValue()
    {
        return itemType;
    }

    public bool IsActiveable()
    {
        return is_Activeable;
    }
}

public class EmptyItem : ItemBehaviour
{
    public EmptyItem()
    {
        Load();
    }

    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.empty;
        is_Stackable = false;
        itemName = "Empty";
        specificName = "Empty";
        specificAddress = "Empty[Sprite]";
        LoadIcon();
    }

    public override bool Equals(object obj)
    {
        return obj is EmptyItem item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               itemName == item.itemName &&
               specificName == item.specificName &&
               specificAddress == item.specificAddress;
    }

    public override int GetHashCode()
    {
        Debug.Log("ORIGINAL");
        return HashCode.Combine(
            is_Usable,
            itemType,
            is_Stackable,
            itemName,
            specificName,
            specificAddress);
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use an Empty item");
    }
}
public abstract class BluePrintItem : ItemBehaviour
{
    private int importTimer;

    private int craftTimer;

    // For the required materials to create the actual item.
    public List<ItemBehaviour> materialsList = new();
 
    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.bluePrint;
        is_Stackable = false;
        itemName = "BluePrint";
        specificAddress = "BluePrints/" + specificName + "[Sprite]";
        LoadIcon();
    }

    public int ImportTimer()
    {
        return importTimer;
    }

    public int CraftTimer()
    {
        return craftTimer;
    }

    public override bool Equals(object obj)
    {
        return obj is BluePrintItem item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               itemName == item.itemName &&
               specificName == item.specificName &&
               specificAddress == item.specificAddress;
    }

    public override int GetHashCode()
    {
        Debug.Log("ORIGINAL");
        return HashCode.Combine(
            is_Usable,
            itemType,
            is_Stackable,
            itemName,
            specificName,
            specificAddress);
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use a blueprint");
    }

    public class WalkingStick : BluePrintItem
    {
        public WalkingStick()
        {
            specificName = "WalkingStick";
            importTimer = 3;
            craftTimer = 20;
            materialsList.Add(new MaterialItem.StainlessSteel(3));
            materialsList.Add(new MaterialItem.Plastic(1));
            Load();
        }
    }

    public class Hoe : BluePrintItem
    {
        public Hoe()
        {
            specificName = "Hoe";
            importTimer = 10;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(3));
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            materialsList.Add(new MaterialItem.TitaniumAlloy(1));
            Load();
        }
    }

    
    public class Gun : BluePrintItem
    {
        public Gun()
        {
            specificName = "Gun";
            importTimer = 20;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.AluminumAlloy(5));
            materialsList.Add(new MaterialItem.TitaniumAlloy(7));
            materialsList.Add(new MaterialItem.Ceramic(2));
            Load();
        }
    }

    public class Plant : BluePrintItem
    {
        public Plant()
        {
            specificName = "Plant";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.Ceramic(3));
            Load();
        }
        
    }
}



public abstract class CraftedItem : ItemBehaviour
{
    protected GameObject uiPrefab;
    protected string uiPrefabString;


    public override void Load()
    {
        is_Usable = true;
        useDelegate = Use;
        itemType = ItemType.craftedItem;
        is_Stackable = false;
        itemName = "CraftedItem";
        specificAddress = "CraftedItem/" + specificName + "[Sprite]";
        uiPrefabString = "UIPrefabGO/" + specificName;
        LoadIcon();
    }

    public class RepairHammer : CraftedItem
    {
        private Transform canvasRefrence;
        private GameObject uiElementRefrence;

        // this is for activator create Instance 
        public RepairHammer()
        {
            specificName = "RepairHammer";
            Load();
        }

        public RepairHammer(Transform canvas)
        {
            canvasRefrence = canvas;
            specificName = "RepairHammer";
            Load();
            LoadAddressable();
            OnCreate();
            is_Activeable = true;
        }

        private void LoadAddressable()
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(uiPrefabString);
            handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                uiPrefab = handle.Result;
                Addressables.Release(handle);
            }
            else
            {
                Debug.LogError("Failed to load the asset");
            }
        }


        private void OnCreate()
        {
            uiElementRefrence = UnityEngine.Object.Instantiate(uiPrefab,canvasRefrence);
        }

        public override void OnActive()
        {
            uiElementRefrence.SetActive(true);
        }

        public override void OnDeactive()
        {
            uiElementRefrence?.SetActive(false);
        }

        public override void Use()
        {

        }
    }
}

public abstract class MaterialItem : ItemBehaviour
{
    // Gets called from the child class to set every related infomation and load icon
    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.material;
        specificAddress = "Materials/" + specificName + "[Sprite]";
        is_Stackable = true;
        itemName = "Materials";
        LoadIcon();
    }

    public override bool Equals(object obj)
    {
        return obj is MaterialItem item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               itemName == item.itemName &&
               specificName == item.specificName &&
               specificAddress == item.specificAddress;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            is_Usable,
            itemType,
            is_Stackable,
            itemName,
            specificName,
            specificAddress);
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use a Material");
    }

    public class Plastic : MaterialItem
    {
        public Plastic()
        {
            maxStack = 10;
            specificName = "Plastic";
            Load();
        }

        public Plastic(int count)
        {
            maxStack = 10;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Plastic";
            currentStack = count;
            Load();
        }
    }

    public class Ceramic : MaterialItem
    {
        public Ceramic()
        {
            maxStack = 10;
            specificName = "Ceramic";
            Load();
        }
        public Ceramic(int count)
        {
            maxStack = 10;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Ceramic";
            currentStack = count;
            Load();
        }

    }

    public class TitaniumAlloy : MaterialItem
    {
        public TitaniumAlloy()
        {
            maxStack = 10;
            specificName = "Titanium";
            Load();
        }

        public TitaniumAlloy(int count)
        {
            maxStack = 10;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Titanium";
            currentStack = count;
            Load();
        }

    }

    public class AluminumAlloy : MaterialItem
    {

        public AluminumAlloy()
        {
            maxStack = 10;
            specificName = "Aluminum";
            Load();
        }

        public AluminumAlloy(int count)
        {
            maxStack = 10;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Aluminum";
            currentStack = count;
            Load();
        }

    }

    public class StainlessSteel : MaterialItem
    {
        public StainlessSteel()
        {
            maxStack = 10;
            specificName = "Stainless Steel";
            Load();
        }

        public StainlessSteel(int count)
        {
            maxStack = 10;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Stainless Steel";
            currentStack = count;
            Load();
        }
    }
}


public abstract class Seed : ItemBehaviour
{

    private int harvestAmount;
    private float maxHarvestTime;

    // Gets called from the child class to set every related infomation and load icon
    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.seed;
        specificAddress = "Seeds/" + specificName + "[Sprite]";
        is_Stackable = true;
        itemName = "Seeds";
        LoadIcon();
    }

    public abstract Herb Harvest();

    public float GetMaxHarvestTimer()
    {
        return maxHarvestTime;
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use a Material");
    }
    public void SetHarvestAmount(int amount)
    {
        harvestAmount = amount;
    }

    public override bool Equals(object obj)
    {
        return obj is Seed item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               itemName == item.itemName &&
               specificName == item.specificName &&
               specificAddress == item.specificAddress &&
               harvestAmount == item.harvestAmount;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            is_Usable,
            itemType,
            is_Stackable,
            itemName,
            specificName,
            specificAddress,
            harvestAmount);
    }

    public class Chamomile : Seed
    {

        public Chamomile()
        {
            maxStack = 100;
            specificName = "Chamomile";
            harvestAmount = 3;
            maxHarvestTime = 5;
            Load();
        }

        public Chamomile(int count)
        {
            maxStack = 100;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Chamomile";
            currentStack = count;
            harvestAmount = 3;
            maxHarvestTime = 5;
            Load();
        }

        public override Herb Harvest()
        {
            return new Herb.Chamomile(harvestAmount);
        }
    }

    public class Lavender : Seed
    {
        public Lavender()
        {
            maxStack = 100;
            specificName = "Lavender";
            harvestAmount = 3;
            maxHarvestTime = 5;
            Load();
        }

        public Lavender(int count)
        {
            maxStack = 100;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Lavender";
            currentStack = count;
            harvestAmount = 3;
            maxHarvestTime = 5;
            Load();
        }
        public override Herb Harvest()
        {
            return new Herb.Lavender(harvestAmount);
        }

    }

    public class Sage : Seed
    {

        public Sage()
        {
            maxStack = 100;
            specificName = "Sage";
            harvestAmount = 3;
            maxHarvestTime = 5;
            Load();
        }

        public Sage(int count)
        {
            maxStack = 100;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Sage";
            currentStack = count;
            harvestAmount = 3;
            maxHarvestTime = 5;
            Load();
        }

        public override Herb Harvest()
        {
            return new Herb.Sage(harvestAmount);
        }

    }
}

public abstract class Herb : ItemBehaviour
{
    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.herb            ;
        specificAddress = "Herbs/" + specificName + "[Sprite]";
        is_Stackable = true;
        itemName = "Herbs";
        LoadIcon();
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use a Material");
    }

    public override bool Equals(object obj)
    {
        return obj is Herb item &&
               is_Usable == item.is_Usable &&
               itemType == item.itemType &&
               is_Stackable == item.is_Stackable &&
               itemName == item.itemName &&
               specificName == item.specificName &&
               specificAddress == item.specificAddress;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            is_Usable,
            itemType,
            is_Stackable,
            itemName,
            specificName,
            specificAddress);
    }

    public class Chamomile : Herb
    {
        public Chamomile()
        {
            maxStack = 50;
            specificName = "Chamomile";
            Load();
        }

        public Chamomile(int count)
        {
            maxStack = 50;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Chamomile";
            currentStack = count;
            Load();
        }
    }

    public class Lavender : Herb
    {
        public Lavender()
        {
            maxStack = 50;
            specificName = "Lavender";
            Load();
        }

        public Lavender(int count)
        {
            maxStack = 50;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Lavender";
            currentStack = count;
            Load();
        }

    }

    public class Sage : Herb
    {
        public Sage()
        {
            maxStack = 50;
            specificName = "Sage";
            Load();
        }

        public Sage(int count)
        {
            maxStack = 50;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Sage";
            currentStack = count;
            Load();
        }
    }


}


public class PotionItem : ItemBehaviour
{

    public PotionEffect firstEffect;
    public PotionEffect secondEffect;
    public PotionEffect thirdEffect;
    public PotionEffect fourthEffect;


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