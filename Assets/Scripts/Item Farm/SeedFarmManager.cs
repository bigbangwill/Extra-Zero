using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class SeedFarmManager : MonoBehaviour
{

    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private Transform bonusHolder;

    [SerializeField] private int currentBonusChance;
    [SerializeField] private List<Transform> bonusLocations = new List<Transform>();

    [SerializeField] private float bonusMaxTimer;

    private int maxBonusChance = 101;

    private float tickRate = 0.5f;
    private float currentTimer;

    private bool shouldCount = false;

    private List<Bonus> bonusList = new List<Bonus>();
    
    

    private PlayerInventory inventory;
    private void LoadSoRefrence()
    {
        inventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
    }

    private void Start()
    {
        LoadSoRefrence();

        List<Type> bonuses = Assembly.GetAssembly(typeof(Bonus)).GetTypes().Where
                (TheType => TheType.IsClass && !TheType.IsAbstract &&
                TheType.IsSubclassOf(typeof(Bonus))).ToList();
        foreach(var bonus in bonuses)
        {
            bonusList.Add((Bonus)Activator.CreateInstance(bonus));
        }
    }

    private void OnEnable()
    {
        shouldCount = true;
    }


    private void Update()
    {
        if (shouldCount)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= tickRate)
            {
                CalculateBonusChance();
                currentTimer = 0;
            }
        }
    }

    private void OnDisable()
    {
        shouldCount = false;
    }


    public void HitWithSeed(Seed seed)
    {
        if (inventory.HaveEmptySlot(seed, true))
        {
            Debug.Log("Added Seed" + seed.GetName() + "Current Stack" + seed.CurrentStack());
        }
        else
        {
            Debug.Log("Not enough space");
        }
    }

    private void CalculateBonusChance()
    {
        int chance = UnityEngine.Random.Range(0, maxBonusChance);
        if (chance <= currentBonusChance)
        {
            SummonBonus();
        }
    }

    private void SummonBonus()
    {
        Bonus targetBonus = bonusList[UnityEngine.Random.Range(0, bonusList.Count)];
        GameObject go = Instantiate(bonusPrefab,bonusHolder);
        go.transform.position = bonusLocations[UnityEngine.Random.Range(0, bonusLocations.Count)].position;
        go.GetComponent<BonusPrefabScript>().SetAction(targetBonus.ApplyEffect,targetBonus.GetIconRefrence(), bonusMaxTimer);
    }

    public abstract class Bonus
    {
        protected Sprite icon;
        protected string iconAddress;

        public abstract void ApplyEffect(Seed seed);

        public Sprite GetIconRefrence()
        {
            return icon;
        }

        protected void LoadIcon()
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(iconAddress);
            handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                icon = handle.Result;
                Addressables.Release(handle);
            }
            else
            {
                Debug.LogError("Failed to load the asset");
            }
        }


        public class PlusOne : Bonus
        {            
            public PlusOne()
            {
                iconAddress = "Seed Icons/Bonus Plus one";
                LoadIcon();
            }

            public override void ApplyEffect(Seed targetSeed)
            {
                int stack = targetSeed.CurrentStack();
                int newStack = stack++;
                if (newStack > targetSeed.MaxStack())
                    newStack = targetSeed.MaxStack();
                targetSeed.SetCurrentStack(newStack);
                Debug.Log("Plus one");
            }
        }

        public class MultiplyTwo : Bonus
        {
            public MultiplyTwo()
            {
                iconAddress = "Seed Icons/Multiply Two";
                LoadIcon();
            }
            public override void ApplyEffect(Seed targetSeed)
            {
                int stack = targetSeed.CurrentStack();
                int newStack = stack * 2;
                if (newStack > targetSeed.MaxStack())
                    newStack = targetSeed.MaxStack();
                targetSeed.SetCurrentStack(newStack);
                Debug.Log("Multiple 2");
            }
        }
    }
}