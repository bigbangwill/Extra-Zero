using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class ImportedUI : MonoBehaviour ,IPointerDownHandler
{

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemNameText;

    [SerializeField] private Image materialIcon1;
    [SerializeField] private Image materialIcon2;
    [SerializeField] private Image materialIcon3;

    [SerializeField] private TextMeshProUGUI countMaterial1;
    [SerializeField] private TextMeshProUGUI countMaterial2;
    [SerializeField] private TextMeshProUGUI countMaterial3;
    [SerializeField] private GameObject activeGameobject;


    private int listNumber;

    private Action<int,GameObject> CurrentActive; // this will get set upon initializing the UI.

    /// <summary>
    /// Will get called on initializing 
    /// </summary>
    /// <param name="action"></param>
    public void SetActiveMethod(Action<int,GameObject> action)
    {
        CurrentActive = action;
        
    }

    public void SetNumber(int number)
    {
        listNumber = number;
    }

    /// <summary>
    /// Will get called on OnPointerDown
    /// </summary>
    private void SetCurrentActive()
    {
        activeGameobject.SetActive(true);
        CurrentActive(listNumber,activeGameobject);
    }

    public void SetItemImage(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public void SetItemNameText(string name)
    {
        itemNameText.text = name;
    }

    public void SetMaterialIcon1(Sprite sprite)
    {
        materialIcon1.sprite = sprite;
    }
    public void SetMaterialIcon2(Sprite sprite)
    {
        materialIcon2.sprite = sprite;
    }
    public void SetMaterialIcon3(Sprite sprite)
    {
        materialIcon3.sprite = sprite;
    }

    public void SetCountMaterialText1(string count)
    {
        countMaterial1.text = count;
    }
    public void SetCountMaterialText2(string count)
    {
        countMaterial2.text = count;
    }
    public void SetCountMaterialText3(string count)
    {
        countMaterial3.text = count;
    }

    public void Initialize(int i)
    {
        if (i == 1)
        {
            materialIcon2.gameObject.SetActive(false);
            materialIcon3.gameObject.SetActive(false);
        }
        if (i == 2)
        {
            materialIcon3.gameObject.SetActive(false);
        }
        if (materialIcon3 == null)
        {
            Debug.Log("Fully Loaded");
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetCurrentActive();
    }
}
