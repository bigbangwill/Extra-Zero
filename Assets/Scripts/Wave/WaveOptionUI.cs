using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class WaveOptionUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Sprite harderIcon;
    [SerializeField] private string harderDescription;
    [SerializeField] private Sprite waveIcon;
    [SerializeField] private string waveDescription;
    [SerializeField] private Sprite rewardIcon;
    [SerializeField] private string rewardDescription;



    private List<Action> relatedImpactsList = new(); 


    public void SetRelatedImpactLists(params Action[] actions)
    {
        foreach (Action impact in actions)
        {
            relatedImpactsList.Add(impact);
        }
    }

    public void ExecuteImpact()
    {
        foreach (Action impact in relatedImpactsList)
        {
            impact.Invoke();
        }
    }


    public void SetHarderIcon(Sprite icon)
    {
        harderIcon = icon;
    }

    public void SetHarderDescription(string description)
    {
        harderDescription = description;
    }


    public void SetWaveIcon(Sprite icon)
    {
        waveIcon = icon;
    }

    public void SetWaveDescription(string description)
    {
        waveDescription = description;
    }

    public void SetRewardIcon(Sprite icon)
    {
        rewardIcon = icon;
    }

    public void SetRewardDescription(string description)
    {
        rewardDescription = description;
    }

    public void SelectThisOption()
    {

    }

    public void DeselectThisOption()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }


}
