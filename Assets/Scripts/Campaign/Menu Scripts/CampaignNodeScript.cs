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


public enum CampaignRewardEnum
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
    shopMenu,
    qubitTalent,
    gateTalent,
    entangleTalent,
    energyMax
}

public class CampaignNodeScript : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private List<CampaignNodeScript> nodeConnectedToList = new();

    [SerializeField] protected List<CampaignNodeScript> nodeConnectedFrom = new();
    [SerializeField] private string nodeName;

    [SerializeField] private GameObject activeIndicator;

    [SerializeField] private Sprite rewardIcon;
    [SerializeField] private int rewardStack = 0;
    [SerializeField] private string rewardText;

    [SerializeField] protected CampaignInfoUI campaignInfo;

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool isDone = false;
    [SerializeField] private int energyCost;

    [TextArea(20,30)]
    [SerializeField] private string description;

    [SerializeField] private ProgressionScript progressionScript;
    public CampaignRewardEnum holdingReward;
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
            case CampaignRewardEnum.scanner: progressionScript.Scanner = true; break;
            case CampaignRewardEnum.computer: progressionScript.Computer = true; break;
            case CampaignRewardEnum.printer: progressionScript.Printer = true; break;
            case CampaignRewardEnum.herbalismPost: progressionScript.HerbalismPost = true; break;
            case CampaignRewardEnum.alchemyPost: progressionScript.AlchemyPost = true; break;
            case CampaignRewardEnum.waveSelector: progressionScript.WaveSelector = true; break;
            case CampaignRewardEnum.quantumStation: progressionScript.QuantumStation = true; break;
            case CampaignRewardEnum.materialFarm: progressionScript.MaterialFarm = true; break;
            case CampaignRewardEnum.seedFarm: progressionScript.SeedFarm = true; break;
            case CampaignRewardEnum.tierStation: progressionScript.TierStation = true; break;
            case CampaignRewardEnum.shopStation: progressionScript.ShopStation = true; break;
            case CampaignRewardEnum.lavaBucket: progressionScript.LavaBucket = true; break;
            case CampaignRewardEnum.itemStash: progressionScript.ItemStash = true; break;
            case CampaignRewardEnum.repairHammer: progressionScript.RepairHammer = true; break;
            case CampaignRewardEnum.recipeTablet: progressionScript.RecipeTablet = true; break;
            case CampaignRewardEnum.shopMenu: progressionScript.MenuShopIsUnlocked = true; break;
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

    public CampaignRewardEnum GetMilestone()
    {
        return holdingReward;
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

    public int GetRewardStackCount()
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

    public int GetEnergyCost()
    {
        return energyCost;
    }

    public string GetDescription()
    {
        return description;
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
            case CampaignRewardEnum.none: rewardText = "None"; break;
            case CampaignRewardEnum.scanner: rewardText = "Scanner"; break;
            case CampaignRewardEnum.computer: rewardText = "Computer"; break;
            case CampaignRewardEnum.printer: rewardText = "Printer"; break;
            case CampaignRewardEnum.herbalismPost: rewardText = "HerbalismPost"; break;
            case CampaignRewardEnum.alchemyPost: rewardText = "AlchemyPost"; break;
            case CampaignRewardEnum.waveSelector: rewardText = "WaveSelector"; break;
            case CampaignRewardEnum.quantumStation: rewardText = "QuantumStation"; break;
            case CampaignRewardEnum.materialFarm: rewardText = "MaterialFarm"; break;
            case CampaignRewardEnum.seedFarm: rewardText = "SeedFarm"; break;
            case CampaignRewardEnum.tierStation: rewardText = "TierStation"; break;
            case CampaignRewardEnum.shopStation: rewardText = "ShopStation"; break;
            case CampaignRewardEnum.lavaBucket: rewardText = "LavaBucket"; break;
            case CampaignRewardEnum.itemStash: rewardText = "ItemStash"; break;
            case CampaignRewardEnum.repairHammer: rewardText = "RepairHammer"; break;
            case CampaignRewardEnum.recipeTablet: rewardText = "RecipeTablet"; break;
            case CampaignRewardEnum.shopMenu: rewardText = "ShopMenu"; break;
            case CampaignRewardEnum.qubitTalent: rewardText = "Qubit Max +1"; break;
            case CampaignRewardEnum.gateTalent: rewardText = "Gate Max +1"; break;
            case CampaignRewardEnum.entangleTalent: rewardText = "Entangle Max +1";break;
            case CampaignRewardEnum.energyMax: rewardText = "Energy Max +1"; break;
        default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }   


    private void SetIconName()
    {
        switch (holdingReward)
        {
            case CampaignRewardEnum.none: iconSpecificAddress = "None"; break;
            case CampaignRewardEnum.scanner: iconSpecificAddress = "Scanner"; break;
            case CampaignRewardEnum.computer: iconSpecificAddress = "Computer"; break;
            case CampaignRewardEnum.printer: iconSpecificAddress = "Printer"; break;
            case CampaignRewardEnum.herbalismPost: iconSpecificAddress = "HerbalismPost"; break;
            case CampaignRewardEnum.alchemyPost: iconSpecificAddress = "AlchemyPost"; break;
            case CampaignRewardEnum.waveSelector: iconSpecificAddress = "WaveSelector"; break;
            case CampaignRewardEnum.quantumStation: iconSpecificAddress = "QuantumStation"; break;
            case CampaignRewardEnum.materialFarm: iconSpecificAddress = "MaterialFarm"; break;
            case CampaignRewardEnum.seedFarm: iconSpecificAddress = "SeedFarm"; break;
            case CampaignRewardEnum.tierStation: iconSpecificAddress = "TierStation"; break;
            case CampaignRewardEnum.shopStation: iconSpecificAddress = "ShopStation"; break;
            case CampaignRewardEnum.lavaBucket: iconSpecificAddress = "LavaBucket"; break;
            case CampaignRewardEnum.itemStash: iconSpecificAddress = "ItemStash"; break;
            case CampaignRewardEnum.repairHammer: iconSpecificAddress = "RepairHammer"; break;
            case CampaignRewardEnum.recipeTablet: iconSpecificAddress = "RecipeTablet"; break;
            case CampaignRewardEnum.shopMenu: iconSpecificAddress = "ShopMenu"; break;
            case CampaignRewardEnum.qubitTalent: iconSpecificAddress = "QubitTalent"; break;
            case CampaignRewardEnum.gateTalent: iconSpecificAddress = "GateTalent"; break;
            case CampaignRewardEnum.entangleTalent: iconSpecificAddress = "EntangleTalent"; break;
            case CampaignRewardEnum.energyMax: iconSpecificAddress = "Energy"; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }


}