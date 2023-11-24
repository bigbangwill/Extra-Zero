using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

public delegate void UseDelegate();

public enum ItemType {potion,bluePrint,material,questItem,empty };

public abstract class ItemBehaviour
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

    // For loading the icon
    protected string specificName;
    protected string specificAddress;
    protected AsyncOperationHandle<Sprite> AsyncHandle;


    // Used on the inventory system when it's called by the player
    public abstract void Use();

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
    /// To return the item type only
    /// </summary>
    /// <returns></returns>
    public ItemType ItemTypeValue()
    {
        return itemType;
    }
}

public class EmptyItem : ItemBehaviour
{
    public EmptyItem()
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
 
    public void Load()
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
public abstract class MaterialItem : ItemBehaviour
{
    // Gets called from the child class to set every related infomation and load icon
    public void Load()
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