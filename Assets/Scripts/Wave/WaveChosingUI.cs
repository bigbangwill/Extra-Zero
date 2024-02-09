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
            Destroy(child);
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
            targetOptionUI.SetRelatedEffectsLists(targetHarder, targetReward);
        }
    }
    public void RunTheWaveEffects()
    {
        if (currentWaveOption == null)
        {
            Instance.ExecuteRandomEffectsAndWave();
        }
        currentWaveOption.ExecuteImpact();
    }

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



}
