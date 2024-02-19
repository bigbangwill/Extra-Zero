using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Reflection;
using Unity.PlasticSCM.Editor.WebApi;

public class TalentManager : MonoBehaviour
{
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
    [SerializeField] private Color passivePurchased;
    [SerializeField] private Color passiveUnpurchased;
    [SerializeField] private Color selectedPurchased;
    [SerializeField] private Color selectedUnpurchased;
    [SerializeField] private Color closePurchasable;

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
    public void SetTargetNode(NodeMovement targetNode)
    {
        currentDistance.Clear();
        NodePassive targetPassive = targetNode.GetComponent<NodePassive>();
        bool isPurchased = targetPassive.IsPurchased;

        if(currentTargetedNode ==  null)
            currentTargetedNode = targetNode;


        if (isPurchaseMode)
        {
            foreach (var orbit in closeOrbits)
            {
                if (orbit == targetNode)
                {
                    infoPanel.SetActivePanel(targetPassive);
                    currentTargetedNode = targetNode;
                    targetNode.SetColor(selectedUnpurchased);
                    return;
                }
            }
        }
        if (targetNode.GetComponent<NodePassive>().IsPurchased)
        {
            targetNode.SetColor(passivePurchased);
        }
        else
        {
            targetNode.SetColor(passiveUnpurchased);
        }

        if (targetNode == currentTargetedNode)
        {
            // Selected the same
        }


        if (isPurchased)
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetColor(selectedPurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = true;
            infoPanel.SetActivePanel(targetPassive);
        }
        else
        {
            currentTargetedNode = targetNode;
            currentTargetedNode.SetColor(selectedUnpurchased);
            TargetClosest(currentTargetedNode);
            isPurchaseMode = false;
            infoPanel.SetActivePanel(targetPassive);
        }













        foreach (var orbit in closeOrbits)
        {
            if(orbit != null)
                orbit.SetColor(Color.blue);
        }
        if (targetNode == currentTargetedNode)
        {
            currentTargetedNode.SetColor(Color.blue);
            currentTargetedNode = null;
            return;
        }
        TargetClosest(targetNode);
        
        currentTargetedNode.SetColor(Color.green);
        closeOrbits[0].SetColor(Color.red);
        closeOrbits[1].SetColor(Color.red);
        closeOrbits[2].SetColor(Color.red);

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