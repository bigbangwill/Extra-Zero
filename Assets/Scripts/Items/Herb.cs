using System;
using UnityEngine;

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

    public class Patchouli : Herb
    {
        public Patchouli()
        {
            maxStack = 50;
            specificName = "Patchouli";
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
            Load();
        }
    }

    public class Hellebore : Herb 
    {
        public Hellebore()
        {
            maxStack = 50;
            specificName = "Hellebore";
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
            Load();
        }    
    }
}