using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Schema;
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

    public virtual void OnCreate()
    {
        Debug.Log("Cant Be Created");
    }

    public object Clone()
    {
        return MemberwiseClone();
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


    public abstract CraftedItem CraftedItemReference();

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
        public override CraftedItem CraftedItemReference()
        {
            throw new NotImplementedException();
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

        public override CraftedItem CraftedItemReference()
        {
            throw new NotImplementedException();
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

        public override CraftedItem CraftedItemReference()
        {
            throw new NotImplementedException();
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

        public override CraftedItem CraftedItemReference()
        {
            throw new NotImplementedException();
        }
    }

    public class DataPadBlueprint : BluePrintItem
    {
        public DataPadBlueprint()
        {
            specificName = "Data Pad";
            importTimer = 15; // You may need to set appropriate values for these timers
            craftTimer = 10;
            materialsList.Add(new MaterialItem.TitaniumAlloy(2));
            materialsList.Add(new MaterialItem.Plastic(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.DataPad();
        }
    }

    public class SignalBoosterBlueprint : BluePrintItem
    {
        public SignalBoosterBlueprint()
        {
            specificName = "Signal Booster";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(3));
            materialsList.Add(new MaterialItem.Ceramic(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.SignalBooster();
        }
    }

    public class EnergyCellBlueprint : BluePrintItem
    {
        public EnergyCellBlueprint()
        {
            specificName = "Energy Cell";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.AluminumAlloy(2));
            materialsList.Add(new MaterialItem.TitaniumAlloy(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.EnergyCell();
        }
    }

    public class SurvivalKitBlueprint : BluePrintItem
    {
        public SurvivalKitBlueprint()
        {
            specificName = "Survival Kit";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(1));
            materialsList.Add(new MaterialItem.Plastic(2));
            materialsList.Add(new MaterialItem.Ceramic(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.SurvivalKit();
        }
    }

    public class RepairDroneBlueprint : BluePrintItem
    {
        public RepairDroneBlueprint()
        {
            specificName = "Repair Drone";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.TitaniumAlloy(3));
            materialsList.Add(new MaterialItem.Plastic(2));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.RepairDrone();
        }
    }

    public class WaterPurifierBlueprint : BluePrintItem
    {
        public WaterPurifierBlueprint()
        {
            specificName = "Water Purifier";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            materialsList.Add(new MaterialItem.Ceramic(2));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.WaterPurifier();
        }
    }

    public class SolarChargerBlueprint : BluePrintItem
    {
        public SolarChargerBlueprint()
        {
            specificName = "Solar Charger";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(2));
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.SolarCharger();
        }
    }

    public class ThermalBlanketBlueprint : BluePrintItem
    {
        public ThermalBlanketBlueprint()
        {
            specificName = "Thermal Blanket";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.TitaniumAlloy(1));
            materialsList.Add(new MaterialItem.Plastic(2));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.ThermalBlanket();
        }
    }

    public class RadiationDetectorBlueprint : BluePrintItem
    {
        public RadiationDetectorBlueprint()
        {
            specificName = "Radiation Detector";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.Ceramic(2));
            materialsList.Add(new MaterialItem.StainlessSteel(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.RadiationDetector();
        }
    }

    public class CommunicationRelayBlueprint : BluePrintItem
    {
        public CommunicationRelayBlueprint()
        {
            specificName = "Communication Relay";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            materialsList.Add(new MaterialItem.TitaniumAlloy(2));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.CommunicationRelay();
        }
    }

    public class HeatShieldBlueprint : BluePrintItem
    {
        public HeatShieldBlueprint()
        {
            specificName = "Heat Shield";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.TitaniumAlloy(3));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.HeatShield();
        }
    }

    public class MemoryCoreBlueprint : BluePrintItem
    {
        public MemoryCoreBlueprint()
        {
            specificName = "Memory Core";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.Ceramic(2));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.MemoryCore();
        }
    }

    public class PowerConduitBlueprint : BluePrintItem
    {
        public PowerConduitBlueprint()
        {
            specificName = "Power Conduit";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(3));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.PowerConduit();
        }
    }

    public class CircuitFrameBlueprint : BluePrintItem
    {
        public CircuitFrameBlueprint()
        {
            specificName = "Circuit Frame";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.AluminumAlloy(2));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.CircuitFrame();
        }
    }

    public class InsulationFoamBlueprint : BluePrintItem
    {
        public InsulationFoamBlueprint()
        {
            specificName = "Insulation Foam";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.Plastic(3));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.InsulationFoam();
        }
    }


    public class AICoreBlueprint : BluePrintItem
    {
        public AICoreBlueprint()
        {
            specificName = "AI Core";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.TitaniumAlloy(1));
            materialsList.Add(new MaterialItem.Ceramic(1));
            materialsList.Add(new MaterialItem.Plastic(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.AICore();
        }
    }

    public class EnvironmentalRegulatorBlueprint : BluePrintItem
    {
        public EnvironmentalRegulatorBlueprint()
        {
            specificName = "Environmental Regulator";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(1));
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            materialsList.Add(new MaterialItem.Ceramic(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.EnvironmentalRegulator();
        }
    }

    public class HapticInterfaceBlueprint : BluePrintItem
    {
        public HapticInterfaceBlueprint()
        {
            specificName = "Haptic Interface";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.TitaniumAlloy(1));
            materialsList.Add(new MaterialItem.StainlessSteel(1));
            materialsList.Add(new MaterialItem.Plastic(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.HapticInterface();
        }
    }

    public class QuantumProcessorBlueprint : BluePrintItem
    {
        public QuantumProcessorBlueprint()
        {
            specificName = "Quantum Processor";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            materialsList.Add(new MaterialItem.Ceramic(1));
            materialsList.Add(new MaterialItem.TitaniumAlloy(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.QuantumProcessor();
        }
    }

    public class BiometricScannerBlueprint : BluePrintItem
    {
        public BiometricScannerBlueprint()
        {
            specificName = "Biometric Scanner";
            importTimer = 15;
            craftTimer = 10;
            materialsList.Add(new MaterialItem.StainlessSteel(1));
            materialsList.Add(new MaterialItem.Plastic(1));
            materialsList.Add(new MaterialItem.AluminumAlloy(1));
            Load();
        }

        public override CraftedItem CraftedItemReference()
        {
            return new CraftedItem.BiometricScanner();
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

    public override bool Equals(object obj)
    {
        return obj is CraftedItem item &&
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

    public class DataPad : CraftedItem
    {
        public DataPad()
        {
            specificName = "DataPad";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class SignalBooster : CraftedItem
    {
        public SignalBooster()
        {
            specificName = "SignalBooster";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class EnergyCell : CraftedItem
    {
        public EnergyCell()
        {
            specificName = "EnergyCell";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class SurvivalKit : CraftedItem
    {
        public SurvivalKit()
        {
            specificName = "SurvivalKit";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class RepairDrone : CraftedItem
    {
        public RepairDrone()
        {
            specificName = "RepairDrone";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class WaterPurifier : CraftedItem
    {
        public WaterPurifier()
        {
            specificName = "WaterPurifier";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class SolarCharger : CraftedItem
    {
        public SolarCharger()
        {
            specificName = "SolarCharger";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class ThermalBlanket : CraftedItem
    {
        public ThermalBlanket()
        {
            specificName = "ThermalBlanket";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class RadiationDetector : CraftedItem
    {
        public RadiationDetector()
        {
            specificName = "RadiationDetector";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class CommunicationRelay : CraftedItem
    {
        public CommunicationRelay()
        {
            specificName = "CommunicationRelay";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }


    public class HeatShield : CraftedItem
    {
        public HeatShield()
        {
            specificName = "HeatShield";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class MemoryCore : CraftedItem
    {
        public MemoryCore()
        {
            specificName = "MemoryCore";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class PowerConduit : CraftedItem
    {
        public PowerConduit()
        {
            specificName = "PowerConduit";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class CircuitFrame : CraftedItem
    {
        public CircuitFrame()
        {
            specificName = "CircuitFrame";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class InsulationFoam : CraftedItem
    {
        public InsulationFoam()
        {
            specificName = "InsulationFoam";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }


    public class AICore : CraftedItem
    {
        public AICore()
        {
            specificName = "AICore";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class EnvironmentalRegulator : CraftedItem
    {
        public EnvironmentalRegulator()
        {
            specificName = "EnvironmentalRegulator";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class HapticInterface : CraftedItem
    {
        public HapticInterface()
        {
            specificName = "HapticInterface";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class QuantumProcessor : CraftedItem
    {
        public QuantumProcessor()
        {
            specificName = "QuantumProcessor";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
    }

    public class BiometricScanner : CraftedItem
    {
        public BiometricScanner()
        {
            specificName = "BiometricScanner";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
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
            //OnCreate();
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


        public override void OnCreate()
        {
            uiElementRefrence = UnityEngine.Object.Instantiate(uiPrefab,canvasRefrence);
            uiElementRefrence.SetActive(false);
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
    private int itemLevel;
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
        SetItemLevel();
        GetItemLevel();
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

    public abstract void SetItemLevel();

    public int GetItemLevel()
    {
        return itemLevel;
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

        public override void SetItemLevel()
        {
            itemLevel = 1;
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

        public override void SetItemLevel()
        {
            itemLevel = 1;
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
        public override void SetItemLevel()
        {
            itemLevel = 2;
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
        public override void SetItemLevel()
        {
            itemLevel = 2;
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
        public override void SetItemLevel()
        {
            itemLevel = 3;
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

    public int GetHarvestAmount()
    {
        return harvestAmount;
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