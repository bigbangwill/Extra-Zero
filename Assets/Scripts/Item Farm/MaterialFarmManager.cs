using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.VisualScripting;

public class MaterialFarmManager : MonoBehaviour
{

    private List<MaterialItem> createdMaterialItems = new();
    private List<BasicMaterialScript> basicMaterialScripts = new();
    [SerializeField] private GameObject farmMaterialPrefab;
    [SerializeField] private Transform contentTransform;


    private PlayerInventory inventory;
    private NewTierManager tierManager;

    private MaterialFarmManagerRefrence refrence;

    private void SetSORefrence()
    {
        refrence = (MaterialFarmManagerRefrence)FindSORefrence<MaterialFarmManager>.FindScriptableObject("Materialfarm Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
        }
        refrence.val = this;
    }

    private void LoadSORefrence()
    {
        inventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
        tierManager = ((NewTierManagerRefrence)FindSORefrence<NewTierManager>.FindScriptableObject("New Tier Manager Refrence")).val;
    }

    private void Awake()
    {
        SetSORefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        Init();
        tierManager.TierChangeAddListener(TierChanged);
        TierChanged();
    }

    private void OnDestroy()
    {
        tierManager.TierChangeRemoveListener(TierChanged);
    }

    private void Init()
    {
        List<Type> materials = Assembly.GetAssembly(typeof(MaterialItem)).GetTypes().Where
                (TheType => TheType.IsClass && !TheType.IsAbstract &&
                TheType.IsSubclassOf(typeof(MaterialItem))).ToList();
        foreach (var material in materials)
        {
            MaterialItem item = (MaterialItem)Activator.CreateInstance(material);
            item.SetCurrentStack(1);
            createdMaterialItems.Add(item);
            GameObject target = Instantiate(farmMaterialPrefab, contentTransform);
            BasicMaterialScript basicMaterial = target.GetComponent<BasicMaterialScript>();
            basicMaterialScripts.Add(basicMaterial);
            basicMaterial.Init(item, inventory);
        }
    }    


    private void TierChanged()
    {
        int unlockedTier = tierManager.UnlockedTier;
        Debug.Log(unlockedTier + "HKJGQJLKHGTQ");
        foreach (var basic in basicMaterialScripts)
        {
            Debug.Log(basic.GetTier());
            if (basic.GetTier() <= unlockedTier)
            {
                basic.SetLockedState(false);
            }
            else
            {
                basic.SetLockedState(true);
            }
        }
    }



    public void StrenghtPotionBuff()
    {
        foreach (var item in basicMaterialScripts)
        {
            item.StrenghtPotionBuff();
        }
    }

    public void StrenghtPotionReset()
    {
        foreach (var item in basicMaterialScripts)
        {
            item.StrenghtPotionReset();
        }
    }
}