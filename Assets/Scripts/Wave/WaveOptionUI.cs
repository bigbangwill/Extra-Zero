using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using static WaveManager;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;

public class WaveOptionUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image harderIcon;
    [SerializeField] private TextMeshProUGUI harderDescription;
    [SerializeField] private Image waveIcon;
    [SerializeField] private TextMeshProUGUI waveDescription;
    [SerializeField] private Image rewardIcon;
    [SerializeField] private TextMeshProUGUI rewardDescription;

    [SerializeField] private GameObject markedImage;

    private WaveDifficultySO waveDifficulty;

    private WaveChosingUI relatedCanvas;

    public WaveDifficultySO TargetWaveDifficulty {get => waveDifficulty;}

    private List<PermanentWaveEffectLibrary> waveEffectList = new(); 


    public void SetWaveDififculty(WaveDifficultySO waveDifficulty)
    {
        this.waveDifficulty = waveDifficulty;
    }


    public void SetRelatedEffectsLists(params PermanentWaveEffectLibrary[] effects)
    {
        foreach (PermanentWaveEffectLibrary effect in effects)
        {
            waveEffectList.Add(effect);
        }
    }

    public void SetWaveChosingUI(WaveChosingUI canvas)
    {
        relatedCanvas = canvas;
    }

    public void ExecuteImpact()
    {
        foreach (PermanentWaveEffectLibrary effect in waveEffectList)
        {
            effect.ImpactEffect();
        }
    }


    public void SetHarderIcon(Sprite icon)
    {
        harderIcon.sprite = icon;
    }

    public void SetHarderDescription(string description)
    {
        harderDescription.text = description;
    }


    public void SetWaveIcon(Sprite icon)
    {
        waveIcon.sprite = icon;
    }

    public void SetWaveDescription(string description)
    {
        waveDescription.text = description;
    }

    public void SetRewardIcon(Sprite icon)
    {
        rewardIcon.sprite = icon;
    }

    public void SetRewardDescription(string description)
    {
        rewardDescription.text = description;
    }

    public void SelectThisOption()
    {
        markedImage.SetActive(true);
    }

    public void DeselectThisOption()
    {
        markedImage.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        relatedCanvas.SetSelectedWaveOption(this);
    }


}
