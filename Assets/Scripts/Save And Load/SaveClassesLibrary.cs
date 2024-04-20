using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public CampaignInfoUI(List<string> doneNodes)
        {
            this.doneNodes = doneNodes;
        }
    }
}