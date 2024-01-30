using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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

}
