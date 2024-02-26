using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionHolderCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textQubit;
    [SerializeField] private TextMeshProUGUI textGate;
    [SerializeField] private TextMeshProUGUI textEntangle;

    private OptionHolderRefrence optionHolderRefrence;

    private void LoadSORefrence()
    {
        optionHolderRefrence = (OptionHolderRefrence)FindSORefrence<OptionHolder>.FindScriptableObject("Option Holder Refrence");
    }

    private void OnEnable()
    {
        LoadSORefrence();
        optionHolderRefrence.val.AddListener(InitUI);
    }

    private void OnDisable()
    {
        if(optionHolderRefrence.val != null)
            optionHolderRefrence.val.RemoveListener(InitUI);
    }

    private void Start()
    {
        InitUI();
    }

    public void InitUI()
    {
        string qubitCurrent = optionHolderRefrence.val.QubitCurrentCount.ToString();
        string qubitMax = optionHolderRefrence.val.QubitMaxCount.ToString();
        string gateCurrent = optionHolderRefrence.val.GateCurrentCount.ToString();
        string gateMax = optionHolderRefrence.val.GateMaxCount.ToString();
        string entangleCurrent = optionHolderRefrence.val.EntangleCurrentCount.ToString();
        string entangleMax = optionHolderRefrence.val.EntangleMaxCount.ToString();

        textQubit.text = qubitCurrent + " / " + qubitMax;
        textGate.text = gateCurrent + " / " + gateMax;
        textEntangle.text = entangleCurrent + " / " + entangleMax;
    }

    
}