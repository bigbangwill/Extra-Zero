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
    public float timerOfNightTime;
    public float walkingOrderSpeed;
    public float orderFulfillTimer;



    [Header("In-Game wave harder")]
    [SerializeField] private float orderFrequencyHardness;
    [SerializeField] private float timerOfOneWaveHardness;
    [SerializeField] private float orderFulfilTimerHardness;

    [Header("Appreance!")]
    [SerializeField] private Sprite waveIcon;
    [SerializeField] private string waveDescription;

    public int GetOrderCombination()
    {
        int minValue = orderCombination - combinationRandomness;
        int maxValue = orderCombination + combinationRandomness;
        int value = Random.Range(minValue, maxValue);
        if (value > OrderManager.Instance.GetMaxOrderCombination())
            value = OrderManager.Instance.GetMaxOrderCombination();
        return value;
    }

    /// <summary>
    /// How fast they should get summond
    /// </summary>
    /// <returns></returns>
    public float GetOrderFrequency()
    {
        float minValue = orderFrequency - frequencyRandomness;
        float maxValue = orderFrequency + frequencyOfTimer;
        float value = Random.Range(minValue, maxValue);
        return value;
    }

    /// <summary>
    /// How long the day is.
    /// </summary>
    /// <returns></returns>
    public float GetTimerOfWave()
    {
        float minValue = timerOfOneWave - frequencyOfTimer;
        float maxValue = timerOfOneWave + frequencyOfTimer;
        float value = Random.Range(minValue, maxValue);
        return value;
    }


    /// <summary>
    /// Night timer + Day timer
    /// </summary>
    /// <returns></returns>
    public float GetTimeOfAFullCycle()
    {
        return timerOfOneWave + timerOfNightTime;
    }

    /// <summary>
    /// How long the night is.
    /// </summary>
    /// <returns></returns>
    public float GetNightMaxTime()
    {
        return timerOfNightTime;
    }

    /// <summary>
    /// Speed of the walking orders.
    /// </summary>
    /// <returns></returns>
    public float GetWalkingOrderSpeed()
    {
        return walkingOrderSpeed;
    }

    /// <summary>
    /// The timer of the order to be fulfilled.
    /// </summary>
    /// <returns></returns>
    public float GetOrderFulfillTimer()
    {
        return orderFulfillTimer;
    }


    public Sprite GetWaveIcon()
    {
        return waveIcon;
    }

    public string GetWaveDescription()
    {
        return waveDescription;
    }

}
