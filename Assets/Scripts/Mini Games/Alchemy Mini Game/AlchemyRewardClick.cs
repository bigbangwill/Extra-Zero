using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AlchemyRewardClick : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private int slotNumber;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponentInParent<AlchemyRewardPanel>().RewardClicked(slotNumber);
    }
}