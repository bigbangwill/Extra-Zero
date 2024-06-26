using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using System.Data;

public class TalentManager : MonoBehaviour, ISaveable
{

    [SerializeField] private List<TalentTreeOrbitalMovement> talentTrees = new();
    [SerializeField] private TalentInfoPanelUI infoPanel;

    private List<TalentLibrary> talentLibraries = new();
    private List<NodePassive> orbits = new();
    private NodePassive currentTargetedNode;
    private List<NodePassive> closeOrbits = new();
    private Dictionary<NodePassive, float> currentDistance = new();

    [SerializeField] private Transform talentInfoPanel;
    private NodePassive purchaseMainNode = null;

    [SerializeField] private TalentInfoPanelQubitUI menuTalentInfoPanel;
    [SerializeField] private TalentOptionsUI optionsUI;

    [SerializeField] private GameState currentGameState;

    [Header("Line Renderer")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject entangleLinePrefab;
    [SerializeField] private Transform lineParent;


    private bool isSecondGate = false;
    private bool isSecondEntangle = false;
    private NodePassive gateBaseNode;
    private NodePassive entangleBaseNode;

    private bool nodesCreated = false;

    private Dictionary<string, GameObject> LineDictionary = new();

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

    //private static TalentManager instance = null;

    private TalentManagerRefrence refrence;

    private OptionHolderRefrence optionHolderRefrence;
    private GameStateManagerRefrence gameStateManagerRefrence;


    private ScannerSlotManagerRefrence scannerSlotManagerRefrence;

    private SaveClassManager saveClassManager;

    private void LoadSORefrence()
    {
        gameStateManagerRefrence = (GameStateManagerRefrence)FindSORefrence<GameStateManager>.FindScriptableObject("Game State Manager Refrence");
        optionHolderRefrence = (OptionHolderRefrence)FindSORefrence<OptionHolder>.FindScriptableObject("Option Holder Refrence");
        saveClassManager = ((SaveClassManagerRefrence)FindSORefrence<SaveClassManager>.FindScriptableObject("Save Class Manager refrence")).val;
    }

    private void SetRefrence()
    {
        refrence = (TalentManagerRefrence)FindSORefrence<TalentManager>.FindScriptableObject("Talent Manager Refrence");
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
    }



    private void Start()
    {
        LoadSORefrence();
        AddISaveableToDictionary();
        if (!nodesCreated)
        {
            if (!CreatedTalents.IsCreated)
            {
                CreateTalents();
            }
            CreateNodes();
            GetAllOrbs();
            
        }
        //gameStateManagerRefrence.val.ChangeStateAddListener(StateChanged);
        StateChanged();
    }

    public List<NodePassive> GetNodePassives()
    {
        return orbits;
    }



    private void StateChanged()
    {

        //GameState currentState = gameStateManagerRefrence.val.GetGameState();
        //currentGameState = currentState;
        if (currentGameState == GameState.OnMenu)
        {
            foreach (var orbit in orbits)
            {
                orbit.ResetToDefault();
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
        else if (currentGameState == GameState.InGame)
        {
            foreach (var orbit in orbits)
            {
                orbit.SetNodeState(NodePurchaseState.IsNotPurchased);
            }
            //orbits[0].PurchaseTalent();
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
        if (passive.IsGated())
            passive.SetNodeState(NodePurchaseState.IsMenuGated);
        else if (passive.IsEntangled())
            passive.SetNodeState(NodePurchaseState.IsMenuEntangled);
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
                gateBaseNode.UpgradeGate(targetNode.talent);
                gateBaseNode.SetNodeState(NodePurchaseState.IsMenuGated);
                //targetNode.SetNodeState(NodePurchaseState.IsMenuGated);
                //StartGateLine(gateBaseNode.transform.GetChild(0),targetNode.transform.GetChild(0));
                optionHolderRefrence.val.AddGateCurrent(+1);
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
                entangleBaseNode.UpgradeEntangle(targetNode.talent);
                entangleBaseNode.SetNodeState(NodePurchaseState.IsMenuEntangled);
                targetNode.SetNodeState(NodePurchaseState.IsMenuEntangled);
                //StartEntangleLine(entangleBaseNode.transform.GetChild(0), targetNode.transform.GetChild(0));
                optionHolderRefrence.val.AddEntangleCurrent(+1);
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
            if (currentTargetedNode.IsGated())
            {
                closeOrbits.Clear();
                closeOrbits.Add(currentTargetedNode.GetNodeToGate());
            }
            else
            {
                TargetClosest(currentTargetedNode);
            }
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
            if (currentTargetedNode.IsGated())
            {
                closeOrbits.Clear();
                closeOrbits.Add(currentTargetedNode.GetNodeToGate());
            }
            else
            {
                TargetClosest(currentTargetedNode);
            }
            isPurchaseMode = false;
            infoPanel.SetActivePanel(targetNode, false);
        }
    }

    public void StartGateLine(Transform start,Transform end,TalentLibrary gateBaseNode)
    {
        GameObject target = Instantiate(linePrefab, lineParent);
        target.GetComponent<GateLineRenderer>().Setup(start, end);
        
        LineDictionary.Add(gateBaseNode.GetSpecificName(), target);
        Debug.Log(gateBaseNode.GetSpecificName());
    }

    public void StartEntangleLine(Transform start, Transform end,TalentLibrary entangleBaseNode)
    {
        GameObject target = Instantiate(entangleLinePrefab, lineParent);
        target.GetComponent<GateLineRenderer>().Setup(start,end);
        LineDictionary.Add(entangleBaseNode.GetSpecificName(), target);
    }

    public void TryStopLine(TalentLibrary targetNode)
    {
        if (LineDictionary.ContainsKey(targetNode.GetSpecificName()))
        {
            Destroy(LineDictionary[targetNode.GetSpecificName()]);
            LineDictionary.Remove(targetNode.GetSpecificName());
        }
        else
        {
            Debug.Log("Couldnt find it" + targetNode);
        }
    }

    public void PurchaseButtonClicked()
    {
        isPurchaseMode = false;
        purchaseMainNode = null;
        currentTargetedNode.PurchaseTalent();
        currentTargetedNode = null;
        ResetCloseStates();
        closeOrbits.Clear();
        infoPanel.DeactivePanel();
    }

    private void ResetNodesBackToDefault(NodePassive targetNode)
    {
        SetState(targetNode);
        currentTargetedNode = null;
        ResetCloseStates();
        infoPanel.DeactivePanel();
        closeOrbits.Clear();
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
        closeOrbits.Clear();
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
        closeOrbits = sortedDistances.Take(3).Select(pair => pair.Key).ToList();
        foreach (var orbit in closeOrbits.ToList())
        {
            if(orbit.IsPurchased)
                closeOrbits.Remove(orbit);
        }
    }

    private void CreateTalents()
    {
        List<Type> talentEffects = Assembly.GetAssembly(typeof(TalentLibrary)).GetTypes().Where
            (TheType => TheType.IsClass && !TheType.IsAbstract &&
            TheType.IsSubclassOf(typeof(TalentLibrary))).ToList();
        List<TalentLibrary> createdTalents = new();
        foreach (var effect in talentEffects)
        {
            TalentLibrary talent = (TalentLibrary)Activator.CreateInstance(effect);
            createdTalents.Add(talent);
            //talent.TalentEffect();
        }
        CreatedTalents.CreateTalents(createdTalents);

    }

    public void CreateNodes()
    {
        int counter = 0;
        foreach(TalentLibrary talent in CreatedTalents.GetTalents())
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
        foreach (var tree in talentTrees)
        {
            tree.SetPassiveStats();
        }
        nodesCreated = true;
    }

    public void PurchaseRandomQubit()
    {
        int target = UnityEngine.Random.Range(0, orbits.Count);
        orbits[target].PurchaseTalent();
    }

    public void AddISaveableToDictionary()
    {
        saveClassManager.AddISaveableToDictionary(GetName(), this, 4);
    }

    public object Save()
    {
        SaveClassesLibrary.TalentManagerSave data = new(this);
        return data;
    }

    public void Load(object savedData)
    {
        SaveClassesLibrary.TalentManagerSave loadData = (SaveClassesLibrary.TalentManagerSave)savedData;
        //foreach (var orbit in loadData.qubitList)
        //{
        //    foreach (var createdNodes in CreatedTalents.GetTalents())
        //    {
        //        if (orbit == createdNodes.GetSpecificName())
        //        {
        //            createdNodes.UpgradeToQubit();
        //        }
        //    }
        //}
        //for (int i = 0; i < loadData.gateListKey.Count; i++)
        //{
        //    foreach (var createdNodes in CreatedTalents.GetTalents())
        //    {
        //        if (loadData.gateListKey[i] == createdNodes.GetSpecificName())
        //        {
        //            foreach (var secondCreatedNodes in CreatedTalents.GetTalents())
        //            {
        //                if (loadData.gateListValue[i] == secondCreatedNodes.GetSpecificName())
        //                {
        //                    createdNodes.AddGateToQubit(secondCreatedNodes);
        //                }
        //            }
        //        }
        //    }
        //}

        //for (int i = 0; i < loadData.entangleListKey.Count; i++)
        //{
        //    foreach (var createdNodes in CreatedTalents.GetTalents())
        //    {
        //        if (loadData.entangleListKey[i] == createdNodes.GetSpecificName())
        //        {
        //            foreach (var secondCreatedNodes in CreatedTalents.GetTalents())
        //            {
        //                if (loadData.entangleListValue[i] == secondCreatedNodes.GetSpecificName())
        //                {
        //                    createdNodes.AddEntangleToQubit(secondCreatedNodes);
        //                }
        //            }
        //        }
        //    }
        //}

        foreach (var orbit in loadData.qubitList)
        {
            foreach (var createdNodes in orbits)
            {
                if (orbit == createdNodes.talent.GetSpecificName())
                {
                    createdNodes.UpgradeQubit();
                }
            }
        }
        for (int i = 0; i < loadData.gateListKey.Count; i++)
        {
            foreach (var createdNodes in orbits)
            {
                if (loadData.gateListKey[i] == createdNodes.talent.GetSpecificName())
                {
                    foreach (var secondCreatedNodes in orbits)
                    {
                        if (loadData.gateListValue[i] == secondCreatedNodes.talent.GetSpecificName())
                        {
                            createdNodes.UpgradeGate(secondCreatedNodes.talent);
                        }
                    }
                }
            }
        }
        for (int i = 0; i < loadData.entangleListKey.Count; i++)
        {
            foreach (var createdNodes in orbits)
            {
                if (loadData.entangleListKey[i] == createdNodes.talent.GetSpecificName())
                {
                    foreach (var secondCreatedNodes in orbits)
                    {
                        if (loadData.entangleListValue[i] == secondCreatedNodes.talent.GetSpecificName())
                        {
                            createdNodes.UpgradeEntangle(secondCreatedNodes.talent);
                        }
                    }
                }
            }
        }

    }

    public string GetName()
    {
        return "TalentManager";
    }
}