using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PrinterManager : MonoBehaviour
{

    [SerializeField] private Transform reaching;
    [SerializeField] private Transform quePrefabParent;
    [SerializeField] private GameObject quePrefab;
    [SerializeField] private ItemStash stash;

    private bool isCrafting;




    [Header("UI")]
    [SerializeField] private Slider ceramicSlider;
    [SerializeField] private Slider plasticSlider;
    [SerializeField] private Slider titaniumSlider;
    [SerializeField] private Slider alominumSlider;
    [SerializeField] private Slider stainlessSteelSldier;

    [SerializeField] private TextMeshProUGUI timer;

    [SerializeField] private Button restartButton;
    [SerializeField] private Button removeQueButton;


    #region Item Stats

    private int maxCeramic = 5;
    private int currentCeramic;

    private int maxPlastic = 5;
    private int currentPlastic;

    private int maxTitaniumAlloy = 5;
    private int currentTitaniumAlloy;

    private int maxAluminumAlloy = 5;
    private int currentAluminumAlloy;

    private int maxStainlessSteel = 5;
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

    #endregion


    public bool isActive = false;


    private Queue<BluePrintItem> craftingQueue = new();

    private float currentElapsedTimer;


    public float CurrentElapsedTimer { get => currentElapsedTimer; }


    private PlayerInventory playerInventory;
    private EventTextManager eventTextManager;

    private PrinterManagerRefrence refrence;


    private void SetSORefrence()
    {
        refrence = (PrinterManagerRefrence)FindSORefrence<PrinterManager>.FindScriptableObject("Printer Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetSORefrence();
    }

    public void RefreshUI()
    {
        ceramicSlider.maxValue = MaxCeramic;
        plasticSlider.maxValue = MaxPlastic;
        titaniumSlider.maxValue = MaxTitaniumAlloy;
        alominumSlider.maxValue = MaxAluminumAlloy;
        stainlessSteelSldier.maxValue = MaxStainlessSteel;

        ceramicSlider.value = CurrentCeramic;
        plasticSlider.value = CurrentPlastic;
        titaniumSlider.value = CurrentTitaniumAlloy;
        alominumSlider.value = CurrentAluminumAlloy;
        stainlessSteelSldier.value = CurrentStainlessSteel;

        int timerValue = (int)CurrentElapsedTimer;
        timer.text = timerValue.ToString();

        if (!isCrafting)
        {
            restartButton.interactable = true;
            removeQueButton.interactable = true;
        }

    }


    private void LoadSORefrence()
    {
        playerInventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
    }

    private void Start()
    {
        LoadSORefrence();
    }

    private void Update()
    {
        if (isActive && craftingQueue.Count > 0)
        {
            timer.text = ((int)CurrentElapsedTimer).ToString();
        }
    }


    public void AddMaterial(MaterialItem item, ItemSlotUI slotUI)
    {        
        if (item.Equals(new MaterialItem.Ceramic()))
        {
            AddMaterial(ref currentCeramic, ref maxCeramic, item, slotUI);
        }
        else if (item.Equals(new MaterialItem.Plastic()))
        {
            AddMaterial(ref currentPlastic, ref maxPlastic, item, slotUI);
        }
        else if (item.Equals(new MaterialItem.StainlessSteel()))
        {
            AddMaterial(ref currentStainlessSteel, ref maxStainlessSteel, item, slotUI);
        }
        else if (item.Equals(new MaterialItem.AluminumAlloy()))
        {
            AddMaterial(ref currentAluminumAlloy, ref maxAluminumAlloy, item, slotUI);
        }
        else if (item.Equals(new MaterialItem.TitaniumAlloy()))
        {
            AddMaterial(ref currentTitaniumAlloy, ref maxTitaniumAlloy, item, slotUI);
        }
    }

    private void AddMaterial(ref int current, ref int max, MaterialItem item, ItemSlotUI slotUI)
    {
        int itemStack = item.CurrentStack();
        int leftToFill = max - current;
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


    public void SentFromComputer(BluePrintItem targetItem)
    {
        craftingQueue.Enqueue(targetItem);
        GameObject go = Instantiate(quePrefab, quePrefabParent);
        go.GetComponent<Image>().sprite = targetItem.CraftedItemReference().IconRefrence();
        Debug.Log(isCrafting);
        Debug.Log(stash.HaveEmptySlot(targetItem.CraftedItemReference(), false));
        Debug.Log(CheckMaterial(targetItem));
        if (!isCrafting && stash.HaveEmptySlot(targetItem.CraftedItemReference(),false) && CheckMaterial(targetItem))
        {
            StartCoroutine(StartCrafting());
            restartButton.interactable = false;
            removeQueButton.interactable = false;
        }
        else
        {
            eventTextManager.CreateNewText("Stopped Crafting", TextType.Error);
            restartButton.interactable = true;
            removeQueButton.interactable = true;
        }
    }

    public void ReStartCrafting()
    {
        if (craftingQueue.Count <= 0)
        {
            eventTextManager.CreateNewText("Nothing To Create", TextType.Error);
            return;
        }
        BluePrintItem targetItem = craftingQueue.Peek();        
        if (!isCrafting && stash.HaveEmptySlot(targetItem.CraftedItemReference(), false) && CheckMaterial(targetItem))
        {
            restartButton.interactable = true;
            removeQueButton.interactable = true;
            StartCoroutine(StartCrafting());
        }
        else
        {
            eventTextManager.CreateNewText("Stopped Crafting", TextType.Error);
        }
    }

    public void CancelQue()
    {
        foreach (Transform child in quePrefabParent)
        {
            Destroy(child.gameObject);
            craftingQueue.Clear();
        }
    }


    private bool CheckMaterial(BluePrintItem item)
    {
        List<Action> toReduce = new();
        foreach (var material in item.materialsList)
        {
            if (material.Equals(new MaterialItem.Ceramic()) && material.CurrentStack() <= currentCeramic)
            {
                toReduce.Add(() => currentCeramic -= material.CurrentStack());
            }
            else if (material.Equals(new MaterialItem.Plastic()) && material.CurrentStack() <= currentPlastic)
            {
                toReduce.Add(() =>currentPlastic -= material.CurrentStack());
                Debug.Log("1");
            }
            else if (material.Equals(new MaterialItem.StainlessSteel()) && material.CurrentStack() <= currentStainlessSteel)
            {
                toReduce.Add(() => currentStainlessSteel -= material.CurrentStack());
            }
            else if (material.Equals(new MaterialItem.AluminumAlloy()) && material.CurrentStack() <= currentAluminumAlloy)
            {
                toReduce.Add(() => currentAluminumAlloy -= material.CurrentStack());
            }
            else if (material.Equals(new MaterialItem.TitaniumAlloy()) && material.CurrentStack() <= currentTitaniumAlloy)
            {
                toReduce.Add(() => currentTitaniumAlloy -= material.CurrentStack());
            }
            else
            {
                return false;
            }
        }
        foreach (var action in toReduce)
        {
            action();
        }
        return true;
    }


    private IEnumerator StartCrafting()
    {
        while (true)
        {
            BluePrintItem first = craftingQueue.Peek();
            float craftTime = first.CraftTimer();
            float elapsedTime = 0f;
            isCrafting = true;

            while (elapsedTime < craftTime)
            {
                // Update the string with the elapsed time
                UpdateCraftingTimer(craftTime - elapsedTime);

                // Wait for the next frame
                yield return null;

                // Increment the elapsed time
                elapsedTime += Time.deltaTime;
            }
            // Ensure the timer shows the full craft time at the end
            UpdateCraftingTimer(craftTime);
            Crafted();
            currentElapsedTimer = 0;
            isCrafting = false;
            yield break;
        }
    }

    private void UpdateCraftingTimer(float elapsedTime)
    {
        currentElapsedTimer = elapsedTime;
    }


    private void Crafted()
    {
        BluePrintItem target = craftingQueue.Dequeue();
        Destroy(quePrefabParent.GetChild(0).gameObject);
        eventTextManager.CreateNewText("Crafted " + target.GetName(), TextType.Information);
        stash.HaveEmptySlot(target.CraftedItemReference(),true);
    }

    public Transform GetReachingTransform()
    {
        return reaching;
    }
}
