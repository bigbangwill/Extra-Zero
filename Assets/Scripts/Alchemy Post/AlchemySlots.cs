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

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        alchemyPost.SlotsOnPointerDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() + 5);
                        holdingHerb = herb;
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() - 5);
                        image.sprite = holdingHerb.IconRefrence();
                        effectSlots.SetHerbForEffect(holdingHerb, slotCount);
                        break;
                    }
                    //if there is no new herb selected just to add the value.
                    else
                    {
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() + 5);
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
                        holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() - 5);
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
            holdingHerb.SetCurrentStack(holdingHerb.CurrentStack() + 5);
            holdingHerb = null;
            image.sprite = null;
        }
    }

    public void PotionCreated()
    {
        if (holdingHerb != null)
        {
            int currentStack = holdingHerb.CurrentStack();
            holdingHerb.SetCurrentStack(5);
            if (PlayerInventory.Instance.HaveItemInInventory(holdingHerb, false))
            {
                PlayerInventory.Instance.HaveItemInInventory(holdingHerb,true);
                holdingHerb.SetCurrentStack(currentStack);
                holdingHerb = null;
                image.sprite = null;
            }
            else
            {
                Debug.Log("Player doesnt have the required Item");
            }
        }
    }

    private void OnDisable()
    {
        Reset();
    }

}