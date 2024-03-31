using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TierUI : MonoBehaviour
{
    [SerializeField] private Transform tier1Parent;
    [SerializeField] private Transform tier2Parent;
    [SerializeField] private Transform tier3Parent;
    [SerializeField] private Transform tier4Parent;

    [SerializeField] private GameObject tierItemPrefab;

    private Dictionary<ItemBehaviour, TierUIItem> itemTierDictionary = new();

    public void SetFirstTier(List<ItemBehaviour> list)
    {
        foreach (var item in list)
        {
            GameObject target = Instantiate(tierItemPrefab,tier1Parent);
            TierUIItem tierUI = target.GetComponent<TierUIItem>();
            tierUI.SetIcon(item.IconRefrence());
            itemTierDictionary.Add(item, tierUI);
        }
    }
    public void SetSecondTier(List<ItemBehaviour> list)
    {
        foreach (var item in list)
        {
            GameObject target = Instantiate(tierItemPrefab, tier2Parent);
            TierUIItem tierUI = target.GetComponent<TierUIItem>();
            tierUI.SetIcon(item.IconRefrence());
            itemTierDictionary.Add(item, tierUI);
        }
    }
    public void SetThirdTier(List<ItemBehaviour> list)
    {
        foreach (var item in list)
        {
            GameObject target = Instantiate(tierItemPrefab, tier3Parent);
            TierUIItem tierUI = target.GetComponent<TierUIItem>();
            tierUI.SetIcon(item.IconRefrence());
            itemTierDictionary.Add(item, tierUI);
        }
    }
    public void SetForthTier(List<ItemBehaviour> list)
    {
        foreach (var item in list)
        {
            GameObject target = Instantiate(tierItemPrefab, tier4Parent);
            TierUIItem tierUI = target.GetComponent<TierUIItem>();
            tierUI.SetIcon(item.IconRefrence());
            itemTierDictionary.Add(item, tierUI);
        }
    }

    public void CheckItemInDictionary(ItemBehaviour targetItem)
    {
        foreach (var item in itemTierDictionary.Keys)
        {
            if (item.Equals(targetItem))
            {
                itemTierDictionary[targetItem].SetState(true);
            }
        }
    }
}