using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionScript : MonoBehaviour
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
    }
}