using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ButtonTestInventory : MonoBehaviour
{
    public int count;
    public bool shouldRemove;

    private void Start()
    {
        Debug.Log(UnityEngine.Rendering.GraphicsSettings.renderPipelineAsset.GetType().Name);
    }
    public void AddWalkingStick()
    {
        ItemBehaviour item = new BluePrintItem.WalkingStick();
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddHoe()
    {
        ItemBehaviour item = new BluePrintItem.Hoe();
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddGun()
    {
        ItemBehaviour item = new BluePrintItem.Gun();
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddPlant()
    {
        ItemBehaviour item = new BluePrintItem.Plant();
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddPlastic()
    {
        ItemBehaviour item = new MaterialItem.Plastic(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddCeramic()
    {
        ItemBehaviour item = new MaterialItem.Ceramic(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddAluminum()
    {
        ItemBehaviour item = new MaterialItem.AluminumAlloy(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddStainlessSteel()
    {
        ItemBehaviour item = new MaterialItem.StainlessSteel(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddTitanium()
    {
        ItemBehaviour item = new MaterialItem.TitaniumAlloy(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddSeedSage()
    {
        ItemBehaviour item = new Seed.Sage(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddSeedLavender()
    {
        ItemBehaviour item = new Seed.Lavender(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void AddSeedChamomile()
    {
        ItemBehaviour item = new Seed.Chamomile(count);
        if (!shouldRemove)
            Debug.Log(PlayerInventory.Instance.HaveEmptySlot(item, true));
        else
            Debug.Log(PlayerInventory.Instance.HaveItemInInventory(item, true));
    }

    public void Save()
    {
        SaveClassManager.Instance.SaveCurrentState();
        Debug.Log("Save called");
    }

    public void Load()
    {
        SaveClassManager.Instance.LoadSavedGame();
        Debug.Log("Load Called");
    }
    
}