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
    }

    public class SignalBooster : CraftedItem
    {
        public SignalBooster()
        {
            specificName = "Signal Booster";
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
            specificName = "Energy Cell";
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
            specificName = "Survival Kit";
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
            specificName = "Repair Drone";
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
            specificName = "Water Purifier";
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
            specificName = "Solar Charger";
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
            specificName = "Thermal Blanket";
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
            specificName = "Radiation Detector";
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
            specificName = "Communication Relay";
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
            specificName = "Heat Shield";
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
            specificName = "Memory Core";
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
            specificName = "Power Conduit";
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
            specificName = "Circuit Frame";
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
            specificName = "Insulation Foam";
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
            specificName = "AI Core";
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
            specificName = "Environmental Regulator";
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
            specificName = "Haptic Interface";
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
            specificName = "Quantum Processor";
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
            specificName = "Biometric Scanner";
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
