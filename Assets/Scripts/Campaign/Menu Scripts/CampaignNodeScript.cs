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

    private bool isUnlocked = false;

    public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }

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
    }

    public void GotDone()
    {
        nodeConnectedTo.SetUnlocked();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        campaignInfo.SetInfoPanelActive(this);
    }
}