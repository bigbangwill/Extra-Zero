using System.Collections;
using System.Collections.Generic;
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

    private Queue<BluePrintItem> craftingQueue = new();

    private float currentElapsedTimer;


    public float CurrentElapsedTimer { get => currentElapsedTimer; }


    private PlayerInventory playerInventory;

    private EventTextManager eventTextManager;



    private void LoadSORefrence()
    {
        playerInventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
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
        if (!isCrafting && stash.HaveEmptySlot(targetItem.CraftedItemReference(),false))
        {
            StartCoroutine(StartCrafting());
        }

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
                UpdateCraftingTimer(elapsedTime);

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
