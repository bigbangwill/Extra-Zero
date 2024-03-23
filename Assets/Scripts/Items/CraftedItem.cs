using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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

    public abstract BluePrintItem GetCraftingRecipe();


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
            specificName = "Data Pad";
            Load();
            is_Activeable = false;
            is_Usable = false;
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {            
            return new BluePrintItem.DataPadBlueprint();
        }
    }

    public class SignalBooster : CraftedItem
    {
        public SignalBooster()
        {
            specificName = "Signal Booster";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.SignalBoosterBlueprint();
        }
    }

    public class EnergyCell : CraftedItem
    {
        public EnergyCell()
        {
            specificName = "Energy Cell";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.EnergyCellBlueprint();
        }
    }

    public class SurvivalKit : CraftedItem
    {
        public SurvivalKit()
        {
            specificName = "Survival Kit";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.SurvivalKitBlueprint();
        }

    }

    public class RepairDrone : CraftedItem
    {
        public RepairDrone()
        {
            specificName = "Repair Drone";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.RepairDroneBlueprint();
        }
    }

    public class WaterPurifier : CraftedItem
    {
        public WaterPurifier()
        {
            specificName = "Water Purifier";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.WaterPurifierBlueprint();
        }
    }

    public class SolarCharger : CraftedItem
    {
        public SolarCharger()
        {
            specificName = "Solar Charger";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.SolarChargerBlueprint();
        }

    }

    public class ThermalBlanket : CraftedItem
    {
        public ThermalBlanket()
        {
            specificName = "Thermal Blanket";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.ThermalBlanketBlueprint();
        }

    }

    public class RadiationDetector : CraftedItem
    {
        public RadiationDetector()
        {
            specificName = "Radiation Detector";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.RadiationDetectorBlueprint();
        }

    }

    public class CommunicationRelay : CraftedItem
    {
        public CommunicationRelay()
        {
            specificName = "Communication Relay";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.CommunicationRelayBlueprint();
        }

    }


    public class HeatShield : CraftedItem
    {
        public HeatShield()
        {
            specificName = "Heat Shield";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.HeatShieldBlueprint();
        }
    }

    public class MemoryCore : CraftedItem
    {
        public MemoryCore()
        {
            specificName = "Memory Core";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.MemoryCoreBlueprint();
        }

    }

    public class PowerConduit : CraftedItem
    {
        public PowerConduit()
        {
            specificName = "Power Conduit";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.PowerConduitBlueprint();
        }
    }

    public class CircuitFrame : CraftedItem
    {
        public CircuitFrame()
        {
            specificName = "Circuit Frame";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.CircuitFrameBlueprint();
        }
    }

    public class InsulationFoam : CraftedItem
    {
        public InsulationFoam()
        {
            specificName = "Insulation Foam";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.InsulationFoamBlueprint();
        }
    }


    public class AICore : CraftedItem
    {
        public AICore()
        {
            specificName = "AI Core";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.AICoreBlueprint();
        }
    }

    public class EnvironmentalRegulator : CraftedItem
    {
        public EnvironmentalRegulator()
        {
            specificName = "Environmental Regulator";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.EnvironmentalRegulatorBlueprint();
        }
    }

    public class HapticInterface : CraftedItem
    {
        public HapticInterface()
        {
            specificName = "Haptic Interface";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }

        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.HapticInterfaceBlueprint();
        }
    }

    public class QuantumProcessor : CraftedItem
    {
        public QuantumProcessor()
        {
            specificName = "Quantum Processor";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.QuantumProcessorBlueprint();
        }
    }

    public class BiometricScanner : CraftedItem
    {
        public BiometricScanner()
        {
            specificName = "Biometric Scanner";
            is_Activeable = false;
            is_Usable = false;
            Load();
        }

        public override void Use()
        {
            throw new NotImplementedException();
        }
        public override BluePrintItem GetCraftingRecipe()
        {
            return new BluePrintItem.BiometricScannerBlueprint();
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


        // NEED A FIX FOR THIS ASAP
        public override BluePrintItem GetCraftingRecipe()
        {
            return null;
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
