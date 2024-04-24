using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarRewardManager : MonoBehaviour
{


    [SerializeField] private Slider barSlider;

    [SerializeField] private float barMaxValue;
    [SerializeField] private float barStartValue;


    private bool firstRewardUnlocked;
    private bool secondRewardUnlocked;
    private bool thirdRewardUnlocked;

    private RewardBarRefrence refrence;

    private EconomyManagerRefrence economyManagerRefrence;

    private void SetRefrence()
    {
        refrence = (RewardBarRefrence)FindSORefrence<BarRewardManager>.FindScriptableObject("Reward Bar Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }


    private void LoadSoRefrence()
    {
        economyManagerRefrence = (EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence");


    }


    private void Awake()
    {
        SetRefrence();
    }

    



    private void Start()
    {
        LoadSoRefrence();
        if (!GameModeState.IsCampaignMode)
        {
            barSlider.maxValue = barMaxValue;
            barSlider.value = barStartValue;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }


    public void AddToBar(float value)
    {
        barSlider.value += value;
        if (!firstRewardUnlocked && barSlider.value >= (barSlider.maxValue / 3))
        {
            firstRewardUnlocked = true;
        }
        if (!secondRewardUnlocked && barSlider.value >= ((barSlider.maxValue * 2 / 3)))
        {
            secondRewardUnlocked = true;
        }
        if (!thirdRewardUnlocked && barSlider.value >= barSlider.maxValue)
        {
            thirdRewardUnlocked = true;
        }
    }

    public void GiveReward(bool isDoubled)
    {
        int doubleModifier = 1;
        if (isDoubled)
            doubleModifier = 2;
        if (firstRewardUnlocked)
        {
            economyManagerRefrence.val.OutGameCurrencyCurrentStack += 1 * doubleModifier;
        }
        if (secondRewardUnlocked)
        {
            economyManagerRefrence.val.CampaignEnergyCurrentStack += 1 * doubleModifier;
        }
        if (thirdRewardUnlocked)
        {
            economyManagerRefrence.val.CampaignEnergyCurrentStack += 2 * doubleModifier;
        }

    }

}
