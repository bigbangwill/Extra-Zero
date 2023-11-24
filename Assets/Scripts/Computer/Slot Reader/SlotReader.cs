using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotReader : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button interactButton;

    public void SetNameText(string text)
    {
        nameText.text = text;
    }

    public void SetTimerText(string text)
    {
        timerText.text = text;
    }

    public void SetButtonText(string text)
    {
        buttonText.text = text;
    }

    public void SetImageSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void SetInteractiveButton(bool setActive)
    {
        interactButton.interactable = setActive;
    }

}