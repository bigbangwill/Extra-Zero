using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [SerializeField] private List<WaveDifficultySO> waveDifficultyList = new();


    private int orderCombinationEffectsApplied;
    private int combinationRandomnessEffectsApplied;
    private float orderFrequencyEffectsApplied;
    private float frequencyRandomnessEffectsApplied;
    private float timerOfOneWaveEffectsApplied;
    private float TimerOfOneWaveRandomnessEffectsApplied;
    private float timeOfAFullCycleEffectsApplied;
    private float walkingOrderSpeedEffectsApplied;
    private float orderFulfillTimerEffectsApplied;


    private WaveDifficultySO selectedWave;


    public WaveDifficultySO GetNextWave()
    {

        if (selectedWave != null)
        {
            return selectedWave;
        }
        else
        {
            return GetRandomNextWave();
        }

    }

    private WaveDifficultySO GetRandomNextWave()
    {
        WaveDifficultySO targetWave = waveDifficultyList[Random.Range(0, waveDifficultyList.Count)];
        return targetWave;
    }




}