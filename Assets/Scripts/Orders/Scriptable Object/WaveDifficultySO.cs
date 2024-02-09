using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New WaveDifficulty", menuName = "Wave Setting", order = 0)]
public class WaveDifficultySO : ScriptableObject
{

    [SerializeField] private int orderCombination;
    [SerializeField] private int combinationRandomness;

    [SerializeField] private float orderFrequency;
    [SerializeField] private float frequencyRandomness;

    [SerializeField] private float timerOfOneWave;
    [SerializeField] private float frequencyOfTimer;

    [SerializeField] private float timeOfAFullCycle;

    [SerializeField] private float walkingOrderSpeed;

    [SerializeField] private float orderFulfillTimer;


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

    public float GetOrderFrequency(int currentWaveNumber)
    {
        float minValue = orderFrequency - frequencyRandomness;
        float maxValue = orderFrequency + frequencyOfTimer;
        float value = Random.Range(minValue, maxValue);
        return value - orderFrequencyHardness * currentWaveNumber;
    }

    public float GetTimerOfWave(int currentWaveNumber)
    {
        float minValue = timerOfOneWave - frequencyOfTimer;
        float maxValue = timerOfOneWave + frequencyOfTimer;
        float value = Random.Range(minValue, maxValue);
        return value - timerOfOneWaveHardness * currentWaveNumber;
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

    public float GetOrderFulfillTimer(int currentWaveNumber)
    {
        return orderFulfillTimer - orderFulfilTimerHardness * currentWaveNumber;
    }

    public Sprite GetWaveIcon()
    {
        return waveIcon;
    }

}
