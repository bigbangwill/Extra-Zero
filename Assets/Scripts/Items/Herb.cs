using System;
using UnityEngine;

public abstract class Herb : ItemBehaviour
{
    private int herbTier;

    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.herb;
        specificAddress = "Herbs/" + specificName + "[Sprite]";
        is_Stackable = true;
        itemName = "Herbs";
        LoadIcon();
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use a Material");
    }

    public int GetHerbTier()
    {
        return herbTier;
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
            herbTier = 1;
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
            herbTier = 1;
            Load();
        }
    }

    public class Lavender : Herb
    {
        public Lavender()
        {
            maxStack = 50;
            specificName = "Lavender";
            herbTier = 1;
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
            herbTier = 1;
            Load();
        }

    }

    public class Sage : Herb
    {
        public Sage()
        {
            maxStack = 50;
            specificName = "Sage";
            herbTier = 2;
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
            herbTier = 2;
            Load();
        }
    }

    public class Patchouli : Herb
    {
        public Patchouli()
        {
            maxStack = 50;
            specificName = "Patchouli";
            herbTier = 3;
            Load();
        }

        public Patchouli(int count)
        {
            maxStack = 50;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Patchouli";
            currentStack = count;
            herbTier = 3;
            Load();
        }
    }

    public class Hellebore : Herb 
    {
        public Hellebore()
        {
            maxStack = 50;
            specificName = "Hellebore";
            herbTier = 4;
            Load();
        }
        public Hellebore(int count)
        {
            maxStack = 50;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Hellebore";
            currentStack = count;
            herbTier = 4;
            Load();
        }    
    }
}