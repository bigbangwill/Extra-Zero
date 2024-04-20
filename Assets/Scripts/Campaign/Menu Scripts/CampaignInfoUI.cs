using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignInfoUI : MonoBehaviour, ISaveable
{
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI infoStackText;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button startButton;

    [SerializeField] private List<CampaignNodeScript> campaignNodes = new();

    private CampaignNodeScript currentActiveNode;



    private List<string> doneNodes = new();


    private SaveClassManager saveClassManager;

    private void LoadSORefrence()
    {
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
    }

    private void Start()
    {
        LoadSORefrence();
        AddISaveableToDictionary();
        ResetDoneNodes();
    }

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

    public void StartCampaign()
    {
        currentActiveNode.GotDone();
    }



    public void ResetDoneNodes()
    {
        doneNodes.Clear();
        foreach (var node in campaignNodes)
        {
            if(node.IsDone)
                doneNodes.Add(node.GetNodeName());
        }
    }

    public void AddISaveableToDictionary()
    {
        saveClassManager.AddISaveableToDictionary(GetName(), this, 0);
    }

    public object Save()
    {
        SaveClassesLibrary.CampaignInfoUI saveData = new(doneNodes);
        return saveData;
    }

    public void Load(object savedData)
    {

        foreach (var node in campaignNodes)
        {
            node.SetReset();
        }

        SaveClassesLibrary.CampaignInfoUI loadData = (SaveClassesLibrary.CampaignInfoUI)savedData;
        //doneNodes  = loadData.doneNodes;
        doneNodes.Clear();
        foreach (string node in loadData.doneNodes)
        {
            doneNodes.Add(node);
            Debug.Log(node);
        }

        foreach (var node in doneNodes)
        {
            foreach (var campNode in campaignNodes)
            {
                if (campNode.GetNodeName() == node)
                {
                    campNode.GotSilentlyDone();
                    break;
                }
            }
        }
    }

    public string GetName()
    {
        return "CampaignInfoUI";
    }
}
