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

    protected TalentManager talentManager;

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

    public void SetTalentManagerRefrence(TalentManager talentManager)
    {
        this.talentManager = talentManager;
    }

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

    public virtual NodePassive TargetEntangledNode()
    {
        return entangledNode;
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

public class ScannerSlotUnlock : TalentLibrary
{
    public ScannerSlotUnlock()
    {
        specificName = "ScannerSlotUnlock";
        talentCost = 10;
        talentDescription = "This talent unlocks one extra slot in the Scanner Machine";
        talentDescriptionQubit = "The Qubit version will unlock another slot";
        LoadIcon();
    }

    
    //REMEMBER : to set up the talents in a way that will be reseted back to normal after scene changes.

    protected override void NormalTalentEffect()
    {
        ((ScannerSlotManagerRefrence)FindSORefrence<ScannerSlotManager>.FindScriptableObject("Scanner Slot Manager Refrence")).val.UpgradeOrbit(false);
    }
    protected override void QubitTalentEffect()
    {
        ((ScannerSlotManagerRefrence)FindSORefrence<ScannerSlotManager>.FindScriptableObject("Scanner Slot Manager Refrence")).val.UpgradeOrbit(true);
    }


}
public class SlotReaderMiniGameUpgrade : TalentLibrary
{
    public SlotReaderMiniGameUpgrade()
    {
        specificName = "SlotReaderMiniGameUpgrade";
        talentCost = 10;
        talentDescription = "This talent will make the roller in the scanner slot to performe better";
        talentDescriptionQubit = "The qubit version will be work ALOT better :)";
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