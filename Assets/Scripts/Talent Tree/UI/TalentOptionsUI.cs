using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentOptionsUI : MonoBehaviour
{

    private NodePassive currentPassive;

    [SerializeField] private Button qubitButton;
    [SerializeField] private Button gateButton;
    [SerializeField] private Button entangleButton;

    [SerializeField] private OptionHolder optionHolder;



    public void SetActive(NodePassive passive)
    {
        gameObject.SetActive(true);
        currentPassive = passive;
        qubitButton.interactable = optionHolder.CanAddQubit();
        gateButton.interactable = optionHolder.CanAddGate();
        entangleButton.interactable = optionHolder.CanAddEntangle();

        if(passive.IsQubit())
        {
            qubitButton.interactable = true;
        }
        if (passive.IsGated())
        {
            gateButton.interactable = false;
        }

    }

    public void SetDeative()
    {
        currentPassive = null;
        gameObject.SetActive(false);
    }


    public void QubitButton()
    {
        if (currentPassive.IsQubit())
        {
            optionHolder.SetQubitCurrent(optionHolder.QubitCurrentCount - 1);
            currentPassive.DowngradeQubit();
        }
        else
        {
            optionHolder.SetQubitCurrent(optionHolder.QubitCurrentCount + 1);
            currentPassive.UpgradeQubit();
        }
    }

    public void GateButton()
    {
        TalentManager.Instance.SetGateStart();
    }

    public void EntanglementButton()
    {

    }

    
}