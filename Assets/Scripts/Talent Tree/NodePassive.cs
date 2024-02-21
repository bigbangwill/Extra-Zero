using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum NodePurchaseState { IsPurchased, IsNotPurchased, IsSelectedPurchased, IsSelectedNotPurchased, IsCloseTarget, IsMenuPassive, IsMenuSelected}

public class NodePassive : MonoBehaviour
{
    private TalentLibrary talent;
    private bool isPurchased = false;
    public bool IsPurchased { get => isPurchased; }
    private Renderer rend;
    private NodePurchaseState currentState;


    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
    }

    public void UpgradeQubit()
    {
        talent.UpgradeToQubit();
        SetNodeState(currentState);
    }

    public void SetTalent(TalentLibrary talent)
    {
        this.talent = talent;
    }

    public void PurchaseTalent()
    {
        talent.TalentEffect();
        isPurchased = true;
        SetNodeState(NodePurchaseState.IsPurchased);
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

    public void SetNodeState(NodePurchaseState nodePurchase)
    {
        switch (nodePurchase)
        {
            case NodePurchaseState.IsPurchased:
                currentState = nodePurchase; SetColor(TalentManager.Instance.passivePurchased);break;
            case NodePurchaseState.IsNotPurchased:
                currentState = nodePurchase; SetColor(TalentManager.Instance.passiveUnpurchased);break;
            case NodePurchaseState.IsSelectedPurchased:
                currentState = nodePurchase; SetColor(TalentManager.Instance.selectedPurchased);break;
            case NodePurchaseState.IsSelectedNotPurchased:
                currentState = nodePurchase; SetColor(TalentManager.Instance.selectedUnpurchased);break;
            case NodePurchaseState.IsCloseTarget:
                currentState = nodePurchase; SetColor(TalentManager.Instance.closePurchasable);break;
            case NodePurchaseState.IsMenuPassive:
                currentState = nodePurchase; SetColor(TalentManager.Instance.menuPassive);break;
            case NodePurchaseState.IsMenuSelected:
                currentState = nodePurchase; SetColor(TalentManager.Instance.menuSelected);break;
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
