using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionHolderCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textQubit;
    [SerializeField] private TextMeshProUGUI textGate;
    [SerializeField] private TextMeshProUGUI textEntangle;

    private void OnEnable()
    {
        OptionHolder.Instance.AddListener(InitUI);
    }

    private void OnDisable()
    {
        OptionHolder.Instance.RemoveListener(InitUI);
    }

    public void InitUI()
    {
        string qubitCurrent = OptionHolder.Instance.QubitCurrentCount.ToString();
        string qubitMax = OptionHolder.Instance.QubitMaxCount.ToString();
        string gateCurrent = OptionHolder.Instance.GateCurrentCount.ToString();
        string gateMax = OptionHolder.Instance.GateCurrentCount.ToString();
        string entangleCurrent = OptionHolder.Instance.EntangleCurrentCount.ToString();
        string entangleMax = OptionHolder.Instance.EntangleMaxCount.ToString();

        textQubit.text = qubitCurrent + " / " + qubitMax;
        textGate.text = gateCurrent + " / " + gateMax;
        textEntangle.text = entangleCurrent + " / " + entangleMax;
    }

    
}