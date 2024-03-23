using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePanel : MonoBehaviour
{
    [SerializeField] private List<OrderPost> orderPosts = new();

    [SerializeField] private GameObject craftingItemPrefab;
    [SerializeField] private GameObject potionRecipePrefab;
    [SerializeField] private GameObject herbRecipePrefab;
    [SerializeField] private GameObject seedRecipePrefab;
    [SerializeField] private GameObject materialRecipePrefab;

    [SerializeField] private GameObject iconPrefab;

    [Header("Transfrom")]
    [SerializeField] private Transform scrollView;
    [SerializeField] private Transform recipeHolder;
    
 
    private List<ItemBehaviour> orderItems = new();




    public void ReloadCurrentOrders()
    {
        orderItems.Clear();
        foreach (Transform item in scrollView.transform)
        {
            Destroy(item.gameObject);
        }
        foreach(var post in orderPosts)
        {
            Order currentOrder = post.GetCurrentOrder();
            if (currentOrder != null)
            {
                foreach (var item in currentOrder.GetOrderItems())
                {
                    orderItems.Add(item);
                }
            }
        }

        foreach (var item in orderItems)
        {
            GameObject iconGO = Instantiate(iconPrefab, scrollView);
            iconGO.GetComponent<RecipeIcon>().SetItem(item);
        }        
    }

    public void IconPrefabClick(ItemBehaviour target)
    {
        switch (target.ItemTypeValue())
        {
            case ItemType.craftedItem: CreateCraftedItem((CraftedItem)target); break;
            case ItemType.potion: CreatePotionItem((PotionItem)target); break;
            case ItemType.herb: CreateHerbItem((Herb)target); break;
            case ItemType.seed: CreateSeedItem((Seed)target); break;
            case ItemType.material: CreateMaterialItem((MaterialItem)target); break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }


    private void CreateCraftedItem(CraftedItem item)
    {
        GameObject GO = Instantiate(craftingItemPrefab, recipeHolder);
        GO.GetComponent<CraftedItemRecipePrefab>().InitIcon(item);
    }

    private void CreatePotionItem(PotionItem item)
    {

    }

    private void CreateHerbItem(Herb item)
    {

    }

    private void CreateSeedItem(Seed item)
    {

    }

    private void CreateMaterialItem(MaterialItem item)
    {

    }
    
}