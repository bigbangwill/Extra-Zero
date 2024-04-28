using ExtraZero.Dialogue;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class GameModeState
{
    private static bool scannerIsUnlocked = false;
    private static bool computerIsUnlocked = false;
    private static bool printerIsUnlocked = false;
    private static bool herbalismPostIsUnlocked = false;
    private static bool alchemyPostIsUnlocked = false;
    private static bool waveSelectorIsUnlocked = false;
    private static bool quantumStationIsUnlocked = false;
    private static bool materialFarmIsUnlocked = false;
    private static bool seedFarmIsUnlocked = false;
    private static bool tierStationIsUnlocked = false;
    private static bool shopStationIsUnlocked = false;
    private static bool lavaBucketIsUnlocked = false;
    private static bool itemStashIsUnlocked = false;
    private static bool repairHammerIsUnlocked = false;
    private static bool recipeTabletIsUnlocked = false;
    private static bool menuShopIsUnlocked = false;


    private static bool isCampaignMode = false;
    private static string currentCampaignNode;

    private static bool isFinished = false;

    private static Dialogue currentDialogue = null;


    public static bool ScannerIsUnlocked { get => scannerIsUnlocked; set => scannerIsUnlocked = value; }
    public static bool ComputerIsUnlocked { get => computerIsUnlocked; set => computerIsUnlocked = value; }
    public static bool PrinterIsUnlocked { get => printerIsUnlocked; set => printerIsUnlocked = value; }
    public static bool HerbalismPostIsUnlocked { get => herbalismPostIsUnlocked; set => herbalismPostIsUnlocked = value; }
    public static bool AlchemyPostIsUnlocked { get => alchemyPostIsUnlocked; set => alchemyPostIsUnlocked = value; }
    public static bool WaveSelectorIsUnlocked { get => waveSelectorIsUnlocked; set => waveSelectorIsUnlocked = value; }
    public static bool QuantumStationIsUnlocked { get => quantumStationIsUnlocked; set => quantumStationIsUnlocked = value; }
    public static bool MaterialFarmIsUnlocked { get => materialFarmIsUnlocked; set => materialFarmIsUnlocked = value; }
    public static bool SeedFarmIsUnlocked { get => seedFarmIsUnlocked; set => seedFarmIsUnlocked = value; }
    public static bool TierStationIsUnlocked { get => tierStationIsUnlocked; set => tierStationIsUnlocked = value; }
    public static bool ShopStationIsUnlocked { get => shopStationIsUnlocked; set => shopStationIsUnlocked = value; }
    public static bool LavaBucketIsUnlocked { get => lavaBucketIsUnlocked; set => lavaBucketIsUnlocked = value; }
    public static bool ItemStashIsUnlocked { get => itemStashIsUnlocked; set => itemStashIsUnlocked = value; }
    public static bool RepairHammerIsUnlocked { get => repairHammerIsUnlocked; set => repairHammerIsUnlocked = value; }
    public static bool RecipeTabletIsUnlocked { get => recipeTabletIsUnlocked; set => recipeTabletIsUnlocked = value; }


    public static bool IsCampaignMode { get => isCampaignMode; set => isCampaignMode = value; }
    public static string CurrentCampaignNode { get => currentCampaignNode; set => currentCampaignNode = value; }
    public static bool IsFinished { get => isFinished; set => isFinished = value; }
    public static Dialogue CurrentDialogue { get => currentDialogue; set => currentDialogue = value; }
    public static bool MenuShopIsUnlocked { get => menuShopIsUnlocked; set => menuShopIsUnlocked = value; }
}