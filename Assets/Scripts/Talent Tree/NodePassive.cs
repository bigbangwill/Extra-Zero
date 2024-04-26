using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum NodePurchaseState { IsPurchased, IsNotPurchased, IsSelectedPurchased,
    IsSelectedNotPurchased, IsCloseTarget, IsMenuPassive, IsMenuSelected,
    IsMenuAvailable, IsMenuGated, IsMenuEntangled}

public class NodePassive : MonoBehaviour
{
    public TalentLibrary talent;
    private bool isPurchased = false;
    public bool IsPurchased { get => isPurchased; }
    private Renderer rend;
    private NodePurchaseState currentState;
    public NodePurchaseState CurrentState { get => currentState;}

    private NodePassive gateToNode;
    private NodePassive entangleToNode;

    private TalentManagerRefrence talentManagerRefrence;

    private void LoadSORefrence()
    {
        talentManagerRefrence = (TalentManagerRefrence)FindSORefrence<TalentManager>.FindScriptableObject("Talent Manager Refrence");

    }


    private void Start()
    {

        LoadSORefrence();
        rend = GetComponentInChildren<Renderer>();
    }

    public void SetTalentManagerRefrence(TalentManagerRefrence refrence)
    {
        talentManagerRefrence = refrence;
    }

    public void UpgradeQubit()
    {
        talent.UpgradeToQubit();
        SetNodeState(currentState);
    }

    public void DowngradeQubit()
    {
        talent.DowngradeFromQubit();
        SetNodeState(currentState);
    }

    public void UpgradeGate(TalentLibrary targetNode)
    {
        talent.AddGateToQubit(targetNode);
        gateToNode = targetNode.GetRelatedNodePassive();
        talentManagerRefrence.val.StartGateLine(
            transform.GetChild(0), 
            targetNode.GetRelatedNodePassive().transform.GetChild(0),
            talent);

    }

    public void DowngradeGate()
    {
        talent.RemoveGateFromQubit();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(talent);
        gateToNode = null;
    }

    public void ForceUpgradeGate(TalentLibrary targetNode)
    {
        talent.AddGateToQubit(targetNode);
        gateToNode = targetNode.GetRelatedNodePassive();
    }

    public void ForceDowngradeGate()
    {
        talent.RemoveGateFromQubit();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(talent);
        gateToNode = null;
    }

    public void UpgradeEntangle(TalentLibrary targetNode)
    {
        talent.AddEntangleToQubit(targetNode);
        targetNode.GetRelatedNodePassive().ForceUpgradeEntangle(talent);
        talentManagerRefrence.val.StartEntangleLine(
            transform.GetChild(0),
            targetNode.GetRelatedNodePassive().transform.GetChild(0),
            talent);
        entangleToNode = targetNode.GetRelatedNodePassive();

    }

    public void DowngradeEntangle()
    {
        talent.RemoveEntangleToQubit();
        entangleToNode.ForceDowngradeEntangle();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(talent);
        entangleToNode = null;
    }

    public void ForceUpgradeEntangle(TalentLibrary targetNode)
    {
        talent.AddEntangleToQubit(targetNode);
        entangleToNode = targetNode.GetRelatedNodePassive();
    }

    public void ForceDowngradeEntangle()
    {
        talent.RemoveEntangleToQubit();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(talent);
        entangleToNode = null;
    }

    public void SetTalent(TalentLibrary talent)
    {
        this.talent = talent;
        //talent.SetConnectedNode(this);
    }

    public void ApplyTalentState()
    {
        if (talent.IsQubit)
        {
            UpgradeQubit();
        }
        if (talent.IsEntangled)
        {
            UpgradeEntangle(talent.TargetEntangledNode().talent);
        }
        if (talent.IsGated)
        {
            UpgradeGate(talent.TargetGatedNode().talent);
        }
    }

    public void PurchaseTalent()
    {
        talent.TalentEffect();
        if (IsEntangled())
        {
            entangleToNode.ForcePurchaseTalent();
        }
        isPurchased = true;
        SetNodeState(NodePurchaseState.IsPurchased);
    }

    public void ResetToDefault()
    {
        isPurchased = false;
    }


    public void ForcePurchaseTalent()
    {
        talent.TalentEffect();
        isPurchased = true;
        SetNodeState(NodePurchaseState.IsPurchased);
    }

    public NodePassive GetNodeToGate()
    {
        return gateToNode;
    }

    public NodePassive GetNodeToEntangle()
    {
        return entangleToNode;
    }

    public int GetTalentCost()
    {
        return talent.GetTalentCost();
    }

    public string GetTalentDescription()
    {
        return talent.GetTalentDescription();
    }

    public Sprite GetTalentIcon()
    {
        return talent.GetTalentIcon();
    }

    public bool IsQubit()
    {
        return talent.IsQubit;
    }

    public bool IsGated()
    {
        return talent.IsGated;
    }

    public bool IsEntangled()
    {
        return talent.IsEntangled;
    }

    public string GetTalentDescriptionQubit()
    {
        return talent.GetTalentDescriptionQubit();
    }

    public string GetTalentName()
    {
        return talent.GetSpecificName();
    }

    public void SetNodeState(NodePurchaseState nodePurchase)
    {
        LoadSORefrence();
        switch (nodePurchase)
        {
            case NodePurchaseState.IsPurchased:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.passivePurchased);break;
            case NodePurchaseState.IsNotPurchased:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.passiveUnpurchased);break;
            case NodePurchaseState.IsSelectedPurchased:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.selectedPurchased);break;
            case NodePurchaseState.IsSelectedNotPurchased:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.selectedUnpurchased);break;
            case NodePurchaseState.IsCloseTarget:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.closePurchasable);break;
            case NodePurchaseState.IsMenuPassive:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.menuPassive);break;
            case NodePurchaseState.IsMenuSelected:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.menuSelected); break;
            case NodePurchaseState.IsMenuAvailable:
                currentState = nodePurchase; SetColor(  talentManagerRefrence.val.MenuAvailable);break;
            case NodePurchaseState.IsMenuGated:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.menuGated);break;
            case NodePurchaseState.IsMenuEntangled:
                currentState = nodePurchase; SetColor(talentManagerRefrence.val.menuEntangled);break;
            default: Debug.LogWarning("Check here Color");break;
        }
    }

    private void SetColor(Color inputColor)
    {
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
        rend.material.SetColor("_Base_Color", inputColor);
        if (talent.IsQubit)
        {
            rend.material.SetInt("_IsQubit", 1);
        }
        else
        {
            rend.material.SetInt("_IsQubit", 0);
        }
    }
}
