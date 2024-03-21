using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

public class TierUIItem : MonoBehaviour
{
    [SerializeField] private GameObject checkObject;
    [SerializeField] private Image icon;


    public void SetIcon(Sprite icon)
    {
        this.icon.sprite = icon;
        SetState(false);
    }

    public void SetState(bool isDone)
    {
        if (isDone)
            checkObject.SetActive(true);
        else
            checkObject.SetActive(false);
    }
}