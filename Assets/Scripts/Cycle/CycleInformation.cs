using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CycleInformationEnum { DayTime,NightTimer}

public class CycleInformation : MonoBehaviour
{
    [SerializeField] private Sprite daySprite;
    [SerializeField] private Sprite nightSprite;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI timerText;


    private OrderManagerRefrence orderManagerRefrence;

    private void Start()
    {
        orderManagerRefrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
    }


    public void SetIcon(CycleInformationEnum info)
    {
        if (info == CycleInformationEnum.DayTime)
        {
            iconImage.sprite = daySprite;
        }
        else if (info == CycleInformationEnum.NightTimer)
        {
            iconImage.sprite = nightSprite;
        }
    }

    public void SetTimerText(float time)
    {
        timerText.text = ((int)time).ToString();
    }

    public void StartGame()
    {
        orderManagerRefrence.val.StartGame();
    }
}