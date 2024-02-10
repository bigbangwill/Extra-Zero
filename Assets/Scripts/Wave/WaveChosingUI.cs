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
            targetOptionUI.SetRelatedEffectsLists(targetHarder, targetReward);
            targetOptionUI.SetWaveDififculty(Instance.GetRandomNextWave());
        }
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

    private void ClearWaveOptions()
    {
        foreach (Transform child in optionUIHolder)
        {
            Destroy(child.gameObject);
        }
    }

}
