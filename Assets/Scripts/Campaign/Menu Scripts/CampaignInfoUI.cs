using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CampaignInfoUI : MonoBehaviour, ISaveable, ILoadDependent
{
    [SerializeField] private Image infoImage;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI infoStackText;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private Button startButton;

    [SerializeField] private List<CampaignNodeScript> campaignNodes = new();

    private CampaignNodeScript currentActiveNode;



    private List<string> doneNodes = new();
    private List<string> unlockedNodes = new();


    private SaveClassManager saveClassManager;

    private bool isStartCalled = false;

    private void LoadSORefrence()
    {
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
    }

    private void Start()
    {
        if (!isStartCalled)
        {
            LoadSORefrence();
            AddISaveableToDictionary();
            ResetDoneNodes();
        }
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
        GameModeState.CurrentCampaignNode = currentActiveNode.GetNodeName();
        GameModeState.IsCampaignMode = true;
        GameModeState.IsFinished = false;
        GameModeState.MilestoneReward = currentActiveNode.GetMilestone();
        if (currentActiveNode.Dialogue != null)
            GameModeState.CurrentDialogue = currentActiveNode.Dialogue;
        else
            GameModeState.CurrentDialogue = null;
        saveClassManager.SaveCurrentState();
        SceneManager.LoadScene("Scene One");
    }

    public void StartEndlessMode()
    {
        GameModeState.CurrentCampaignNode = null;
        GameModeState.IsFinished = false;
        GameModeState.CurrentDialogue = null;
        saveClassManager.SaveCurrentState();
        GameModeState.IsCampaignMode = false;
        GameModeState.CurrentDialogue = null;
        SceneManager.LoadScene("Scene One");
    }

    public void GiveReward(string name)
    {
        foreach (var node in campaignNodes)
        {
            if (node.GetNodeName() == name)
            {
                node.GiveReward();
                node.GotDone();
            }
        }
    }

    public void ResetDoneNodes()
    {
        doneNodes.Clear();
        foreach (var node in campaignNodes)
        {
            if(node.IsDone)
                doneNodes.Add(node.GetNodeName());
            if(node.IsUnlocked)
                unlockedNodes.Add(node.GetNodeName());
        }
    }

    public void AddISaveableToDictionary()
    {
        saveClassManager.AddISaveableToDictionary(GetName(), this, 0);
    }

    public object Save()
    {
        ResetDoneNodes();
        SaveClassesLibrary.CampaignInfoUI saveData = new(doneNodes,unlockedNodes);
        return saveData;
    }

    public void Load(object savedData)
    {

        foreach (var node in campaignNodes)
        {
            node.SetReset();
        }

        SaveClassesLibrary.CampaignInfoUI loadData = (SaveClassesLibrary.CampaignInfoUI)savedData;
        doneNodes  = loadData.doneNodes;
        unlockedNodes = loadData.unlockedNodes;
        //doneNodes.Clear();
        //foreach (string node in loadData.doneNodes)
        //{
        //    doneNodes.Add(node);
        //    Debug.Log(node);
        //}

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

        foreach (var node in unlockedNodes)
        {
            foreach (var campNode in campaignNodes)
            {
                if (campNode.GetNodeName() == node)
                {
                    campNode.IsUnlocked = true;
                    break;
                }
            }
        }
    }

    public string GetName()
    {
        return "CampaignInfoUI";
    }

    public void CallAwake()
    {
        
    }

    public void CallStart()
    {
        Start();
        isStartCalled = true;
    }
}
