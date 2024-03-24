using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionItemRecipePrefab : MonoBehaviour
{
    [SerializeField] private List<RecipeIcon> recipeIcons = new();

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    public void InitIcon(PotionItem item)
    {
        icon.sprite = item.firstEffect.sprite;
        itemName.text = item.firstEffect.GetEffectName();

        for(int i = 0; i < item.firstEffect.HerbReceipe().Count; i++)
        {
            recipeIcons[i].SetItem(item.firstEffect.HerbReceipe()[i]);
        }
    }

    public void Close()
    {
        Destroy(gameObject);
    }

}
