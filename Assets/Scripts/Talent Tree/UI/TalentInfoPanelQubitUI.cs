using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentInfoPanelQubitUI : MonoBehaviour
{
    [SerializeField] private Image uiImage;
    [SerializeField] private TextMeshProUGUI talentDescription;
    [SerializeField] private TextMeshProUGUI talentCost;
    [SerializeField] private TextMeshProUGUI talentDescriptionQubit;


    public void SetQubitInfoUI(NodePassive passive)
    {
        gameObject.SetActive(true);
        uiImage.sprite = passive.GetTalentIcon();
        talentDescription.text = passive.GetTalentDescription();
        talentCost.text = passive.GetTalentCost().ToString();
        talentDescriptionQubit.text = passive.GetTalentDescriptionQubit();
    }

}