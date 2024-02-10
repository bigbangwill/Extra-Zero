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




    public void CreateWaveOptionUI()
    {
        foreach (Transform child in optionUIHolder)
        {
            Destroy(child.gameObject);
        }
        int waveOptionToSpawn = Instance.GetTotalWaveOptionCount();
        for (int i = 0; i < waveOptionToSpawn; i++)
        {
            GameObject targetGO = Instantiate(waveOptionPrefab);
            targetGO.transform.SetParent(optionUIHolder);
            WaveOptionUI targetOptionUI = targetGO.GetComponent<WaveOptionUI>();
            targetOptionUI.SetWaveChosingUI(this);
            HarderSideEffects targetHarder = Instance.GetRandomHarderEffect();
            RewardSideEffects targetReward = Instance.GetRandomRewardEffect();
            WaveDifficultySO waveDifficulty = Instance.GetRandomNextWave();
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
            Instance.ExecuteRandomEffectsAndWave();
            return;
        }
        currentWaveOption.ExecuteImpact();
        WaveDifficultySO tweakedWave = Instance.ApplyCurrentEffectsToTheWave(currentWaveOption.TargetWaveDifficulty);
        OrderManager.Instance.StartNewWave(tweakedWave);
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
