using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionScript : MonoBehaviour, ISaveable
{
    [SerializeField] private bool scanner;
    [SerializeField] private bool computer;
    [SerializeField] private bool printer;
    [SerializeField] private bool herbalismPost;
    [SerializeField] private bool alchemyPost;
    [SerializeField] private bool waveSelector;
    [SerializeField] private bool quantumStation;
    [SerializeField] private bool materialFarm;
    [SerializeField] private bool seedFarm;
    [SerializeField] private bool tierStation;
    [SerializeField] private bool shopStation;
    [SerializeField] private bool lavaBucket;
    [SerializeField] private bool itemStash;
    [SerializeField] private bool repairHammer;
    [SerializeField] private bool recipeTablet;

    [SerializeField] private CampaignInfoUI infoUI;

    private SaveClassManager saveClassManager;

    public bool Scanner { get => scanner; set {
            scanner = value; 
            SetGameModeState(); 
        } }
    public bool Computer { get => computer; set {
            computer = value;
            SetGameModeState(); 
        } }
    public bool Printer { get => printer; set {
            printer = value;
            SetGameModeState();
        } }
    public bool HerbalismPost { get => herbalismPost; set {
            herbalismPost = value;
            SetGameModeState();
        } }
    public bool AlchemyPost { get => alchemyPost; set {
            alchemyPost = value;
            SetGameModeState();
        } }
    public bool WaveSelector { get => waveSelector; set {
            waveSelector = value;
            SetGameModeState();
        } }
    public bool QuantumStation { get => quantumStation; set {
            quantumStation = value;
            SetGameModeState();
        } }
    public bool MaterialFarm { get => materialFarm; set {
            materialFarm = value;
            SetGameModeState();
        } }
    public bool SeedFarm { get => seedFarm; set {
            seedFarm = value;
            SetGameModeState();
        } }
    public bool TierStation { get => tierStation; set {
            tierStation = value;
            SetGameModeState();
        } }
    public bool ShopStation { get => shopStation; set {
            shopStation = value;
            SetGameModeState();
        } }
    public bool LavaBucket { get => lavaBucket; set {
            lavaBucket = value;
            SetGameModeState();
        } }
    public bool ItemStash { get => itemStash; set {
            itemStash = value;
            SetGameModeState();
        } }

    public bool RepairHammer { get => repairHammer; set {
            repairHammer = value;
            SetGameModeState();
        } }

    public bool RecipeTablet { get => recipeTablet; set {
            recipeTablet = value;
            SetGameModeState();
        } }




    private void Start()
    {        
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
        SetGameModeState();
        AddISaveableToDictionary();

        Debug.LogWarning("Before hitting the thing");
        if (GameModeState.IsFinished)
        {
            saveClassManager.LoadSavedGame();
            Debug.LogWarning("Inside the thing " + GameModeState.CurrentCampaignNode);
            infoUI.GiveReward(GameModeState.CurrentCampaignNode);
        }
    }

    public void SetGameModeState()
    {
        GameModeState.ScannerIsUnlocked = Scanner;
        GameModeState.ComputerIsUnlocked = Computer;
        GameModeState.PrinterIsUnlocked = Printer;
        GameModeState.HerbalismPostIsUnlocked = HerbalismPost;
        GameModeState.AlchemyPostIsUnlocked = AlchemyPost;
        GameModeState.WaveSelectorIsUnlocked = WaveSelector;
        GameModeState.QuantumStationIsUnlocked = QuantumStation;
        GameModeState.MaterialFarmIsUnlocked = MaterialFarm;
        GameModeState.SeedFarmIsUnlocked  = SeedFarm;
        GameModeState.TierStationIsUnlocked = TierStation;
        GameModeState.ShopStationIsUnlocked = ShopStation;
        GameModeState.LavaBucketIsUnlocked = LavaBucket;
        GameModeState.ItemStashIsUnlocked = ItemStash;
        GameModeState.RepairHammerIsUnlocked = repairHammer;
        GameModeState.RecipeTabletIsUnlocked = recipeTablet;

        GameModeState.IsCampaignMode = true;

    }

    public void AddISaveableToDictionary()
    {
        saveClassManager.AddISaveableToDictionary(GetName(), this, 1);
    }

    public object Save()
    {
        SaveClassesLibrary.ProgressionScriptSave saveData = new(this);
        return saveData;
    }

    public void Load(object savedData)
    {
        SaveClassesLibrary.ProgressionScriptSave data = (SaveClassesLibrary.ProgressionScriptSave)savedData;
        Scanner = data.scanner;
        Computer = data.computer;
        Printer = data.printer;
        HerbalismPost = data.herbalismPost;
        AlchemyPost = data.alchemyPost;
        WaveSelector = data.waveSelector;
        QuantumStation = data.quantumStation;
        MaterialFarm = data.materialFarm;
        SeedFarm = data.seedFarm;
        TierStation = data.tierStation;
        ShopStation = data.shopStation;
        LavaBucket = data.lavaBucket;
        ItemStash = data.itemStash;
        RepairHammer = data.repairHammer;
        RecipeTablet = data.recipeTablet;

        SetGameModeState();
    }

    public string GetName()
    {
        return "Progression";
    }
}