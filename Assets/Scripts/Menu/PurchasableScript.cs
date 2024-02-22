using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor.Timeline.Actions;

public class PurchasableScript : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject activeIndicator;

    private PurchasableLibrary purchasable;
    public PurchasableLibrary Purchasable { get { return purchasable; } }

    private MenuPurchaseScript purchaseScript;

    public void SetupPurchasables(PurchasableLibrary purchasable, MenuPurchaseScript purchaseScript)
    {
        this.purchasable = purchasable;
        this.purchaseScript = purchaseScript;
        GetComponent<Image>().sprite = purchasable.Icon;
    }

    public void SetActiveIndicator(bool active)
    {
        activeIndicator.SetActive(active);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        purchaseScript.SetPurchasableActive(this);
    }
}
