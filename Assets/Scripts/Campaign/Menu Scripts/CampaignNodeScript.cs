using ExtraZero.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public class CampaignNodeScript : MonoBehaviour, IPointerClickHandler
{

    public enum campaignRewardEnum
    {
        none,
        scanner,
        computer,
        printer,
        herbalismPost,
        alchemyPost,
        waveSelector,
        quantumStation,
        materialFarm,
        seedFarm,
        tierStation,
        shopStation,
        lavaBucket,
        itemStash,
        repairHammer,
        recipeTablet,
        shopMenu
    }

    [SerializeField] private List<CampaignNodeScript> nodeConnectedToList = new();

    [SerializeField] protected List<CampaignNodeScript> nodeConnectedFrom = new();
    [SerializeField] private string nodeName;

    [SerializeField] private GameObject activeIndicator;

    [SerializeField] private Sprite rewardIcon;
    [SerializeField] private string rewardStack;
    [SerializeField] private string rewardText;

    [SerializeField] protected CampaignInfoUI campaignInfo;

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool isDone = false;

    [SerializeField] private ProgressionScript progressionScript;
    [SerializeField] private campaignRewardEnum holdingReward;
    [SerializeField] private Dialogue dialogue;

    private string iconSpecificAddress;

    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    public bool IsDone { get => isDone; set => isDone = value; }
    public Dialogue Dialogue { get => dialogue; set => dialogue = value; }

    private void Start()
    {
        SetTotalState();
    }


    protected virtual void LoadIcon()
    {
        try
        {

            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>("Talent Reward Icon/" + iconSpecificAddress + "[Sprite]");
            handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                rewardIcon = handle.Result;
            }
            else
            {
                Debug.Log("Talent Reward Icon/" + iconSpecificAddress + "[Sprite]");
                Debug.LogError("Failed to load the asset");
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }



    public void GiveReward()
    {
        Debug.Log(holdingReward.ToString());
        switch (holdingReward) 
        {
            case campaignRewardEnum.scanner: progressionScript.Scanner = true; break;
            case campaignRewardEnum.computer: progressionScript.Computer = true; break;
            case campaignRewardEnum.printer: progressionScript.Printer = true; break;
            case campaignRewardEnum.herbalismPost: progressionScript.HerbalismPost = true; break;
            case campaignRewardEnum.alchemyPost: progressionScript.AlchemyPost = true; break;
            case campaignRewardEnum.waveSelector: progressionScript.WaveSelector = true; break;
            case campaignRewardEnum.quantumStation: progressionScript.QuantumStation = true; break;
            case campaignRewardEnum.materialFarm: progressionScript.MaterialFarm = true; break;
            case campaignRewardEnum.seedFarm: progressionScript.SeedFarm = true; break;
            case campaignRewardEnum.tierStation: progressionScript.TierStation = true; break;
            case campaignRewardEnum.shopStation: progressionScript.ShopStation = true; break;
            case campaignRewardEnum.lavaBucket: progressionScript.LavaBucket = true; break;
            case campaignRewardEnum.itemStash: progressionScript.ItemStash = true; break;
            case campaignRewardEnum.repairHammer: progressionScript.RepairHammer = true; break;
            case campaignRewardEnum.recipeTablet: progressionScript.RecipeTablet = true; break;
            case campaignRewardEnum.shopMenu: progressionScript.MenuShopIsUnlocked = true; break;
            default:Debug.LogWarning("ASAP"); break;
        }
    }


    public void SetNodeName(string name)
    {
        nodeName = name;
    }


    public void GotTargeted()
    {
        activeIndicator.SetActive(true);
    }

    public void GotDetargeted()
    {
        activeIndicator.SetActive(false);
    }

    public virtual void SetUnlocked()
    {
        IsUnlocked = true;
        //campaignInfo.ResetDoneNodes();
    }

    public void GotDone()
    {
        isDone = true;
        foreach (var node in nodeConnectedToList)
        {
            node.SetUnlocked();
        }
        foreach (var node in nodeConnectedFrom)
        {
            node.SetUnlocked();
        }
    }

    public void GotSilentlyDone()
    {
        isDone = true;
        isUnlocked = true;
        foreach (var node in nodeConnectedToList)
        {
            node.SetUnlocked();
        }
    }

    public void SetReset()
    {
        IsDone = false;
        isUnlocked = false;
    }

    public Sprite GetRewardIcon()
    {
        return rewardIcon;
    }

    public string GetRewardStack()
    {
        return rewardStack;
    }

    public string GetRewardText()
    {
        return rewardText;
    }

    public string GetNodeName()
    {
        return nodeName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        campaignInfo.SetInfoPanelActive(this);
    }



    private void SetTotalState()
    {
        SetIconName();
        SetRewardText();
        LoadIcon();
    }

    private void SetRewardText()
    {
        switch (holdingReward)
        {
        case campaignRewardEnum.none: rewardText = "None"; break;
        case campaignRewardEnum.scanner: rewardText = "Scanner"; break;
        case campaignRewardEnum.computer: rewardText = "Computer"; break;
        case campaignRewardEnum.printer: rewardText = "Printer"; break;
        case campaignRewardEnum.herbalismPost: rewardText = "HerbalismPost"; break;
        case campaignRewardEnum.alchemyPost: rewardText = "AlchemyPost"; break;
        case campaignRewardEnum.waveSelector: rewardText = "WaveSelector"; break;
        case campaignRewardEnum.quantumStation: rewardText = "QuantumStation"; break;
        case campaignRewardEnum.materialFarm: rewardText = "MaterialFarm"; break;
        case campaignRewardEnum.seedFarm: rewardText = "SeedFarm"; break;
        case campaignRewardEnum.tierStation: rewardText = "TierStation"; break;
        case campaignRewardEnum.shopStation: rewardText = "ShopStation"; break;
        case campaignRewardEnum.lavaBucket: rewardText = "LavaBucket"; break;
        case campaignRewardEnum.itemStash: rewardText = "ItemStash"; break;
        case campaignRewardEnum.repairHammer: rewardText = "RepairHammer"; break;
        case campaignRewardEnum.recipeTablet: rewardText = "RecipeTablet"; break;
        case campaignRewardEnum.shopMenu: rewardText = "ShopMenu"; break;
        default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }   


    private void SetIconName()
    {
        switch (holdingReward)
        {
            case campaignRewardEnum.none: iconSpecificAddress = "None"; break;
            case campaignRewardEnum.scanner: iconSpecificAddress = "Scanner"; break;
            case campaignRewardEnum.computer: iconSpecificAddress = "Computer"; break;
            case campaignRewardEnum.printer: iconSpecificAddress = "Printer"; break;
            case campaignRewardEnum.herbalismPost: iconSpecificAddress = "HerbalismPost"; break;
            case campaignRewardEnum.alchemyPost: iconSpecificAddress = "AlchemyPost"; break;
            case campaignRewardEnum.waveSelector: iconSpecificAddress = "WaveSelector"; break;
            case campaignRewardEnum.quantumStation: iconSpecificAddress = "QuantumStation"; break;
            case campaignRewardEnum.materialFarm: iconSpecificAddress = "MaterialFarm"; break;
            case campaignRewardEnum.seedFarm: iconSpecificAddress = "SeedFarm"; break;
            case campaignRewardEnum.tierStation: iconSpecificAddress = "TierStation"; break;
            case campaignRewardEnum.shopStation: iconSpecificAddress = "ShopStation"; break;
            case campaignRewardEnum.lavaBucket: iconSpecificAddress = "LavaBucket"; break;
            case campaignRewardEnum.itemStash: iconSpecificAddress = "ItemStash"; break;
            case campaignRewardEnum.repairHammer: iconSpecificAddress = "RepairHammer"; break;
            case campaignRewardEnum.recipeTablet: iconSpecificAddress = "RecipeTablet"; break;
            case campaignRewardEnum.shopMenu: iconSpecificAddress = "ShopMenu"; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }


}