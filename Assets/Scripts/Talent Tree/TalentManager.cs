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
    private List<NodeMovement> orbits = new();

    private NodeMovement currentTargetedNode;

    private NodeMovement[] closeOrbits = new NodeMovement[3];

    private Dictionary<NodeMovement, float> currentDistance = new();

    [SerializeField] private Transform talentInfoPanel;
    private bool isPurchasable = false;


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
                orbits.Add(orbit.GetComponent<NodeMovement>());
            }
        }
    }


    private bool isPurchaseMode = false;

    public void SetColor(NodePassive passive, NodeMovement movement)
    {
        if (passive.IsPurchased)
            movement.SetColor(passivePurchased);
        else
            movement.SetColor(passiveUnpurchased);
    }

    public void SetTargetNode(NodeMovement targetNode)
    {
        currentDistance.Clear();
        NodePassive targetPassive = targetNode.GetComponent<NodePassive>();
        bool isTargetPurchased = targetPassive.IsPurchased;

        if(currentTargetedNode != null)
            SetColor(currentTargetedNode.GetComponent<NodePassive>(),currentTargetedNode);

        //DO ANYTHING THAT IS RELATED TO THE PREVIUS NODE HERE BEFORE THE NEXT SET OF THE NODE.
        
        if (isPurchaseMode)
        {
            foreach (var orbit in closeOrbits)
            {
                if (orbit == targetNode)
                {
                    infoPanel.SetActivePanel(targetPassive);
                    targetNode.SetColor(selectedUnpurchased);
                    return;
                }
            }
        }

        if (targetNode == currentTargetedNode)
        {
            Debug.Log("Hereee");
            SetColor(targetPassive, targetNode);
            foreach (var orbit in closeOrbits)
            {
                NodePassive passive = orbit.GetComponent<NodePassive>();
                SetColor(passive,orbit);
            }
            currentTargetedNode = null;
            closeOrbits = new NodeMovement[3];
            return;
        }


        if (isTargetPurchased)
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetColor(selectedPurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = true;
            infoPanel.SetActivePanel(targetPassive);
            foreach (var orbit in closeOrbits)
            {
                orbit.SetColor(closePurchasable);
            }
        }
        else
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetColor(selectedUnpurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = false;
            infoPanel.SetActivePanel(targetPassive);
        }

    }

    private void TargetClosest(NodeMovement targetNode)
    {
        closeOrbits = new NodeMovement[3];
        if (currentTargetedNode != null)
            currentTargetedNode.SetColor(Color.blue);
        currentTargetedNode = targetNode;
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