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



    
    public void SetActivePanel(NodePassive nodePassvie)
    {
        gameObject.SetActive(true);
    }

    public void DeactivePanel()
    {
        gameObject.SetActive(false);
    }
}
