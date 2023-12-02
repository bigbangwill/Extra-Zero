using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectSlots : MonoBehaviour
{
    [SerializeField] private int slot;
    [SerializeField] private Image image;

    private Herb[] herbArray = new Herb[3];
    private PotionEffect currentEffect;

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
    
    private void SetEffect()
    {
        currentEffect = PotionLibrary.FindEffect(herbArray[0], herbArray[1], herbArray[2]);
        image.sprite = currentEffect.sprite;
        Debug.Log(currentEffect.name);
    }
    private void SetNoPotion()
    {
        currentEffect = null;
        image.sprite = null;
    }
}