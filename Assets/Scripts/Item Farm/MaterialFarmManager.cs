using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using System.Net.NetworkInformation;

public class MaterialFarmManager : MonoBehaviour
{

    private List<MaterialItem> createdMaterialItems = new List<MaterialItem>();
    [SerializeField] private GameObject farmMaterialPrefab;
    [SerializeField] private Transform contentTransform;


    private PlayerInventory inventory;

    private void LoadSORefrence()
    {
        inventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
    }

    private void Start()
    {
        LoadSORefrence();
        Init();
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
            basicMaterial.Init(item, inventory);
        }

    }    
}