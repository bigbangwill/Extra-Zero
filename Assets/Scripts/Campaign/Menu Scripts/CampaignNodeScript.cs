using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CampaignNodeScript : MonoBehaviour, IPointerClickHandler
{

    public enum campaignRewardEnum
    {
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
        itemStash
    }


    [SerializeField] private CampaignNodeScript nodeConnectedTo;
    [SerializeField] private CampaignNodeScript nodeConnectedFrom;
    [SerializeField] private string nodeName;

    [SerializeField] private GameObject activeIndicator;

    [SerializeField] private Sprite rewardIcon;
    [SerializeField] private string rewardStack;
    [SerializeField] private string rewardText;

    [SerializeField] private CampaignInfoUI campaignInfo;

    [SerializeField] private bool isUnlocked = false;
    [SerializeField] private bool isDone = false;

    [SerializeField] private ProgressionScript progressionScript;
    [SerializeField] private campaignRewardEnum holdingReward;

    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    public bool IsDone { get => isDone; set => isDone = value; }


    public void GiveReward()
    {
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
            case campaignRewardEnum.lavaBucket:progressionScript.LavaBucket = true; break;
            case campaignRewardEnum.itemStash:progressionScript.ItemStash = true; break;
            default:Debug.LogWarning("ASAP"); break;
        }
    }

    public void GotTargeted()
    {
        activeIndicator.SetActive(true);
    }

    public void GotDetargeted()
    {
        activeIndicator.SetActive(false);
    }

    public void SetUnlocked()
    {
        IsUnlocked = true;
        campaignInfo.ResetDoneNodes();
    }

    public void GotDone()
    {
        isDone = true;
        if(nodeConnectedTo != null)
            nodeConnectedTo.SetUnlocked();
    }

    public void GotSilentlyDone()
    {
        isDone = true;
        isUnlocked = true;
        if (nodeConnectedTo != null)
            nodeConnectedTo.IsUnlocked = true;
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
}