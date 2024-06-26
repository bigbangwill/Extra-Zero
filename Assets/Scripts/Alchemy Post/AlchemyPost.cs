using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class AlchemyPost : MonoBehaviour
{
    [SerializeField] private AlchemyHerbsPopUp popUp;

    [SerializeField] private AlchemyMiniGame miniGame;

    [SerializeField] private EffectSlots firstSlot;
    [SerializeField] private EffectSlots secondSlot;
    [SerializeField] private EffectSlots thirdSlot;

    [SerializeField] private List<AlchemySlots> slotsList;

    private List<Herb> inventoryHerbs;
    private List<string> herbNames;
    private List<Herb> sendingHerbs;

    private bool minigameIsPlaying = false;


    [SerializeField] private int potionCreationHerbCount = 5;


    private AlchemyPostRefrence refrence;
    private PlayerInventoryRefrence inventoryRefrence;
    private EventTextManager eventTextManager;
    private PlayerInventory playerInventory;

    private void SetRefrence()
    {
        refrence = (AlchemyPostRefrence)FindSORefrence<AlchemyPost>.FindScriptableObject("Alchemy Post Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
        }
        refrence.val = this;
    }


    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
        eventTextManager = ((EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence")).val;
        playerInventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
    }

    private void Awake()
    {
        SetRefrence();
    }


    private void Start()
    {
        PotionLibrary.Initialize();        
        SetLists();
    }

    private void OnEnable()
    {
        LoadSORefrence();
        SetLists();
    }

    /// <summary>
    /// Method to set the bool for knowing to active the canvas or minigame on player interact
    /// </summary>
    /// <param name="isPlaying"></param>
    public void SetMiniGameStatus(bool isPlaying)
    {
        minigameIsPlaying = isPlaying;
    }

    
    
    // To reset the needed lists to clear and refill them from the very begining.
    private void SetLists()
    {
        inventoryHerbs = new();
        herbNames = new();
        sendingHerbs = new();
        List<Herb> herbs = inventoryRefrence.val.SearchInventoryOfItemBehaviour<Herb>(ItemType.herb);
        foreach (Herb herb in herbs)
        {
            Type type = herb.GetType();
            inventoryHerbs.Add((Herb)Activator.CreateInstance(type,new object[]{ herb.CurrentStack()}));
        }

       

        SetSendingList();
    }

    // to reset the sendingHerbs and herbNames list to refresh the ui after the OnPointerUp event.
    private void SetSendingList()
    {
        herbNames.Clear();
        sendingHerbs.Clear();
        foreach (var herb in inventoryHerbs)
        {
            if (herb.CurrentStack() >= potionCreationHerbCount)
            {
                if (!herbNames.Contains(herb.GetName()))
                {
                    herbNames.Add(herb.GetName());
                    sendingHerbs.Add(herb);
                }
            }
        }
    }
    
    public void LuckyPotionBuff(int amount)
    {
        miniGame.LuckyPotionBuff(amount);
    }

    public void LuckyPotionReset()
    {
        miniGame.LuckyPotionReset();
    }


    public void UpgradeQubitCritPassive(bool isQubit)
    {
        if (!isQubit)
        {
            miniGame.SetCritPassiveAmount(5);
        }
        else
        {
            miniGame.SetCritPassiveAmount(10);
        }
    }


    public void UpgradeQubitUnlockSlot(bool isQubit)
    {
        secondSlot.UpgradeOrbit();
        if (isQubit)
            thirdSlot.UpgradeOrbit();

    }


    public void UpgradeOrbitLessHerb(bool isQubit)
    {
        if (!isQubit)
            potionCreationHerbCount = 4;
        else
            potionCreationHerbCount = 3;

        foreach (var slot in slotsList)
        {
            slot.SetHerbCount(potionCreationHerbCount);
        }
    }

    /// <summary>
    /// Method for the UI button to get called when ever the create potion is pressed.
    /// The method sorts the potion effects and reset the ui and remove the related herbs from the player bag.
    /// </summary>
    public void CreatePotionClicked()
    {
        PotionEffect first, second,third;
        PotionItem targetPotion;
        List<PotionEffect> sortedPotionEfect = PotionEffectSorter();

        PotionEffect emptyEffect = new PotionEffect.EmptyEffect();
        bool allIsEmpty = true;
        if (sortedPotionEfect[0].isBase == false)
        {
            eventTextManager.CreateNewText("No base effect is selected",TextType.Error);
            return;
        }
        else if (sortedPotionEfect[1].isBase || sortedPotionEfect[2].isBase)
        {
            eventTextManager.CreateNewText("More than one base effect is selected", TextType.Error);
            return;
        }
        foreach (var item in sortedPotionEfect)
        {
            if (!item.Equals(emptyEffect))
            {
                allIsEmpty = false;
            }
        }
        if (allIsEmpty)
        {
            eventTextManager.CreateNewText("No known potion effect is selected", TextType.Error);
            return;
        }

        first = sortedPotionEfect[0];
        second = sortedPotionEfect[1];
        third = sortedPotionEfect[2];

        targetPotion = new PotionItem(first, second, third);

        //miniGame.CreatePotionButtonClicked(targetPotion);
        //transform.parent.gameObject.SetActive(false);
        if (playerInventory.HaveEmptySlot(targetPotion, true))
        {
            eventTextManager.CreateNewText("Added to inventory", TextType.Information);
        }
        else
        {
            eventTextManager.CreateNewText("Inventory is full", TextType.Error);
        }

        PotionCreated();
    }


    // This method is used to return a list that has it's effect sorted by their priority
    // so we dont have multiple potions with same effects.
    private List<PotionEffect> PotionEffectSorter()
    {
        List<PotionEffect> sortingList = new()
        {
            firstSlot.GetCurrentEffect(),
            secondSlot.GetCurrentEffect(),
            thirdSlot.GetCurrentEffect()
        };

        // Use Sort method to sort the list based on the Priority
        sortingList.Sort((x, y) => x.Priority().CompareTo(y.Priority()));

        return sortingList;
    }

    // this method is used to check if the and EffectSlot has an effect in it or not.
    // if yes it will order it to reduct the related material and reset back to default state.
    private void PotionCreated()
    {
        SetSendingList();
        firstSlot.CreatedThePotion();
        secondSlot.CreatedThePotion();
        thirdSlot.CreatedThePotion();
    }



    /// <summary>
    /// Method to start the activation of the herb selection UI.
    /// </summary>
    public void SlotsOnPointerDown()
    {
        Vector3 pos = Input.mousePosition;
        popUp.gameObject.SetActive(true);
        popUp.GetComponent<RectTransform>().position = pos;
        popUp.SetHerb(sendingHerbs);
    }

    /// <summary>
    /// Method to set the herb selection UI back to deactive.
    /// </summary>
    public void SlotsOnPointerUp()
    {
        popUp.gameObject.SetActive(false);
        SetSendingList();
    }

}