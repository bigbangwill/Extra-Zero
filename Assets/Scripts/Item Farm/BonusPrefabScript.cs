using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.StickyNote;

public class BonusPrefabScript : MonoBehaviour
{
    [SerializeField] Image image;

    public Action<Seed> holdingAction;

    private float currentTimer = 0;
    private float maxTimer = 5;

    [SerializeField] private Color colorStart;
    [SerializeField] private Color colorEnd;

    private void Update()
    {
        currentTimer += Time.deltaTime;
        ChangeColor(currentTimer);
        if (maxTimer <= currentTimer)
        {
            Destroy(gameObject);
        }
    }

    private void ChangeColor(float timerValue)
    {
        float normalizedTime = Mathf.Clamp01(timerValue / maxTimer);
        Color lerpedColor = Color.Lerp(colorStart, colorEnd, normalizedTime);
        image.color = lerpedColor;
    }


    public void SetAction(Action<Seed> action,Sprite icon,float maxTimer)
    {
        image.sprite = icon;
        holdingAction = action;
        this.maxTimer = maxTimer;
    }

    public Action<Seed> GetAction()
    {
        return holdingAction;
    }

}