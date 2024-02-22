using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalentInfoPanelUI : MonoBehaviour
{

    [SerializeField] private Image uiImage;
    [SerializeField] private TextMeshProUGUI talentDescription;
    [SerializeField] private TextMeshProUGUI talentCost;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI purchaseButtonText;



    
    public void SetActivePanel(NodePassive nodePassvie,bool canPurchase)
    {
        gameObject.SetActive(true);
        uiImage.sprite = nodePassvie.GetTalentIcon();
        talentDescription.text = nodePassvie.GetTalentDescription();
        talentCost.text = nodePassvie.GetTalentCost().ToString();
        purchaseButton.interactable = canPurchase;
        if (!canPurchase)
        {
            purchaseButtonText.text = "CantPurchase";
        }
        else
        {
            purchaseButtonText.text = "Purchase?";
        }
    }

    public void DeactivePanel()
    {
        gameObject.SetActive(false);
    }
}