using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignInfoUI : MonoBehaviour
{
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI infoStackText;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button startButton;
    private CampaignNodeScript currentActiveNode;

    public void SetInfoPanelActive(CampaignNodeScript campaignNode)
    {
        if (currentActiveNode != null)
            currentActiveNode.GotDetargeted();
        infoPanel.SetActive(true);
        campaignNode.GotTargeted();
        currentActiveNode = campaignNode;
        infoImage.sprite = campaignNode.GetRewardIcon();
        infoText.text = campaignNode.GetRewardText();
        infoStackText.text = campaignNode.GetRewardStack();
        if (campaignNode.IsUnlocked)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }


    public void SetDeactive()
    {
        currentActiveNode.GotDetargeted();
        currentActiveNode = null;
    }

    public void StartCampaign(CampaignNodeScript campaignNode)
    {

    }
}
