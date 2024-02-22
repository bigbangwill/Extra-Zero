using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPurchaseScript : MonoBehaviour
{
    [SerializeField] private Transform scrollViewContent;
    [SerializeField] private GameObject purchasePrefab;

    private List<Action> purchasablesList = new();

    private Dictionary<GameObject, Action> purchasableDictionary = new();
    private Action activeAction;

    private void Start()
    {
        Init();
    }


    private void Init()
    {
        purchasablesList.Add(PurchasedQubit);
        purchasablesList.Add(PurchasedGate);
        purchasablesList.Add(PurchasedEntangle);
        for(int i = 0; i < purchasablesList.Count; i++)
        {
            GameObject purchasableGO = Instantiate(purchasePrefab, scrollViewContent);
            purchasableGO.GetComponent<PurchasableScript>().SetupPurchasables(this);
            purchasableDictionary.Add(purchasableGO, purchasablesList[i]);
        }
    }
    

    public void SetPurchasableActive(GameObject purchasableGO)
    {
        activeAction = purchasableDictionary[purchasableGO];
    }


    public void PurchasedQubit()
    {

    }

    public void PurchasedGate()
    {

    }

    public void PurchasedEntangle()
    {

    }

}
