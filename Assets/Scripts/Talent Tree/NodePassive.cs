using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NodePassive : MonoBehaviour
{
    private TalentLibrary talent;
    private bool isPurchased = false;
    public bool IsPurchased { get => isPurchased; }


    public void SetTalent(TalentLibrary talent)
    {
        this.talent = talent;
    }

    public void PurchaseTalent()
    {
        talent.TalentEffect();
        isPurchased = true;
        TalentManager.Instance.SetColor(this, GetComponent<NodeMovement>());
    }

    public int GetTalentCost()
    {
        return talent.GetTalentCost();
    }

    public string GetTalentDescription()
    {
        return talent.GetTalentDescription();
    }

}
