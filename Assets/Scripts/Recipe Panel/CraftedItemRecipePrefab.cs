using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftedItemRecipePrefab : MonoBehaviour
{
    [SerializeField] private RecipeIcon firstIcon;
    [SerializeField] private RecipeIcon secondIcon;
    [SerializeField] private RecipeIcon thirdIcon;

    [SerializeField] private List<RecipeIcon> recipeIcon = new();

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private Sprite empty;

    public void InitIcon(CraftedItem item)
    {
        List<MaterialItem> materials = item.GetCraftingRecipe().materialsList;
        for(int i = 0; i < materials.Count; i++)
        {
            recipeIcon[i].SetItem(materials[i]);
        }

        if (materials.Count == 1)
        {
            Destroy(recipeIcon[1].gameObject);
            Destroy(recipeIcon[2].gameObject);
        }
        else if (materials.Count == 2)
        {
            Destroy(recipeIcon[2].gameObject);
        }
        icon.sprite = item.IconRefrence();
        itemName.text = item.GetName();
    }

    public void ClosePrefab()
    {
        Destroy(gameObject);
    }

    

}
