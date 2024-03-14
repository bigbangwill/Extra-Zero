using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSlots : MonoBehaviour
{
    [SerializeField] private int slot;
    [SerializeField] private Image image;
    [SerializeField] private Sprite lockedImage;
    [SerializeField] private Sprite unlockedImage;

    [SerializeField] List<AlchemySlots> slots;

    private Herb[] herbArray = new Herb[3];
    private PotionEffect currentEffect;

    [SerializeField] private bool isLocked;

    private void Start()
    {
        SetSlotState(isLocked);
    }

    private void SetSlotState(bool isLocked)
    {
        foreach(var slot in slots)
        {
            if (isLocked)
            {
                slot.SetLocked();
            }
            else
            {
                slot.SetUnlocked();
            }
        }
    }

    public void UpgradeOrbit()
    {
        isLocked = false;
        SetSlotState(isLocked);
    }


    /// <summary>
    /// Will get called from the AlchemySlots Script to set the selected herb by the player in the
    /// related herbArray slot.
    /// </summary>
    /// <param name="herb"></param>
    /// <param name="slotNumber"></param>
    public void SetHerbForEffect(Herb herb, int slotNumber)
    {
        herbArray[slotNumber] = herb;
        bool isFull = true;
        for (int i = 0; i < herbArray.Length; i++)
        {
            if (herbArray[i] == null)
            {
                isFull = false;
                SetNoPotion();
            }
        }
        if(isFull)
        {
            SetEffect();
        }
    }
    
    // This method gets called if all the herbs in the slots are selected.
    private void SetEffect()
    {
        currentEffect = PotionLibrary.FindEffect(herbArray[0], herbArray[1], herbArray[2]);
        image.sprite = currentEffect.sprite;
        Debug.Log(currentEffect.name);
    }

    // This method gets called if there are some slots empty.
    private void SetNoPotion()
    {
        currentEffect = null;
        image.sprite = null;
    }

    /// <summary>
    /// This method gets called from AlchemyPost to reset the values.
    /// </summary>
    public void CreatedThePotion()
    {
        foreach (var slot in slots)
        {
            slot.PotionCreated();
        }
        herbArray = new Herb[3];
        currentEffect = null;
        image.sprite = null;
    }

    /// <summary>
    /// This method get called from AlchemyPost to return the current selected PotionEffect for 
    /// creating the potion.
    /// </summary>
    /// <returns></returns>
    public PotionEffect GetCurrentEffect()
    {
        if (currentEffect == null)
            currentEffect = new PotionEffect.EmptyEffect();
        return currentEffect;
    }
    private void OnDisable()
    {
        SetNoPotion();
    }
}