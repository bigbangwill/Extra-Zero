using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUnlocker : MonoBehaviour
{
    [SerializeField] private ProgressionScript progressionScript;

    [SerializeField] private Button menuShop;


    private void OnEnable()
    {
        if(progressionScript.MenuShopIsUnlocked)
        {
            menuShop.interactable = true;
        }
        else
        {
            menuShop.interactable = false;
        }
    }


}