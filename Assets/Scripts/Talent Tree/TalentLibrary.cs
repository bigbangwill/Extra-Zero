using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class TalentLibrary
{
    protected string specificName;
    protected Sprite talentIcon;
    protected int talentCost;
    protected string talentDescription;

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
    public abstract void TalentEffect();
    public Sprite GetTalentIcon()
    {
        return talentIcon;
    }
}

public class FirstTalent : TalentLibrary
{
    public FirstTalent()
    {
        specificName = "FirstTalent";
        talentCost = 10;
        talentDescription = "The first talent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("First effect impacted");
    }
}
public class SecondTalent : TalentLibrary
{
    public SecondTalent()
    {
        specificName = "SecondTalent";
        talentCost = 10;
        talentDescription = "The SecondTalent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("SecondTalent impacted");
    }
}
public class ThirdTalent : TalentLibrary
{
    public ThirdTalent()
    {
        specificName = "ThirdTalent";
        talentCost = 10;
        talentDescription = "The ThirdTalent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("ThirdTalent impacted");
    }
}
public class ForthTalent : TalentLibrary
{
    public ForthTalent()
    {
        specificName = "ForthTalent";
        talentCost = 10;
        talentDescription = "The ForthTalent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("ForthTalent impacted");
    }
}
public class FifthTalent : TalentLibrary
{
    public FifthTalent()
    {
        specificName = "FifthTalent";
        talentCost = 10;
        talentDescription = "The FifthTalent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("FifthTalent impacted");
    }
}
public class SixthTalent : TalentLibrary
{
    public SixthTalent()
    {
        specificName = "SixthTalent";
        talentCost = 10;
        talentDescription = "The SixthTalent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("SixthTalent impacted");
    }
}
public class SeventhTalent : TalentLibrary
{
    public SeventhTalent()
    {
        specificName = "SeventhTalent";
        talentCost = 10;
        talentDescription = "The SeventhTalent that is only for testing";
        LoadIcon();
    }

    public override void TalentEffect()
    {
        Debug.Log("SeventhTalent impacted");
    }
}