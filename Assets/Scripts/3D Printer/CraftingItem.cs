using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingItem : MonoBehaviour
{

    [SerializeField] private GameObject greenIndicator;
    [SerializeField] private GameObject redIndicator;
    [SerializeField] private GameObject greyIndicator;

    [SerializeField] private Image materialImage;
    [SerializeField] private TextMeshProUGUI stackText;

    private ItemBehaviour holdingItem;

    private craftingItemState state;
    

    public void RemoveItemFromPrinterSlot()
    {
        if (PlayerInventory.Instance.HaveEmptySlot(holdingItem,false))
        {
            PlayerInventory.Instance.HaveEmptySlot(holdingItem, true);
            state = craftingItemState.CanFill;
            SetIndicator(holdingItem);
        }
        else
        {
            Debug.Log("Not enough space to add the item to the inventory");
        }
    }


    public void SetIndicator(ItemBehaviour sentMaterial)
    {
        if (state == craftingItemState.Filled)
        {
            Debug.Log("Filled");
            return;
        }

        greenIndicator.SetActive(false);
        redIndicator.SetActive(false);
        greyIndicator.SetActive(false);
        materialImage.sprite = null;
        stackText.text = sentMaterial.CurrentStack().ToString();

        if (sentMaterial.Equals(new EmptyItem()))
        {
            greyIndicator.SetActive(true);
            materialImage.sprite = sentMaterial.IconRefrence();
            state = craftingItemState.DontFill;
            holdingItem = null;
            return;
        }

        materialImage.sprite = sentMaterial.IconRefrence();
        if (PlayerInventory.Instance.HaveItemInInventory(sentMaterial, false))
        {
            greenIndicator.SetActive(true);
            holdingItem = sentMaterial;
            state = craftingItemState.CanFill;
        }
        else
        {
            redIndicator.SetActive(true);
            holdingItem = sentMaterial;
            state = craftingItemState.CantFill;
        }
    }

    public void GetItemFromInventory()
    {
        if(holdingItem == null)
        {
            Debug.Log("Reached empty slot");
            return;
        }


        if (state == craftingItemState.CanFill && PlayerInventory.Instance.HaveItemInInventory(holdingItem, true))
        {
            state = craftingItemState.Filled;
            greenIndicator.SetActive(false);
            redIndicator.SetActive(false);
            greyIndicator.SetActive(false);
            Debug.Log("Filled");
        }
    }

    public void ResetState()
    {
        state = craftingItemState.DontFill;
        holdingItem = null;
        materialImage.sprite = null;
        stackText.text = string.Empty;
    }


    public craftingItemState GetState()
    {
        return state;
    }

    public void SetState(craftingItemState state)
    {
        greyIndicator.SetActive(false);
        redIndicator.SetActive(false);
        greenIndicator.SetActive(false);

        this.state = state;
        if (state == craftingItemState.DontFill)
        {
            greyIndicator.SetActive(true);
        }
        else if (state == craftingItemState.CanFill)
        {
            greenIndicator.SetActive(true);
        }
        else if (state == craftingItemState.CantFill)
        {
            redIndicator.SetActive(true);
        }

    }

    public void SetBluePrint(ItemBehaviour item)
    {
        holdingItem = item;
        if (holdingItem.Equals(new EmptyItem()))
            holdingItem = null;
        materialImage.sprite = item.IconRefrence();
        stackText.text = item.CurrentStack().ToString();
    }


}
