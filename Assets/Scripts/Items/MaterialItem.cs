using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class MaterialItem : ItemBehaviour
{
    private int itemTier;
    protected string farmIconAddress;
    protected int maxThreshold;
    protected int currentThreshold;
    private Sprite farmIcon;


    protected void LoadFarmIcon()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(farmIconAddress);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            farmIcon = handle.Result;
            Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load the asset");
        }
    }

    // Gets called from the child class to set every related infomation and load icon
    public override void Load()
    {
        is_Usable = false;
        useDelegate = Use;
        itemType = ItemType.material;
        specificAddress = "Materials/" + specificName + "[Sprite]";
        is_Stackable = true;
        itemName = "Materials";
        farmIconAddress = "Farm Icons/" + specificName + "[Sprite]";
        SetItemTier();
        GetItemTier();
        LoadIcon();
        LoadFarmIcon();        
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

    public abstract void SetItemTier();

    public int GetItemTier()
    {
        return itemTier;
    }

    public override void Use()
    {
        Debug.LogWarning("You shouldnt see this but you are trying to use a Material");
    }

    public Sprite GetFarmIcon()
    {
        return farmIcon;
    }

    public int GetMaxThreshold()
    {
        return maxThreshold;
    }

    

    public class Plastic : MaterialItem
    {
        public Plastic()
        {
            maxStack = 10;
            specificName = "Plastic";
            maxThreshold = 3;
            
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
            maxThreshold = 3;
            
            Load();
        }

        public override void SetItemTier()
        {
            itemTier = 1;
        }
    }

    public class Ceramic : MaterialItem
    {
        public Ceramic()
        {
            maxStack = 10;
            specificName = "Ceramic";
            maxThreshold = 4;
            
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
            maxThreshold = 4;
            
            Load();
        }

        public override void SetItemTier()
        {
            itemTier = 1;
        }
    }

    public class TitaniumAlloy : MaterialItem
    {
        public TitaniumAlloy()
        {
            maxStack = 10;
            specificName = "Titanium";
            maxThreshold = 6;
            
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
            maxThreshold = 6;
            
            Load();
        }
        public override void SetItemTier()
        {
            itemTier = 2;
        }   
    }

    public class AluminumAlloy : MaterialItem
    {

        public AluminumAlloy()
        {
            maxStack = 10;
            specificName = "Aluminum";
            maxThreshold = 7;
            
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
            maxThreshold = 3;
            currentThreshold = 7;
            Load();
        }
        public override void SetItemTier()
        {
            itemTier = 3;
        }
    }

    public class StainlessSteel : MaterialItem
    {
        public StainlessSteel()
        {
            maxStack = 10;
            specificName = "Stainless Steel";
            maxThreshold = 8;
            
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
            maxThreshold = 8;
            
            Load();
        }
        public override void SetItemTier()
        {
            itemTier = 4;
        }
    }
}
