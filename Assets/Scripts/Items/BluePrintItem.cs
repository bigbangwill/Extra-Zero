using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BluePrintItem : ItemBehaviour
{
    private int importTimer;

    private int craftTimer;

    // For the required materials to create the actual item.
    public List<MaterialItem> materialsList = new();
 
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

    //public class WalkingStick : BluePrintItem
    //{
    //    public WalkingStick()
    //    {
    //        specificName = "WalkingStick";
    //        importTimer = 3;
    //        craftTimer = 20;
    //        materialsList.Add(new MaterialItem.StainlessSteel(3));
    //        materialsList.Add(new MaterialItem.Plastic(1));
    //        Load();
    //    }
    //    public override CraftedItem CraftedItemReference()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Hoe : BluePrintItem
    //{
    //    public Hoe()
    //    {
    //        specificName = "Hoe";
    //        importTimer = 10;
    //        craftTimer = 10;
    //        materialsList.Add(new MaterialItem.StainlessSteel(3));
    //        materialsList.Add(new MaterialItem.AluminumAlloy(1));
    //        materialsList.Add(new MaterialItem.TitaniumAlloy(1));
    //        Load();
    //    }

    //    public override CraftedItem CraftedItemReference()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    
    //public class Gun : BluePrintItem
    //{
    //    public Gun()
    //    {
    //        specificName = "Gun";
    //        importTimer = 20;
    //        craftTimer = 10;
    //        materialsList.Add(new MaterialItem.AluminumAlloy(5));
    //        materialsList.Add(new MaterialItem.TitaniumAlloy(7));
    //        materialsList.Add(new MaterialItem.Ceramic(2));
    //        Load();
    //    }

    //    public override CraftedItem CraftedItemReference()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class Plant : BluePrintItem
    //{
    //    public Plant()
    //    {
    //        specificName = "Plant";
    //        importTimer = 15;
    //        craftTimer = 10;
    //        materialsList.Add(new MaterialItem.Ceramic(3));
    //        Load();
    //    }

    //    public override CraftedItem CraftedItemReference()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

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
