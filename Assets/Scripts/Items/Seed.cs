using System;
using UnityEngine;

public abstract class Seed : ItemBehaviour
{

    private int harvestAmount;
    private float maxHarvestTime;

    private int seedTier;

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

    public int GetHarvestAmount()
    {
        return harvestAmount;
    }

    public int GetSeedTier()
    {
        return seedTier;
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
            seedTier = 1;
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
            seedTier = 1;
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
            seedTier = 1;
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
            seedTier = 1;
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
            seedTier = 2;
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
            seedTier = 2;
            Load();
        }

        public override Herb Harvest()
        {
            return new Herb.Sage(harvestAmount);
        }
    }

    public class Patchouli : Seed
    {
        public Patchouli()
        {
            maxStack = 100;
            specificName = "Patchouli";
            harvestAmount = 3;
            maxHarvestTime = 5;
            seedTier = 3;
            Load();
        }

        public Patchouli(int count)
        {
            maxStack = 100;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Patchouli";
            currentStack = count;
            harvestAmount = 3;
            maxHarvestTime = 5;
            seedTier = 3;
            Load();
        }

        public override Herb Harvest()
        {
            return new Herb.Patchouli(harvestAmount);
        }
    }

    public class Hellebore : Seed
    {
        public Hellebore()
        {
            maxStack = 100;
            specificName = "Hellebore";
            harvestAmount = 3;
            maxHarvestTime = 5;
            seedTier = 4;
            Load();
        }

        public Hellebore(int count)
        {
            maxStack = 100;
            if (count > maxStack)
            {
                count = maxStack;
                Debug.LogWarning("You are making it higher than the max stack. Setting the current stack to max stack");
            }
            specificName = "Hellebore";
            currentStack = count;
            harvestAmount = 3;
            maxHarvestTime = 5;
            seedTier = 4;
            Load();
        }
        public override Herb Harvest()
        {
            return new Herb.Hellebore(harvestAmount);
        }
    }
}
