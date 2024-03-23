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

    [Header("Transfroms")]
    [SerializeField] private Transform RecipeHolderTransform;
    
 
    private List<ItemBehaviour> orderItems = new();




    public void ReloadCurrentOrders()
    {
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
            switch (item.ItemTypeValue())
            {
                case ItemType.craftedItem: CreateCraftedItem((CraftedItem)item); break;
                case ItemType.potion: CreatePotionItem((PotionItem)item); break;
                case ItemType.herb: CreateHerbItem((Herb)item); break;
                case ItemType.seed: CreateSeedItem((Seed)item); break;
                case ItemType.material: CreateMaterialItem((MaterialItem)item);break;
                default: Debug.LogWarning("CHECK HERE ASAP"); break;
            }
        }
    }


    private void CreateCraftedItem(CraftedItem item)
    {

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