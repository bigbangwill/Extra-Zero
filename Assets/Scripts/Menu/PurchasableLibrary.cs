using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public abstract class PurchasableLibrary
{
    protected int cost;
    protected Sprite icon;
    protected string specificAddress;
    protected string purchasableDescription;

    public int Cost { get => cost; }
    public Sprite Icon { get => icon; }
    public string PurchasableDescription { get => purchasableDescription; }

    protected OptionHolderRefrence optionHolderRefrence;

    private void LoadSORefrence()
    {
        optionHolderRefrence = (OptionHolderRefrence)FindSORefrence<OptionHolder>.FindScriptableObject("Option Holder Refrence");
    }
    public abstract void purchasedMethod();

    protected void LoadIcon()
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(specificAddress);
        handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            icon = handle.Result;
            //Addressables.Release(handle);
        }
        else
        {
            Debug.LogError("Failed to load the asset");
        }
        LoadSORefrence();
    }
}

public class Qubit : PurchasableLibrary
{
    public Qubit()
    {
        cost = 2;
        specificAddress = "Purchasable/Qubit[Sprite]";
        purchasableDescription = "THIS IS QUBIT";
        LoadIcon();
    }

    public override void purchasedMethod()
    {
        optionHolderRefrence.val.AddQubitMax(+1);
    }
}
public class Gate : PurchasableLibrary
{

    public Gate()
    {
        cost = 2;
        specificAddress = "Purchasable/Gate[Sprite]";
        purchasableDescription = "THIS IS GATE";
        LoadIcon();
    }

    public override void purchasedMethod()
    {
        optionHolderRefrence.val.AddGateMax(+1);
    }
}
public class Entangle : PurchasableLibrary
{
    public Entangle()
    {
        cost = 2;
        specificAddress = "Purchasable/Entangle[Sprite]";
        purchasableDescription = "THIS IS Entangle";
        LoadIcon();
    }

    public override void purchasedMethod()
    {
        optionHolderRefrence.val.AddEntangleMax(+1);
    }
}