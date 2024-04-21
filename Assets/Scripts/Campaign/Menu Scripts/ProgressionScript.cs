using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionScript : MonoBehaviour, ISaveable
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

    private SaveClassManager saveClassManager;

    private void Start()
    {
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
        SetGameModeState();
        AddISaveableToDictionary();
    }

    public void SetGameModeState()
    {
        GameModeState.ScannerIsUnlocked = scanner;
        GameModeState.ComputerIsUnlocked = computer;
        GameModeState.PrinterIsUnlocked = printer;
        GameModeState.HerbalismPostIsUnlocked = herbalismPost;
        GameModeState.AlchemyPostIsUnlocked = alchemyPost;
        GameModeState.WaveSelectorIsUnlocked = waveSelector;
        GameModeState.QuantumStationIsUnlocked = quantumStation;
        GameModeState.MaterialFarmIsUnlocked = materialFarm;
        GameModeState.SeedFarmIsUnlocked  = seedFarm;
        GameModeState.TierStationIsUnlocked = tierStation;
        GameModeState.ShopStationIsUnlocked = shopStation;
        GameModeState.LavaBucketIsUnlocked = lavaBucket;
        GameModeState.ItemStashIsUnlocked = itemStash;

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
        scanner = data.scanner;
        computer = data.computer;
        printer = data.printer;
        herbalismPost = data.herbalismPost;
        alchemyPost = data.alchemyPost;
        waveSelector = data.waveSelector;
        quantumStation = data.quantumStation;
        materialFarm = data.materialFarm;
        seedFarm = data.seedFarm;
        tierStation = data.tierStation;
        shopStation = data.shopStation;
        lavaBucket = data.lavaBucket;
        itemStash = data.itemStash;


        SetGameModeState();
    }

    public string GetName()
    {
        return "Progression";
    }
}