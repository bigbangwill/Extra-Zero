using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New WaveDifficulty", menuName = "Wave Setting", order = 0)]
public class WaveDifficultySO : ScriptableObject
{

    public int orderCombination;
    public int combinationRandomness;
    public float orderFrequency;
    public float frequencyRandomness;
    public float timerOfOneWave;
    public float frequencyOfTimer;
    public float timeOfAFullCycle;
    public float walkingOrderSpeed;
    public float orderFulfillTimer;


    [Header("In-Game wave harder")]
    [SerializeField] private float orderFrequencyHardness;
    [SerializeField] private float timerOfOneWaveHardness;
    [SerializeField] private float orderFulfilTimerHardness;

    [Header("Appreance!")]
    [SerializeField] private Sprite waveIcon;

    public int GetOrderCombination()
    {
        int minValue = orderCombination - combinationRandomness;
        int maxValue = orderCombination + combinationRandomness;
        int value = Random.Range(minValue, maxValue);
        if (value > OrderManager.Instance.GetMaxOrderCombination())
            value = OrderManager.Instance.GetMaxOrderCombination();
        return value;
    }

    public float GetOrderFrequency()
    {
        float minValue = orderFrequency - frequencyRandomness;
        float maxValue = orderFrequency + frequencyOfTimer;
        float value = Random.Range(minValue, maxValue);
        return value;
    }

    public float GetTimerOfWave()
    {
        float minValue = timerOfOneWave - frequencyOfTimer;
        float maxValue = timerOfOneWave + frequencyOfTimer;
        float value = Random.Range(minValue, maxValue);
        return value;
    }

    public float GetTimeOfAFullCycle()
    {
        return timeOfAFullCycle;
    }

    public float GetNightMaxTime()
    {
        return timeOfAFullCycle - timerOfOneWave;
    }

    public float GetWalkingOrderSpeed()
    {
        return walkingOrderSpeed;
    }

    public float GetOrderFulfillTimer()
    {
        return orderFulfillTimer;
    }

    public Sprite GetWaveIcon()
    {
        return waveIcon;
    }

}
