using System;
using System.Diagnostics.Contracts;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public delegate void UseDelegate();

public enum ItemType {potion,bluePrint,material,questItem,empty,seed,herb,craftedItem};

public abstract class ItemBehaviour : IComparable<ItemBehaviour>, ICloneable
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
    protected int orderShowSlot;
    protected int slotUINumber;

    protected bool is_Activeable = false;

    protected bool isConsumable = false;


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
    protected virtual void LoadIcon()
    {
        try
        {

            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(specificAddress);
            handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                itemIcon = handle.Result;
            }
            else
            {
                Debug.LogError("Failed to load the asset");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
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

    public bool IsConsumable()
    {
        return isConsumable;
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
    public ItemType GetItemTypeValue()
    {
        return itemType;
    }

    public bool IsActiveable()
    {
        return is_Activeable;
    }

    public virtual void OnCreate()
    {
        Debug.Log("Cant Be Created");
    }

    public object Clone()
    {
        return MemberwiseClone();
    }


    public void SetOrderShowSlot(int i)
    {
        orderShowSlot = i;
    }

    public int GetOrderShowSlot()
    {
        return orderShowSlot;
    }

    public void SetItemSlotUI(int slot)
    {
        slotUINumber = slot;
    }

    public int GetItemSlotUI()
    {
        return slotUINumber;
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
