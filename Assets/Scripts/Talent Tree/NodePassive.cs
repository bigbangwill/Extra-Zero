using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum NodePurchaseState { IsPurchase, IsNotPurchased, IsSelectedPurchased, IsSelectedNotPurchased, IsCloseTarget}

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



    public void SetTalent(TalentLibrary talent)
    {
        this.talent = talent;
    }

    public void PurchaseTalent()
    {
        talent.TalentEffect();
        isPurchased = true;
        SetNodeState(NodePurchaseState.IsPurchase);
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
            case NodePurchaseState.IsPurchase:
                SetColor(TalentManager.Instance.passivePurchased);break;
            case NodePurchaseState.IsNotPurchased:
                SetColor(TalentManager.Instance.passiveUnpurchased);break;
            case NodePurchaseState.IsSelectedPurchased:
                SetColor(TalentManager.Instance.selectedPurchased);break;
            case NodePurchaseState.IsSelectedNotPurchased:
                SetColor(TalentManager.Instance.selectedUnpurchased);break;
            case NodePurchaseState.IsCloseTarget:
                SetColor(TalentManager.Instance.closePurchasable);break;
            default: Debug.LogWarning("Check here Color");break;
        }
    }

    private void SetColor(Color inputColor)
    {
        if (rend == null)
            rend = GetComponentInChildren<Renderer>();
        rend.material.SetColor("_Base_Color", inputColor);
    }
}
