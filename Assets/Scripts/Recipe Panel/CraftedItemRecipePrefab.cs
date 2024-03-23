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


    public void InitIcon(CraftedItem item)
    {
        List<MaterialItem> materials = item.GetCraftingRecipe().materialsList;

        for(int i = 0; i < materials.Count; i++)
        {
            recipeIcon[i].SetItem(materials[i]);
        }

        //if (materials[0] != null)
        //    firstIcon.SetItem(item.GetCraftingRecipe().materialsList[0]);
        //else
        //    firstIcon.SetItem(null);
        //if (materials[1] != null)
        //    secondIcon.SetItem(item.GetCraftingRecipe().materialsList[1]);
        //else
        //    secondIcon.SetItem(null);
        //if (materials[2] != null)
        //    thirdIcon.SetItem(item.GetCraftingRecipe().materialsList[2]);
        //else 
        //    thirdIcon.SetItem(null);
        icon.sprite = item.IconRefrence();
        itemName.text = item.GetName();
    }

    public void ClosePrefab()
    {
        Destroy(gameObject);
    }

    

}
