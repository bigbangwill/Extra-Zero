using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class SaveClassesLibrary
{

    /// <summary>
    /// This method is for player inventory save system.
    /// </summary>
    [Serializable]
    public class PlayerInventory : SaveClassesLibrary
    {
        public int inventorySlotCount;
        public Dictionary<int, string> inventorySlotsSpecificNameDictionary = new();
        public int[] listCurrentStacks;

        public PlayerInventory(int slotCount, ItemBehaviour[] inventory)
        {
            inventorySlotCount = slotCount;

            listCurrentStacks = new int[slotCount];
            for (int i = 0; i < inventory.Length; i++)
            {
                inventorySlotsSpecificNameDictionary.Add(i, inventory[i].GetName());
                listCurrentStacks[i] = inventory[i].CurrentStack();
            }
        }
    }

    [Serializable]
    public class ScannerSlotUI : SaveClassesLibrary
    {
        public string holdingItemSpecificName;
        public ScannerSlotManager.SlotState state;
        public int slotNumber;

        public ScannerSlotUI(string specificName, ScannerSlotManager.SlotState currentState, int currentSlotNumber)
        {
            holdingItemSpecificName = specificName;
            state = currentState;
            slotNumber = currentSlotNumber;
        }
    }


    [Serializable]
    public class SlotReaderManager : SaveClassesLibrary
    {
        public List<ImportJob> savedJobsList;

        public SlotReaderManager(List<ImportJob> savedJobs)
        {
            this.savedJobsList = savedJobs;
        }
    }


    [Serializable]
    public class ItemPrinter : SaveClassesLibrary
    {
        public craftingItemState state1;
        public craftingItemState state2;
        public craftingItemState state3;

        public int currentCraftTimer;
        public int maxCraftTimer;
        public string savedBluePrintName;
        public bool isCrafting;

        public ItemPrinter(
            craftingItemState state1,
            craftingItemState state2,
            craftingItemState state3,
            string savedBluePrint,
            bool isCrafting,
            int currentCraftTimer,
            int maxCraftTimer)
        {
            this.state1 = state1;
            this.state2 = state2;
            this.state3 = state3;
            savedBluePrintName = savedBluePrint;
            this.isCrafting = isCrafting;
            this.currentCraftTimer = currentCraftTimer;
            this.maxCraftTimer = maxCraftTimer;
        }
    }

    [Serializable]
    public class CampaignInfoUI : SaveClassesLibrary
    {
        public List<string> doneNodes = new();
        public List<string> unlockedNodes = new();
        public CampaignInfoUI(List<string> doneNodes, List<string> unlockedNodes)
        {
            this.doneNodes = doneNodes;
            this.unlockedNodes = unlockedNodes;
        }
    }

    [Serializable]
    public class ProgressionScriptSave : SaveClassesLibrary
    {
        public bool scanner;
        public bool computer;
        public bool printer;
        public bool herbalismPost;
        public bool alchemyPost;
        public bool waveSelector;
        public bool quantumStation;
        public bool materialFarm;
        public bool seedFarm;
        public bool tierStation;
        public bool shopStation;
        public bool lavaBucket;
        public bool itemStash;
        public bool repairHammer;
        public bool recipeTablet;

        public ProgressionScriptSave(ProgressionScript data)
        {
            scanner = data.Scanner;
            computer = data.Computer;
            printer = data.Printer;
            herbalismPost = data.HerbalismPost;
            alchemyPost = data.AlchemyPost;
            waveSelector = data.WaveSelector;
            quantumStation = data.QuantumStation;
            materialFarm = data.MaterialFarm;
            seedFarm = data.SeedFarm;
            tierStation = data.TierStation;
            shopStation = data.ShopStation;
            lavaBucket = data.LavaBucket;
            itemStash = data.ItemStash;
            repairHammer = data.RepairHammer;
            recipeTablet = data.RecipeTablet;
        }
    }

    [Serializable]
    public class EconomyManagerSave : SaveClassesLibrary
    {
        public int outGameCurrent;
        public int outGameMax;
        public int campaignEnergyCurrent;
        public int campaignEnergyMax;

        public EconomyManagerSave(EconomyManager data)
        {
            outGameCurrent = data.OutGameCurrencyCurrentStack;
            outGameMax = data.OutGameCurrencyMaxStack;
            campaignEnergyCurrent = data.CampaignEnergyCurrentStack;
            campaignEnergyMax = data.CampaignEnergyMaxStack;
        }
    }

    [Serializable]
    public class TalentManagerSave : SaveClassesLibrary
    {
        public List<string> qubitList = new();
        public List<string> gateListKey = new();
        public List<string> gateListValue = new();
        public List<string> entangleListKey = new();
        public List<string> entangleListValue = new();



        public TalentManagerSave(TalentManager data)
        {
            //foreach (var talent in CreatedTalents.GetTalents())
            //{
            //    if (talent.IsQubit)
            //    {
            //        qubitList.Add(talent.GetSpecificName());
            //    }
            //    if(talent.IsGated)
            //    {
            //        gateListKey.Add(talent.GetSpecificName());
            //        gateListValue.Add(talent.GetRelatedNodePassive().talent.GetSpecificName());
            //    }
            //    else if (talent.IsEntangled)
            //    {
            //        entangleListKey.Add(talent.GetSpecificName());
            //        entangleListValue.Add(talent.GetRelatedNodePassive().talent.GetSpecificName());
            //    }
            //}

            foreach(var talent in data.GetNodePassives())
            {
                if (talent.IsQubit())
                {
                    qubitList.Add(talent.GetTalentName());
                }
                if (talent.IsGated())
                {
                    gateListKey.Add(talent.GetTalentName());
                    gateListValue.Add(talent.GetNodeToGate().GetTalentName());
                }
                if (talent.IsEntangled())
                {
                    entangleListKey.Add(talent.GetTalentName());
                    entangleListValue.Add(talent.GetNodeToEntangle().GetTalentName());
                }
            }


        }
    }

    [Serializable]
    public class OptionHolderSave : SaveClassesLibrary
    {
        public int qubitMax;
        public int qubitCurrent;    
        public int gateMax;
        public int gateCurrent;
        public int entangleMax;
        public int entangleCurrent;

        public OptionHolderSave(int qubitMax,int gateMax, int entangleMax, int qubitCurrent, int gateCurrent, int entangleCurrent)
        {
            this.qubitMax = qubitMax;
            this.gateMax = gateMax;
            this.entangleMax = entangleMax;
            this.qubitCurrent = qubitCurrent;
            this.gateCurrent = gateCurrent;
            this.entangleCurrent = entangleCurrent;
        }
    }
}