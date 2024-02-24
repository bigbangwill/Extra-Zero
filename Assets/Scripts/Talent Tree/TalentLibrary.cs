using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class TalentLibrary
{
    protected string specificName;
    protected Sprite talentIcon;
    protected int talentCost;
    protected string talentDescription;
    protected string talentDescriptionQubit;
    protected bool isQubit;
    protected bool isGated;
    protected bool isEntangled;

    protected NodePassive gatedNode;
    protected NodePassive entangledNode;

    public bool IsQubit { get => isQubit; }
    public bool IsGated { get => isGated; }
    public bool IsEntangled { get => isEntangled; }

    public void LoadIcon()
    {
        string address = "Talent Icon/" + specificName + "[Sprite]";
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(address);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            talentIcon = handle.Result;
            Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load the asset");
        }
    }

    public void TalentEffect()
    {
        if (!isQubit)
            NormalTalentEffect();
        else
            QubitTalentEffect();
    }

    protected abstract void NormalTalentEffect();

    protected abstract void QubitTalentEffect();


    public virtual void AddGateToQubit(NodePassive secondNode)
    {
        isGated = true;
        gatedNode = secondNode;
    }

    public virtual void RemoveGateFromQubit()
    {
        isGated = false;
        gatedNode = null;
    }

    public virtual void AddEntangleToQubit(NodePassive secondNode)
    {
        isEntangled = true;
        entangledNode = secondNode;
    }

    public virtual void RemoveEntangleToQubit()
    {
        isEntangled = false;
        entangledNode = null;
    }

    public virtual NodePassive TargetGatedNode()
    {
        return gatedNode;
    }
    public Sprite GetTalentIcon()
    {
        return talentIcon;
    }

    public string GetTalentDescription()
    {
        return talentDescription;
    }

    public int GetTalentCost()
    {
        return talentCost;
    }

    public string GetTalentDescriptionQubit()
    {
        return talentDescriptionQubit;
    }

    public virtual void DowngradeFromQubit()
    {
        isQubit = false;
    }

    public virtual void UpgradeToQubit()
    {
        isQubit = true;
    }

}

public class FirstTalent : TalentLibrary
{
    public FirstTalent()
    {
        specificName = "FirstTalent";
        talentCost = 10;
        talentDescription = "The first talent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }

    

    protected override void NormalTalentEffect()
    {
        Debug.Log("First effect impacted");
    }
    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }


}
public class SecondTalent : TalentLibrary
{
    public SecondTalent()
    {
        specificName = "SecondTalent";
        talentCost = 10;
        talentDescription = "The SecondTalent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }


    protected override void NormalTalentEffect()
    {
        Debug.Log("SecondTalent impacted");
    }
    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }
}
public class ThirdTalent : TalentLibrary
{
    public ThirdTalent()
    {
        specificName = "ThirdTalent";
        talentCost = 10;
        talentDescription = "The ThirdTalent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }


    protected override void NormalTalentEffect()
    {
        Debug.Log("ThirdTalent impacted");
    }

    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }
}
public class ForthTalent : TalentLibrary
{
    public ForthTalent()
    {
        specificName = "ForthTalent";
        talentCost = 10;
        talentDescription = "The ForthTalent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }

    protected override void NormalTalentEffect()
    {
        Debug.Log("ForthTalent impacted");
    }
    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }
}
public class FifthTalent : TalentLibrary
{
    public FifthTalent()
    {
        specificName = "FifthTalent";
        talentCost = 10;
        talentDescription = "The FifthTalent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }


    protected override void NormalTalentEffect()
    {
        Debug.Log("FifthTalent impacted");
    }
    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }
}
public class SixthTalent : TalentLibrary
{
    public SixthTalent()
    {
        specificName = "SixthTalent";
        talentCost = 10;
        talentDescription = "The SixthTalent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }


    protected override void NormalTalentEffect()
    {
        Debug.Log("SixthTalent impacted");
    }
    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }
}
public class SeventhTalent : TalentLibrary
{
    public SeventhTalent()
    {
        specificName = "SeventhTalent";
        talentCost = 10;
        talentDescription = "The SeventhTalent that is only for testing";
        talentDescriptionQubit = talentDescription + "QUBIT VERSION";
        LoadIcon();
    }


    protected override void NormalTalentEffect()
    {
        Debug.Log("SeventhTalent impacted");
    }
    protected override void QubitTalentEffect()
    {
        Debug.Log(specificName + " QubitEffect");
    }
}