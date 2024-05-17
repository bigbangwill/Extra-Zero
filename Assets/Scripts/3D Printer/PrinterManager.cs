using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PrinterManager : MonoBehaviour
{

    [SerializeField] private Transform reaching;




    private int maxCeramic = 50;
    private int currentCeramic;

    private int maxPlastic = 50;
    private int currentPlastic;

    private int maxTitaniumAlloy = 50;
    private int currentTitaniumAlloy;

    private int maxAluminumAlloy = 50;
    private int currentAluminumAlloy;

    private int maxStainlessSteel = 50;
    private int currentStainlessSteel;

    public int MaxCeramic { get => maxCeramic; }
    public int CurrentCeramic { get => currentCeramic; set => currentCeramic = value; }
    public int MaxPlastic { get => maxPlastic; }
    public int CurrentPlastic { get => currentPlastic; set => currentPlastic = value; }
    public int MaxTitaniumAlloy { get => maxTitaniumAlloy; }
    public int CurrentTitaniumAlloy { get => currentTitaniumAlloy; set => currentTitaniumAlloy = value; }
    public int MaxAluminumAlloy { get => maxAluminumAlloy; }
    public int CurrentAluminumAlloy { get => currentAluminumAlloy; set => currentAluminumAlloy = value; }
    public int MaxStainlessSteel { get => maxStainlessSteel; }
    public int CurrentStainlessSteel { get => currentStainlessSteel; set => currentStainlessSteel = value; }


    private PlayerInventory playerInventory;


    private void LoadSORefrence()
    {
        playerInventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
    }

    private void Start()
    {
        LoadSORefrence();
    }



    public void AddMaterial(MaterialItem item, ItemSlotUI slotUI)
    {        
        if (item.Equals(new MaterialItem.Ceramic()))
        {
            AddMaterial(ref currentCeramic, ref maxCeramic, item, slotUI);
            Debug.Log("1");
        }
        else if (item.Equals(new MaterialItem.Plastic()))
        {
            AddMaterial(ref currentPlastic, ref maxPlastic, item, slotUI);
            Debug.Log("2");
        }
        else if (item.Equals(new MaterialItem.StainlessSteel()))
        {
            AddMaterial(ref currentStainlessSteel, ref maxStainlessSteel, item, slotUI);
            Debug.Log("3");
        }
        else if (item.Equals(new MaterialItem.AluminumAlloy()))
        {
            AddMaterial(ref currentAluminumAlloy, ref maxAluminumAlloy, item, slotUI);
            Debug.Log("4");
        }
        else if (item.Equals(new MaterialItem.TitaniumAlloy()))
        {
            AddMaterial(ref currentTitaniumAlloy, ref maxTitaniumAlloy, item, slotUI);
            Debug.Log("5");
        }
    }



    private void AddMaterial(ref int current, ref int max, MaterialItem item, ItemSlotUI slotUI)
    {
        int itemStack = item.CurrentStack();
        int leftToFill = current - max;
        if (itemStack >= leftToFill && leftToFill > 0)
        {
            item.SetCurrentStack(item.CurrentStack() - leftToFill);
            current = max;
            slotUI.RefreshText();
        }
        else if (itemStack < leftToFill && leftToFill > 0)
        {
            current += itemStack;
            playerInventory.RemoveItemFromInventory(slotUI.slotNumber);
        }
    }


    public Transform GetReachingTransform()
    {
        return reaching;
    }



}
