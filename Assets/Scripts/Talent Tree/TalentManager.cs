using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;

public class TalentManager : SingletonComponent<TalentManager>
{

    #region Singleton
    public static TalentManager Instance
    {
        get { return (TalentManager)_Instance; }
        set { _Instance = value; }
    }
    #endregion

    [SerializeField] private List<TalentTreeOrbitalMovement> talentTrees = new();
    [SerializeField] private TalentInfoPanelUI infoPanel;

    private List<TalentLibrary> talentLibraries = new();
    private List<NodePassive> orbits = new();
    private NodePassive currentTargetedNode;
    private NodePassive[] closeOrbits = new NodePassive[3];
    private Dictionary<NodePassive, float> currentDistance = new();

    [SerializeField] private Transform talentInfoPanel;
    private NodePassive purchaseMainNode = null;

    [SerializeField] private TalentInfoPanelQubitUI menuTalentInfoPanel;
    [SerializeField] private TalentOptionsUI optionsUI;

    private GameState currentGameState;

    [Header("Line Renderer")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject entangleLinePrefab;
    [SerializeField] private Transform lineParent;


    private bool isSecondGate = false;
    private bool isSecondEntangle = false;
    private NodePassive gateBaseNode;
    private NodePassive entangleBaseNode;

    private Dictionary<NodePassive, GameObject> LineDictionary = new();

    [Header("In-Game Color Setting")]
    public Color passivePurchased;
    public Color passiveUnpurchased;
    public Color selectedPurchased;
    public Color selectedUnpurchased;
    public Color closePurchasable;
    [Header("Menu Color Setting")]
    public Color menuPassive;
    public Color menuSelected;
    public Color MenuAvailable;
    public Color menuGated;
    public Color menuEntangled;

    private void Start()
    {
        CreateNodes();
        GetAllOrbs();
        GameStateManager.Instance.ChangeStateAddListener(StateChanged);
    }



    private void StateChanged()
    {

        GameState currentState = GameStateManager.Instance.GetGameState();
        currentGameState = currentState;
        if (currentState == GameState.OnMenu)
        {
            foreach (var orbit in orbits)
            {
                if (orbit.IsGated())
                {
                    orbit.SetNodeState(NodePurchaseState.IsMenuGated);
                }
                else if (orbit.IsEntangled())
                {
                    orbit.SetNodeState(NodePurchaseState.IsMenuEntangled);
                }
                else
                {
                    orbit.SetNodeState(NodePurchaseState.IsMenuPassive);
                }
            }
        }
        else if (currentState == GameState.InGame)
        {
            foreach (var orbit in orbits)
            {
                orbit.SetNodeState(NodePurchaseState.IsNotPurchased);
            }
        }
    }


    private void GetAllOrbs()
    {
        foreach (var talent in talentTrees)
        {
            foreach(Transform orbit in talent.GetAllNodes())
            {
                orbits.Add(orbit.GetComponent<NodePassive>());
            }
        }
    }


    private bool isPurchaseMode = false;


    public void SetState(NodePassive passive)
    {
        if (passive.IsPurchased)
            passive.SetNodeState(NodePurchaseState.IsPurchased);
        else
            passive.SetNodeState(NodePurchaseState.IsNotPurchased);
    }

    public void MenuSetState(NodePassive passive)
    {
        if(passive.IsGated())
            passive.SetNodeState(NodePurchaseState.IsMenuGated);
        else
            passive.SetNodeState(NodePurchaseState.IsMenuPassive);
    }

    public void SetTargetNode(NodePassive targetNode)
    {
        if (currentGameState == GameState.OnMenu)
        {
            MenuSetTargetNode(targetNode);
        }
        else if (currentGameState == GameState.InGame)
        {
            InGameSetTargetNode(targetNode);
        }
    }

    

    public void SetGateStart()
    {
        isSecondGate = true;
        gateBaseNode = currentTargetedNode;
        foreach (var orbit in orbits)
        {
            if (orbit.IsQubit() && !orbit.IsGated() && orbit != gateBaseNode)
            {
                orbit.SetNodeState(NodePurchaseState.IsMenuAvailable);
            }
        }
    }

    public void SetEntangleStart()
    {
        isSecondEntangle = true;
        entangleBaseNode = currentTargetedNode;
        foreach (var orbit in orbits)
        {
            if (orbit.IsQubit() && !orbit.IsGated() && orbit != gateBaseNode)
            {
                orbit.SetNodeState(NodePurchaseState.IsMenuAvailable);
            }
        }
    }

    private void MenuSetTargetNode(NodePassive targetNode)
    {
        if(currentTargetedNode != null)
            MenuSetState(currentTargetedNode);
        if (isSecondGate)
        {
            if (targetNode.CurrentState == NodePurchaseState.IsMenuAvailable)
            {
                gateBaseNode.UpgradeGate(targetNode);
                gateBaseNode.SetNodeState(NodePurchaseState.IsMenuGated);
                targetNode.SetNodeState(NodePurchaseState.IsMenuGated);
                StartGateLine(gateBaseNode.transform.GetChild(0),targetNode.transform.GetChild(0));
                OptionHolder.Instance.AddGateCurrent(+1);
                isSecondGate = false;
                gateBaseNode = null;

                foreach (var orbit in orbits)
                {
                    if (orbit.CurrentState == NodePurchaseState.IsMenuAvailable)
                    {
                        orbit.SetNodeState(NodePurchaseState.IsMenuPassive);
                    }
                }
                optionsUI.SetDeative();
                return;
            }
            else
            {
                gateBaseNode = null;
                isSecondGate = false;
            }
            foreach (var orbit in orbits)
            {
                if (orbit.CurrentState == NodePurchaseState.IsMenuAvailable)
                {
                    orbit.SetNodeState(NodePurchaseState.IsMenuPassive);
                }
            }
        }

        if (isSecondEntangle)
        {
            if (targetNode.CurrentState == NodePurchaseState.IsMenuAvailable)
            {
                entangleBaseNode.UpgradeEntangle(targetNode);
                entangleBaseNode.SetNodeState(NodePurchaseState.IsMenuEntangled);
                targetNode.SetNodeState(NodePurchaseState.IsMenuEntangled);
                StartEntangleLine(entangleBaseNode.transform.GetChild(0), targetNode.transform.GetChild(0));
                OptionHolder.Instance.AddEntangleCurrent(+1);
                isSecondEntangle = false;
                entangleBaseNode = null;

                foreach (var orbit in orbits)
                {
                    if (orbit.CurrentState == NodePurchaseState.IsMenuAvailable)
                    {
                        orbit.SetNodeState(NodePurchaseState.IsMenuPassive);
                    }
                }
                optionsUI.SetDeative();
                return;
            }
            else
            {
                entangleBaseNode = null;
                isSecondEntangle = false;
            }
            foreach (var orbit in orbits)
            {
                if (orbit.CurrentState == NodePurchaseState.IsMenuAvailable)
                {
                    orbit.SetNodeState(NodePurchaseState.IsMenuPassive);
                }
            }
        }

        if (currentTargetedNode == targetNode)
        {
            menuTalentInfoPanel.gameObject.SetActive(false);
            currentTargetedNode = null;
            optionsUI.SetDeative();
            return;
        }

        currentTargetedNode = targetNode;
        targetNode.SetNodeState(NodePurchaseState.IsMenuSelected);
        menuTalentInfoPanel.SetQubitInfoUI(targetNode);
        optionsUI.SetActive(currentTargetedNode);
            
    }

    private void InGameSetTargetNode(NodePassive targetNode)
    {
        currentDistance.Clear();
        bool isTargetPurchased = targetNode.IsPurchased;
        if (currentTargetedNode != null)
            SetState(currentTargetedNode);

        if (isPurchaseMode)
        {
            if (targetNode == purchaseMainNode)
            {
                isPurchaseMode = false;
                SetState(purchaseMainNode);
                purchaseMainNode = null;
                ResetNodesBackToDefault(targetNode);
                return;
            }
            foreach (var orbit in closeOrbits)
            {
                orbit.SetNodeState(NodePurchaseState.IsCloseTarget);
            }
            foreach (var orbit in closeOrbits)
            {
                if (orbit == targetNode)
                {
                    infoPanel.SetActivePanel(targetNode, !targetNode.IsPurchased);
                    targetNode.SetNodeState(NodePurchaseState.IsSelectedNotPurchased);
                    currentTargetedNode = orbit;
                    return;
                }
            }
        }
        ResetCloseStates();
        if (targetNode == currentTargetedNode)
        {
            ResetNodesBackToDefault(targetNode);
            talentInfoPanel.gameObject.SetActive(false);
            return;
        }
        if (isTargetPurchased)
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetNodeState(NodePurchaseState.IsSelectedPurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = true;
            purchaseMainNode = targetNode;
            infoPanel.SetActivePanel(targetNode, false);
            foreach (var orbit in closeOrbits)
            {
                orbit.SetNodeState(NodePurchaseState.IsCloseTarget);
            }
        }
        else
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetNodeState(NodePurchaseState.IsSelectedNotPurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = false;
            infoPanel.SetActivePanel(targetNode, false);
        }
    }

    private void StartGateLine(Transform start,Transform end)
    {
        GameObject target = Instantiate(linePrefab, lineParent);
        target.GetComponent<GateLineRenderer>().Setup(start, end);
        LineDictionary.Add(gateBaseNode, target);
    }

    private void StartEntangleLine(Transform start, Transform end)
    {
        GameObject target = Instantiate(entangleLinePrefab, lineParent);
        target.GetComponent<GateLineRenderer>().Setup(start,end);
        LineDictionary.Add(entangleBaseNode, target);
    }

    public void TryStopLine(NodePassive targetNode)
    {
        if (LineDictionary.ContainsKey(targetNode))
        {
            Destroy(LineDictionary[targetNode]);
        }
    }

    public void PurchaseButtonClicked()
    {
        isPurchaseMode = false;
        purchaseMainNode = null;
        currentTargetedNode.PurchaseTalent();
        currentTargetedNode = null;
        ResetCloseStates();
        closeOrbits = new NodePassive[3];
        infoPanel.DeactivePanel();
    }

    private void ResetNodesBackToDefault(NodePassive targetNode)
    {
        SetState(targetNode);
        currentTargetedNode = null;
        ResetCloseStates();
        infoPanel.DeactivePanel();
        closeOrbits = new NodePassive[3];
    }


    private void ResetCloseStates()
    {
        foreach (var orbit in closeOrbits)
        {
            if(orbit != null)
                SetState(orbit);
        }
    }

    private void TargetClosest(NodePassive targetNode)
    {
        closeOrbits = new NodePassive[3];
        foreach (var orbit in orbits)
        {
            if (orbit == targetNode)
            {
                continue;
            }
            Vector3 targetNodeChild = targetNode.transform.GetChild(0).position;
            Vector3 orbitNodeChild = orbit.transform.GetChild(0).position;
            float distance = Vector3.Distance(targetNodeChild, orbitNodeChild);
            currentDistance.Add(orbit, distance);
        }

        var sortedDistances = currentDistance.OrderBy(x => x.Value);
        closeOrbits = sortedDistances.Take(3).Select(pair => pair.Key).ToArray();
    }

    public void CreateNodes()
    {
        List<Type> talentEffects = Assembly.GetAssembly(typeof(TalentLibrary)).GetTypes().Where
            (TheType => TheType.IsClass && !TheType.IsAbstract &&
            TheType.IsSubclassOf(typeof(TalentLibrary))).ToList();
        foreach (var effect in talentEffects)
        {
            TalentLibrary talent = (TalentLibrary)Activator.CreateInstance(effect);
            talentLibraries.Add(talent);
            talent.TalentEffect();
        }

        int counter = 0;
        foreach(TalentLibrary talent in talentLibraries)
        {
            talentTrees[counter].AddToTalentList(talent);
            counter++;
            if (counter == talentTrees.Count)
                counter = 0;            
        }
        foreach (var tree in talentTrees)
        {
            tree.StartSummonNodes();
        }
    }
}