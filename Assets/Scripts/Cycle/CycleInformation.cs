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
    [SerializeField] private Button startButton;
    private int maxTimer = 60;
    private int currentTimer = 0;

    private bool isRunning = false;


    Coroutine runningCoroutine;


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
        if (isRunning)
        {
            isRunning = false;
            if(runningCoroutine != null)
                StopCoroutine(runningCoroutine);
        }
        orderManagerRefrence.val.StartGame();
    }


    public void StartCounter()
    {
        currentTimer = maxTimer;
        isRunning = true;
        runningCoroutine = StartCoroutine(WaveStartCounter());
    }


    private IEnumerator WaveStartCounter()
    {
        while (true)
        {
            if (currentTimer <= 0)
            {
                startButton.gameObject.SetActive(false);
                isRunning = false;
                runningCoroutine = null;
                StartGame();
                yield break;
            }
            currentTimer -= 1;
            timerText.text = currentTimer.ToString();
            yield return new WaitForSeconds(1);
        }
    }
}