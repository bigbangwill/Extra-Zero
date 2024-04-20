using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CampaignNodeScript : MonoBehaviour, IPointerClickHandler
{

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

    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    public bool IsDone { get => isDone; set => isDone = value; }

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