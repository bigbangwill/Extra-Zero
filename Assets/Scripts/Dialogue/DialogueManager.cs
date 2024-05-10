using ExtraZero.Dialogue;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private TextMeshProUGUI textObject;

    [SerializeField] private float textAddingSpeed;


    [SerializeField] private Transform scanner;
    [SerializeField] private Transform computer;
    [SerializeField] private Transform printer;
    [SerializeField] private Transform herbalismPost;
    [SerializeField] private Transform alchemyPost;
    [SerializeField] private Transform waveSelector;
    [SerializeField] private Transform quantumStation;
    [SerializeField] private Transform materialFarm;
    [SerializeField] private Transform seedFarm;
    [SerializeField] private Transform tierStation;
    [SerializeField] private Transform shopStation;
    [SerializeField] private Transform lavaBucket;
    [SerializeField] private Transform itemStash;
    [SerializeField] private Transform repairHammer;
    [SerializeField] private Transform recipeTablet;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Vector3 cameraLocalPosition;

    [SerializeField] private CycleInformation cycleInformation;


    private bool shouldMove = false;
    private Vector2 movingObject;
    private Vector3 camStartPos;

    private float currentTimer = 0;

    private DialogueNode currentNode;
    private string leftToShow;


    private bool isAdding;

    private DialogueManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (DialogueManagerRefrence)FindSORefrence<DialogueManager>.FindScriptableObject("Dialogue Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetRefrence();
        camStartPos = mainCamera.transform.localPosition;
    }


    private void Update()
    {
        if (isAdding)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > textAddingSpeed)
            {
                textObject.text += leftToShow[0];
                leftToShow = leftToShow[1..];
                currentTimer = 0;
                if (leftToShow.Length == 0)
                {
                    isAdding = false;
                }
            }
        }

        if (shouldMove)
        {
            mainCamera.position = Vector2.Lerp(mainCamera.position, movingObject, Time.deltaTime);
            mainCamera.position = new Vector3(mainCamera.position.x, mainCamera.position.y, -20);
            if (Vector2.Distance(transform.position, mainCamera.position) < 1f)
            {
                shouldMove = false;
            }

        }
    }


    private List<DialogueNode> holdingNodes = new();
    private int nodeCounter = 0;


    public void ShowDialogue(Dialogue input)
    {
        gameObject.SetActive(true);
        currentDialogue = input;
        holdingNodes = currentDialogue.GetAllNodes().ToList();
        currentNode = holdingNodes[nodeCounter];
        textObject.text = string.Empty;
        leftToShow = currentNode.GetText();
        isAdding = true;
    }


    public void SetMovingTarget(CampaignRewardEnum milestone)
    {
        camStartPos = mainCamera.position;
        shouldMove = true;
        switch (milestone)
        {
            case CampaignRewardEnum.none: shouldMove = false; break;
            case CampaignRewardEnum.scanner: movingObject = scanner.position; break;
            case CampaignRewardEnum.computer: movingObject = computer.position; break;
            case CampaignRewardEnum.printer: movingObject = printer.position; break;
            case CampaignRewardEnum.herbalismPost: movingObject = herbalismPost.position; break;
            case CampaignRewardEnum.alchemyPost: movingObject = alchemyPost.position; break;
            case CampaignRewardEnum.waveSelector: movingObject = waveSelector.position; break;
            case CampaignRewardEnum.quantumStation: movingObject = quantumStation.position; break;
            case CampaignRewardEnum.materialFarm: movingObject = materialFarm.position; break;
            case CampaignRewardEnum.seedFarm: movingObject = seedFarm.position; break;
            case CampaignRewardEnum.tierStation: movingObject = tierStation.position; break;
            case CampaignRewardEnum.shopStation: movingObject = shopStation.position; break;
            case CampaignRewardEnum.lavaBucket: movingObject = lavaBucket.position; break;
            case CampaignRewardEnum.itemStash: movingObject = itemStash.position; break;
            case CampaignRewardEnum.repairHammer: movingObject = repairHammer.position; break;
            case CampaignRewardEnum.recipeTablet: movingObject = recipeTablet.position; break;
            case CampaignRewardEnum.shopMenu: shouldMove = false; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }

    public void Skip()
    {
        if (isAdding)
        {
            isAdding = false;
            textObject.text = currentNode.GetText();
        }
        else
        {
            GoNextChild();
        }
    }


    private void GoNextChild()
    {
        if (nodeCounter < holdingNodes.Count -1)
        {
            nodeCounter += 1;
            currentNode = holdingNodes[nodeCounter];
            textObject.text = string.Empty;
            leftToShow = currentNode.GetText();
            isAdding = true;
            Debug.Log("Inside if");
        }
        else
        {
            FinishedDialogue();
            Debug.Log("outside if");
            return;
        }
    }

    public void FinishedDialogue()
    {
        mainCamera.localPosition = cameraLocalPosition;
        cycleInformation.StartCounter();
        gameObject.SetActive(false);
    }


}
