using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class BluePrintInfo : MonoBehaviour
{

    [SerializeField] private Button purchaseButton;
    [SerializeField] private Image blueprintIcon;

    [SerializeField] private GameObject material1GO;
    [SerializeField] private Image material1Icon;
    [SerializeField] private TextMeshProUGUI material1Name;
    [SerializeField] private TextMeshProUGUI material1Stack;
    [SerializeField] private GameObject material2GO;
    [SerializeField] private Image material2Icon;
    [SerializeField] private TextMeshProUGUI material2Name;
    [SerializeField] private TextMeshProUGUI material2Stack;
    [SerializeField] private GameObject material3GO;
    [SerializeField] private Image material3Icon;
    [SerializeField] private TextMeshProUGUI material3Name;
    [SerializeField] private TextMeshProUGUI material3Stack;


    [SerializeField] private Image resultIcon;

    private EconomyManager economyManager;
    private bool isLoaded = false;
    
    private void LoadSORefrence()
    {
        economyManager = ((EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence")).val;
        Debug.Log(economyManager, economyManager.gameObject);
        isLoaded = true;
    }

    public void SetItem(BluePrintItem target)
    {
        if (!isLoaded)
        {
            LoadSORefrence();
        }
        blueprintIcon.sprite = target.IconRefrence();

        if (target.materialsList.Count == 1)
        {
            SetFirst(target.materialsList[0]);
            material2GO.SetActive(false);
            material3GO.SetActive(false);
        }
        else if(target.materialsList.Count == 2)
        {
            SetFirst(target.materialsList[0]);
            SetSecond(target.materialsList[1]);
            material3GO.SetActive(false);
        }
        else if(target.materialsList.Count == 3)
        {
            SetFirst(target.materialsList[0]);
            SetSecond(target.materialsList[1]);
            SetThird(target.materialsList[2]);
        }
        resultIcon.sprite = target.CraftedItemReference().IconRefrence();
        if (economyManager.QuantumQuartersCurrentStack >= target.PurchaseCost)
        {
            purchaseButton.interactable = true;
        }
        else
        {
            purchaseButton.interactable = false;
        }

    }



    private void SetFirst(ItemBehaviour item)
    {
        material1GO.SetActive(true);
        material1Icon.sprite = item.IconRefrence();
        material1Name.text = item.GetName();
        material1Stack.text = item.CurrentStack().ToString();
    }

    private void SetSecond(ItemBehaviour item)
    {
        material2GO.SetActive(true);
        material2Icon.sprite = item.IconRefrence();
        material2Name.text = item.GetName();
        material2Stack.text = item.CurrentStack().ToString();
    }

    private void SetThird(ItemBehaviour item)
    {
        material3GO.SetActive(true);
        material3Icon.sprite = item.IconRefrence();
        material3Name.text = item.GetName();
        material3Stack.text = item.CurrentStack().ToString();
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }

}