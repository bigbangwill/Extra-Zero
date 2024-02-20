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
    private bool isPurchasable = false;

    private NodePassive purchaseMainNode = null;


    [Header("Color Setting")]
    public Color passivePurchased;
    public Color passiveUnpurchased;
    public Color selectedPurchased;
    public Color selectedUnpurchased;
    public Color closePurchasable;

    private void Start()
    {
        CreateNodes();
        GetAllOrbs();
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
        orbits[0].PurchaseTalent();
    }


    private bool isPurchaseMode = false;


    public void SetColor(NodePassive passive)
    {
        if (passive.IsPurchased)
            passive.SetNodeState(NodePurchaseState.IsPurchase);
        else
            passive.SetNodeState(NodePurchaseState.IsNotPurchased);
    }

    public void SetTargetNode(NodePassive targetNode)
    {
        currentDistance.Clear();
        
        bool isTargetPurchased = targetNode.IsPurchased;
        
        if (currentTargetedNode != null)
            SetColor(currentTargetedNode);

        //DO ANYTHING THAT IS RELATED TO THE PREVIUS NODE HERE BEFORE THE NEXT SET OF THE NODE.
        
        if (isPurchaseMode)
        {
            if (targetNode == purchaseMainNode)
            {
                isPurchaseMode = false;
                SetColor(purchaseMainNode);
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
                    infoPanel.SetActivePanel(targetNode,!targetNode.IsPurchased);
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
            return;
        }


        if (isTargetPurchased)
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetNodeState(NodePurchaseState.IsSelectedPurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = true;
            purchaseMainNode = targetNode;
            infoPanel.SetActivePanel(targetNode,false);
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
            infoPanel.SetActivePanel(targetNode,false);
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
        SetColor(targetNode);
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
                SetColor(orbit);
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