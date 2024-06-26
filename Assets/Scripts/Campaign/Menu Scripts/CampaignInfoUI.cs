using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
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
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private RectTransform content;

    [SerializeField] private List<CampaignNodeScript> campaignNodes = new();

    private CampaignNodeScript currentActiveNode;



    private List<string> doneNodes = new();
    private List<string> unlockedNodes = new();


    private SaveClassManager saveClassManager;
    private EconomyManager economyManager;
    private OptionHolder optionHolder;

    private bool isStartCalled = false;

    private void LoadSORefrence()
    {
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
        optionHolder = ((OptionHolderRefrence)FindSORefrence<OptionHolder>.FindScriptableObject("Option Holder Refrence")).val;
    }

    private void Start()
    {
        if (!isStartCalled)
        {
            LoadSORefrence();
            AddISaveableToDictionary();
            ResetDoneNodes();            
        }
        if (GameModeState.IsContentPosSet)
        {
            content.position = GameModeState.ContentPos;
        }
    }
    private void OnDestroy()
    {
        GameModeState.ContentPos = content.position;
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
        int stack = campaignNode.GetRewardStackCount();
        if (stack == 0)
            infoStackText.text = string.Empty;
        else
            infoStackText.text = stack.ToString();

        description.text = campaignNode.GetDescription();
        costText.text = campaignNode.GetEnergyCost().ToString();
        if (campaignNode.GetEnergyCost() > economyManager.CampaignEnergyCurrentStack)
        {
            costText.color = Color.red;
        }
        else
        {
            costText.color = Color.black;
        }
        if (campaignNode.IsUnlocked && campaignNode.GetEnergyCost() <= economyManager.CampaignEnergyCurrentStack)
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
        //************
        //economyManager.CampaignEnergyCurrentStack -= currentActiveNode.GetEnergyCost();
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
                if (node.holdingReward == CampaignRewardEnum.qubitTalent)
                {
                    optionHolder.AddQubitMax(node.GetRewardStackCount());
                }
                else if (node.holdingReward == CampaignRewardEnum.gateTalent)
                {
                    optionHolder.AddGateMax(node.GetRewardStackCount());
                }
                else if (node.holdingReward == CampaignRewardEnum.entangleTalent)
                {
                    optionHolder.AddEntangleMax(node.GetRewardStackCount());
                }
                else if (node.holdingReward == CampaignRewardEnum.energyMax)
                {
                    economyManager.CampaignEnergyMaxStack += 1;
                }
                else
                {
                    node.GiveReward();
                }
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
