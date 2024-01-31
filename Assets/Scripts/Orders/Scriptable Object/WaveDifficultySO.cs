using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
        return Random.Range(minValue, maxValue);
    }

    public float GetTimerOfWave()
    {
        float minValue = timerOfOneWave - frequencyOfTimer;
        float maxValue = timerOfOneWave + frequencyOfTimer;
        return Random.Range(minValue, maxValue);
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

}
