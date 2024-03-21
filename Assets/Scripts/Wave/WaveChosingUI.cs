using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using static WaveManager;

public class WaveChosingUI : MonoBehaviour
{
    private WaveOptionUI currentWaveOption;

    [SerializeField] private GameObject waveOptionPrefab;
    [SerializeField] private Transform optionUIHolder;



    private WaveManagerRefrence waveManagerRefrence;

    private OrderManagerRefrence orderManagerRefrence;

    private void LoadSORefrence()
    {
        orderManagerRefrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
        waveManagerRefrence = (WaveManagerRefrence)FindSORefrence<WaveManager>.FindScriptableObject("Wave Manager Refrence");
        
    }

    

    private void Start()
    {
        LoadSORefrence();
    }

    public void CreateWaveOptionUI()
    {
        foreach (Transform child in optionUIHolder)
        {
            Destroy(child.gameObject);
        }
        if (waveManagerRefrence == null)
            LoadSORefrence();
        int waveOptionToSpawn = waveManagerRefrence.val.GetTotalWaveOptionCount();
        for (int i = 0; i < waveOptionToSpawn; i++)
        {
            GameObject targetGO = Instantiate(waveOptionPrefab);
            targetGO.transform.SetParent(optionUIHolder);
            WaveOptionUI targetOptionUI = targetGO.GetComponent<WaveOptionUI>();
            targetOptionUI.SetWaveChosingUI(this);
            HarderSideEffects targetHarder = waveManagerRefrence.val.GetRandomHarderEffect();
            RewardSideEffects targetReward = waveManagerRefrence.val.GetRandomRewardEffect();
            WaveDifficultySO waveDifficulty = waveManagerRefrence.val.GetRandomNextWave();
            targetOptionUI.SetRelatedEffectsLists(targetHarder, targetReward);
            targetOptionUI.SetWaveDififculty(waveDifficulty);
            targetOptionUI.SetHarderIcon(targetHarder.IconRefrence());
            targetOptionUI.SetWaveIcon(waveDifficulty.GetWaveIcon());
            targetOptionUI.SetRewardIcon(targetReward.IconRefrence());
            targetOptionUI.SetHarderDescription(targetHarder.GetEffectDescription());
            targetOptionUI.SetWaveDescription(waveDifficulty.GetWaveDescription());
            targetOptionUI.SetRewardDescription(targetReward.GetEffectDescription());
        }
    }

    /// <summary>
    /// Method to be called by the waveOptions to set the current selected wave option.
    /// </summary>
    /// <param name="waveOption"></param>
    public void SetSelectedWaveOption(WaveOptionUI waveOption)
    {
        if (currentWaveOption == waveOption)
        {
            currentWaveOption.DeselectThisOption();
            currentWaveOption = null;
        }
        else
        {
            if(currentWaveOption != null)
                currentWaveOption.DeselectThisOption();
            currentWaveOption = waveOption;
            currentWaveOption.SelectThisOption();
        }
    }

    /// <summary>
    /// To get called after the night time is over and set the selected wave options/
    /// </summary>
    public void GetNextWaveAndExecuteEffects()
    {
        ClearWaveOptions();
        if(currentWaveOption == null)
        {
            waveManagerRefrence.val.ExecuteRandomEffectsAndWave();
            return;
        }
        currentWaveOption.ExecuteImpact();
        WaveDifficultySO tweakedWave = waveManagerRefrence.val.ApplyCurrentEffectsToTheWave(currentWaveOption.TargetWaveDifficulty);
        orderManagerRefrence.val.StartNewWave(tweakedWave);
    }

    // to clear the UI from the waveOptions.
    private void ClearWaveOptions()
    {
        foreach (Transform child in optionUIHolder)
        {
            Destroy(child.gameObject);
        }
    }

}
