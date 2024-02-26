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
    private TalentLibrary talent;
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

    public void UpgradeGate(NodePassive targetNode)
    {
        talent.AddGateToQubit(targetNode);
        gateToNode = targetNode;
    }

    public void DowngradeGate()
    {
        talent.RemoveGateFromQubit();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(this);
        gateToNode = null;
    }

    public void ForceUpgradeGate(NodePassive targetNode)
    {
        talent.AddGateToQubit(targetNode);
        gateToNode = targetNode;
    }

    public void ForceDowngradeGate()
    {
        talent.RemoveGateFromQubit();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(this);
        gateToNode = null;
    }

    public void UpgradeEntangle(NodePassive targetNode)
    {
        talent.AddEntangleToQubit(targetNode);
        targetNode.ForceUpgradeEntangle(this);
        entangleToNode = targetNode;
    }

    public void DowngradeEntangle()
    {
        talent.RemoveEntangleToQubit();
        entangleToNode.ForceDowngradeEntangle();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(this);
        entangleToNode = null;
    }

    public void ForceUpgradeEntangle(NodePassive targetNode)
    {
        talent.AddEntangleToQubit(targetNode);
        entangleToNode = targetNode;
    }

    public void ForceDowngradeEntangle()
    {
        talent.RemoveEntangleToQubit();
        SetNodeState(NodePurchaseState.IsMenuPassive);
        talentManagerRefrence.val.TryStopLine(this);
        entangleToNode = null;
    }

    public void SetTalent(TalentLibrary talent)
    {
        this.talent = talent;
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

    public void SetNodeState(NodePurchaseState nodePurchase)
    {
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
