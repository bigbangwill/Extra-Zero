using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavMeshPlus.Components;
using UnityEngine.SceneManagement;
using static CampaignNodeScript;

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
    [SerializeField] private GameObject repairHammer;
    [SerializeField] private GameObject recipeTablet;

    [SerializeField] private NavMeshSurface surface;

    private CampaignRewardEnum currentMilestone;

    private DialogueManagerRefrence dialogueManagerRefrence;

    private void LoadSoRefrence()
    {
        dialogueManagerRefrence = (DialogueManagerRefrence)FindSORefrence<DialogueManager>.FindScriptableObject("Dialogue Manager Refrence");
    }

    private void Start()
    {
        LoadSoRefrence();
        SetCurrentState();
        AddCurrentMilestone();

        if (GameModeState.CurrentDialogue != null)
        {
            dialogueManagerRefrence.val.ShowDialogue(GameModeState.CurrentDialogue);
            dialogueManagerRefrence.val.SetMovingTarget(currentMilestone);
        }
        else
            dialogueManagerRefrence.val.FinishedDialogue();
    }


    private void SetCurrentState()
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

        repairHammer.GetComponent<Collider2D>().enabled = GameModeState.RepairHammerIsUnlocked;
        repairHammer.GetComponent<SpriteRenderer>().enabled = GameModeState.RepairHammerIsUnlocked;

        recipeTablet.GetComponent<Collider2D>().enabled = GameModeState.RecipeTabletIsUnlocked;
        recipeTablet.GetComponent<Collider2D>().enabled = GameModeState.RecipeTabletIsUnlocked;

        surface.BuildNavMesh();
    }

    private void AddCurrentMilestone()
    {
        currentMilestone = GameModeState.MilestoneReward;
        switch (currentMilestone)
        {
            case CampaignRewardEnum.none: break;
            case CampaignRewardEnum.scanner:
                scanner.GetComponent<Collider2D>().enabled = true;
                scanner.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.computer:
                computer.GetComponent<Collider2D>().enabled = true;
                computer.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.printer:
                printer.GetComponent<Collider2D>().enabled = true;
                printer.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.herbalismPost:
                herbalismPost.GetComponent<Collider2D>().enabled = true;
                herbalismPost.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.alchemyPost:
                alchemyPost.GetComponent<Collider2D>().enabled = true;
                alchemyPost.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.waveSelector:
                waveSelector.GetComponent<Collider2D>().enabled = true;
                waveSelector.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.quantumStation:
                quantumStation.GetComponent<Collider2D>().enabled = true;
                quantumStation.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.materialFarm:
                materialFarm.GetComponent<Collider2D>().enabled = true;
                materialFarm.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.seedFarm:
                seedFarm.GetComponent<Collider2D>().enabled = true;
                seedFarm.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.tierStation:
                tierStation.GetComponent<Collider2D>().enabled = true;
                tierStation.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.shopStation:
                shopStation.GetComponent<Collider2D>().enabled = true;
                shopStation.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.lavaBucket:
                lavaBucket.GetComponent<Collider2D>().enabled = true;
                lavaBucket.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.itemStash:
                itemStash.GetComponent<Collider2D>().enabled = true;
                itemStash.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.repairHammer:
                repairHammer.GetComponent<Collider2D>().enabled = true;
                repairHammer.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.recipeTablet:
                recipeTablet.GetComponent<Collider2D>().enabled = true;
                recipeTablet.GetComponent<SpriteRenderer>().enabled = true;
                break;
            case CampaignRewardEnum.shopMenu:
                Debug.Log("Need to fix this later on");
                break;
            default: Debug.LogWarning("ASAP"); break;

        }

    }


    //ONLY FOR TEST PURPOSE REMOVE LATER
    public void FinishTheWave()
    {
        GameModeState.IsFinished = true;
        SceneManager.LoadScene("Menu Scene");
    }




}
