using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;

public class DeviceStateSetter : MonoBehaviour
{
    [SerializeField] private GameObject scanner;
    [SerializeField] private GameObject computer;
    [SerializeField] private GameObject printer;
    [SerializeField] private GameObject herbalismPost;
    [SerializeField] private GameObject alchemyPost;
    [SerializeField] private GameObject waveSelector;
    [SerializeField] private GameObject quantumStation;
    [SerializeField] private GameObject materialFarm;
    [SerializeField] private GameObject seedFarm;
    [SerializeField] private GameObject tierStation;
    [SerializeField] private GameObject shopStation;
    [SerializeField] private GameObject lavaBucket;
    [SerializeField] private GameObject itemStash;


    [SerializeField] private NavMeshSurface surface;




    private void Start()
    {
        SetCurrentState();

    }


    public void SetCurrentState()
    {
        scanner.GetComponent<Collider2D>().enabled = GameModeState.ScannerIsUnlocked;
        scanner.GetComponent<SpriteRenderer>().enabled = GameModeState.ScannerIsUnlocked;

        computer.GetComponent<Collider2D>().enabled = GameModeState.ComputerIsUnlocked;
        computer.GetComponent<SpriteRenderer>().enabled = GameModeState.ComputerIsUnlocked;

        printer.GetComponent<Collider2D>().enabled = GameModeState.PrinterIsUnlocked;
        printer.GetComponent<SpriteRenderer>().enabled = GameModeState.PrinterIsUnlocked;

        herbalismPost.GetComponent<Collider2D>().enabled = GameModeState.HerbalismPostIsUnlocked;
        herbalismPost.GetComponent<SpriteRenderer>().enabled = GameModeState.HerbalismPostIsUnlocked;

        alchemyPost.GetComponent<Collider2D>().enabled = GameModeState.AlchemyPostIsUnlocked;
        alchemyPost.GetComponent<SpriteRenderer>().enabled = GameModeState.AlchemyPostIsUnlocked;

        waveSelector.GetComponent<Collider2D>().enabled = GameModeState.WaveSelectorIsUnlocked;
        waveSelector.GetComponent<SpriteRenderer>().enabled = GameModeState.WaveSelectorIsUnlocked;

        quantumStation.GetComponent<Collider2D>().enabled = GameModeState.QuantumStationIsUnlocked;
        quantumStation.GetComponent<SpriteRenderer>().enabled = GameModeState.QuantumStationIsUnlocked;

        materialFarm.GetComponent<Collider2D>().enabled = GameModeState.MaterialFarmIsUnlocked;
        materialFarm.GetComponent<SpriteRenderer>().enabled = GameModeState.MaterialFarmIsUnlocked;

        seedFarm.GetComponent<Collider2D>().enabled = GameModeState.SeedFarmIsUnlocked;
        seedFarm.GetComponent<SpriteRenderer>().enabled = GameModeState.SeedFarmIsUnlocked;

        tierStation.GetComponent<Collider2D>().enabled = GameModeState.TierStationIsUnlocked;
        tierStation.GetComponent<SpriteRenderer>().enabled = GameModeState.TierStationIsUnlocked;

        shopStation.GetComponent<Collider2D>().enabled = GameModeState.ShopStationIsUnlocked;
        shopStation.GetComponent<SpriteRenderer>().enabled = GameModeState.ShopStationIsUnlocked;

        lavaBucket.GetComponent<Collider2D>().enabled = GameModeState.LavaBucketIsUnlocked;
        lavaBucket.GetComponent<SpriteRenderer>().enabled = GameModeState.LavaBucketIsUnlocked;

        itemStash.GetComponent<Collider2D>().enabled = GameModeState.ItemStashIsUnlocked;
        itemStash.GetComponent<SpriteRenderer>().enabled = GameModeState.ItemStashIsUnlocked;

        surface.BuildNavMesh();
    }
}
