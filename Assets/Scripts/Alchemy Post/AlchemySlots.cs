using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlchemySlots : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private AlchemyPost alchemyPost;
    [SerializeField] private EffectSlots effectSlots;
    [SerializeField] private int slotCount;

    private RectTransform rectTransform;
    private Herb holdingHerb;
    private Image image;

    private bool isLocked;
    [SerializeField] private Sprite lockedImage;
    [SerializeField] private Sprite unlockedImage = null;

    private int herbCost = 5;

    private PlayerInventoryRefrence inventoryRefrence;
    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
    }


    private void Start()
    {
        LoadSORefrence();
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }


    public void SetLocked()
    {
        isLocked = true;
        if (image == null)
            image = GetComponent<Image>();
        image.sprite = lockedImage;
    }

    public void SetUnlocked()
    {
        isLocked = false;
        if (image == null)
            image = GetComponent<Image>();
        image.sprite = unlockedImage;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(!isLocked)
            alchemyPost.SlotsOnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked)
            return;
        PointerEventData pointerEventData = new(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(pointerEventData, results);

        foreach(RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("PopUpSlot"))
            {
                Herb herb = result.gameObject.GetComponent<PopUpSlot>().HerbSelected();
                // To check if there was a value set before in the holdingHerb to make sure if there
                // is anything to be added to their current stack.
                if (holdingHerb != null)
                {
                    //if there is a new herb selected to add and remove the related values.
                    if (herb != null)
                    {
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() + herbCost);
                        holdingHerb = herb;
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() - herbCost);
                        image.sprite = holdingHerb.IconRefrence();
                        effectSlots.SetHerbForEffect(holdingHerb, slotCount);
                        break;
                    }
                    //if there is no new herb selected just to add the value.
                    else
                    {
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() + herbCost);
                        holdingHerb = null;
                        image.sprite = null;
                        effectSlots.SetHerbForEffect(holdingHerb, slotCount);
                        break;
                    }
                }
                else
                {
                    if(herb != null)
                    {
                        holdingHerb = herb;
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() - herbCost);
                        image.sprite = holdingHerb.IconRefrence();
                        effectSlots.SetHerbForEffect(holdingHerb, slotCount);
                        break;
                    }
                }
            }
        }
        alchemyPost.SlotsOnPointerUp();
    }

    public void Reset()
    {
        if (holdingHerb != null)
        {
            holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() + herbCost);
            holdingHerb = null;
            image.sprite = null;
        }
    }

    public void PotionCreated()
    {
        Debug.Log("1");
        if (holdingHerb != null)
        {
            Debug.Log("2");
            int currentStack = holdingHerb.CurrentStack();
            holdingHerb.SetCurrentStack(herbCost);
            Debug.Log(herbCost + "HERB COST");
            Debug.Log(holdingHerb.CurrentStack() + " HOLDING HERB STACK");
            if (inventoryRefrence.val.HaveItemInInventory(holdingHerb, true))
            {
                Debug.Log("3");
                holdingHerb.SetCurrentStack(currentStack);
                holdingHerb = null;
                image.sprite = null;
            }
            else
            {
                Debug.LogWarning("Player doesnt have the required Item");
            }
        }
    }

    public void SetHerbCount(int count)
    {
        herbCost = count;
    }

    private void OnDisable()
    {
        //Reset();
    }

}