using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class OrderPostsUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<OrderPostHealth> orderPostHealths;

    private OrderPostsUpgradeManagerRefrence refrence;




    private void SetRefrence()
    {
        refrence = (OrderPostsUpgradeManagerRefrence)FindSORefrence<OrderPostsUpgradeManager>.FindScriptableObject("Order Posts Upgrade Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetRefrence();
    }


    public void UpgradeOrbitIncreaseHealth(bool isQubit)
    {
        if (!isQubit)
        {
            foreach (var orderPost in orderPostHealths)
            {
                orderPost.SetUnlockedHealth(4);
            }
        }
        else
        {
            foreach (var orderPost in orderPostHealths)
            {
                orderPost.SetUnlockedHealth(5);
            }
        }
    }


    public void UpgradeOrbitLessMaterialForRepair(bool isQubit)
    {
        if (!isQubit)
        {
            foreach (var orderPost in orderPostHealths)
            {
                orderPost.RepairAmountMinusAmount(1);
            }
        }
        else
        {
            foreach (var orderPost in orderPostHealths)
            {
                orderPost.RepairAmountMinusAmount(2);
            }
        }
    }


}