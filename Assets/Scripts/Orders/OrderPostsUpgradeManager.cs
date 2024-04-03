using System.Collections.Generic;
using UnityEngine;

public class OrderPostsUpgradeManager : MonoBehaviour
{
    [SerializeField] private List<OrderPostHealth> orderPostHealths;

    [SerializeField] private List<OrderPost> orderPosts;

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

    public void StrenghtPotionBuff(float speed)
    {
        foreach (var orderPost in orderPosts)
        {
            orderPost.StrenghtTimeAmount(speed);
        }
    }

    public void StrenghtPotionReset()
    {
        foreach (var orderPost in orderPosts)
        {
            orderPost.StrenghtTimeReset();
        }
    }

    public void SpeedPotionBuff(float amount)
    {
        foreach (var orderPost in orderPosts)
        {
            orderPost.SpeedTimeAmount(amount);
        }
    }
    public void SpeedPotionReset()
    {
        foreach (var orderPost in orderPosts)
        {
            orderPost.SpeedTimeReset();
        }
    }


    public void PrecisionPotionBuff()
    {
        orderPosts[Random.Range(0,orderPosts.Count)].PrecisionBuff();
    }


}