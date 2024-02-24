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


        // For Gate button
        if (currentPassive.IsQubit() && !currentPassive.IsEntangled() && optionHolder.CanAddGate())
            gateButton.interactable = true;
        else
            gateButton.interactable = false;

        // For Entangle button
        if (currentPassive.IsQubit() && !currentPassive.IsGated() && optionHolder.CanAddEntangle())
            entangleButton.interactable = true;
        else
            entangleButton.interactable = false;


        if(passive.IsQubit())
            qubitButton.interactable = true;
        if (passive.IsGated())
            gateButton.interactable = true;

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
            optionHolder.AddQubitCurrent(-1);
            currentPassive.DowngradeQubit();
        }
        else
        {
            optionHolder.AddQubitCurrent(+1);
            currentPassive.UpgradeQubit();
        }
        SetActive(currentPassive);
    }

    public void GateButton()
    {
        if (currentPassive.IsGated())
        {
            optionHolder.AddGateCurrent(-1);
            currentPassive.DowngradeGate();
        }
        else
        {
            TalentManager.Instance.SetGateStart();
        }
        SetActive(currentPassive);

    }

    public void EntanglementButton()
    {
        if (currentPassive.IsEntangled())
        {
            optionHolder.AddEntangleCurrent(-1);
            currentPassive.DowngradeEntangle();
        }
        else
        {
            TalentManager.Instance.SetEntangleStart();
        }
        SetActive(currentPassive);
    }

    
}