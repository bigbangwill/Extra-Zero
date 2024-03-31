using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    
    public int slotNumber;

    private IStashable stashable;
    private Image imageComponent;
    private TextMeshProUGUI stackText;

    private bool isStarted = false;

    private ItemBehaviour itemSlot;

    private LavaItemRemover lavaRefrence;
    private ItemStashLevelObject itemStashLevelObject;

    private OrderPost currentPost;

    #region Graphic raycast related
    private Vector2 startPos;
    private bool isDragging = false;
    private EventSystem m_EventSystem;
    private PointerEventData m_PointerEventData;

    private GraphicRaycaster m_GraphicRaycaster;

    #endregion

    private PlayerMovementRefrence playerMovementRefrence;

    private void LoadSORefrence()
    {
        playerMovementRefrence = (PlayerMovementRefrence)FindSORefrence<PlayerMovement>.FindScriptableObject("Player Movement Refrence");
    }



    private void Start()
    {
        LoadSORefrence();
        imageComponent = GetComponent<Image>();
        stackText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        isStarted = true;
        stackText.text = " ";

        m_EventSystem = EventSystem.current;
        m_GraphicRaycaster = GetComponentInParents<GraphicRaycaster>();

    }

    /// <summary>
    /// This method should get called during the initialization of the related stash.
    /// </summary>
    /// <param name="stashable"></param>
    public void SetStashable(IStashable stashable)
    {
        this.stashable = stashable;
    }

    // To loop through all of the parents of the object to find the related object
    public T GetComponentInParents<T>() where T : Component
    {
        T component = null;
        Transform currentTransform = transform;

        while (component == null && currentTransform != null)
        {
            component = currentTransform.GetComponent<T>();
            currentTransform = currentTransform.parent;
        }

        return component;
    }

    private void Update()
    {
        if (imageComponent.sprite == null)
        {
            itemSlot = stashable.ItemRefrence(slotNumber);
            GetComponent<Image>().sprite = itemSlot.IconRefrence();
            RefreshText();
        }
    }

    // To reset the icon of the item
    public void Reset()
    {
        if (isStarted)
        {
            imageComponent.sprite = null;
            stackText.text = " ";
        }
    }

    public void RefreshText()
    {
        if (itemSlot.IsStackable())
            stackText.text = itemSlot.CurrentStack().ToString();
        else stackText.text = " ";
    }

    

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot.GetItemTypeValue() == ItemType.empty)
        {
            return;
        }
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot.GetItemTypeValue() == ItemType.empty)
        {
            return;
        }
        startPos = transform.position;
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (itemSlot.GetItemTypeValue() == ItemType.empty)
        {
            return;
        }        
        m_PointerEventData = new(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new();
        m_GraphicRaycaster.Raycast(m_PointerEventData, results);
        if (results.Count <= 1)
        {
            if (RaycastWorldSpace())
            {
                transform.position = startPos;
                return;
            }
            transform.position = startPos;
            //stashable.RemoveItemFromInventory(slotNumber);
        }
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Item Slot"))
            {
                ItemSlotUI targetSwap = result.gameObject.transform.GetChild(0).GetComponent<ItemSlotUI>();
                int targetSlotNumber = targetSwap.slotNumber;
                int selfSlotNumber = slotNumber;
                targetSwap.slotNumber = slotNumber;
                if(targetSlotNumber == slotNumber)
                {
                    Debug.Log("Target self");
                    transform.position = startPos;
                    return;
                }
                slotNumber = targetSlotNumber;
                targetSwap.SwapToNewPostion(transform.parent.gameObject);
                SwapToNewPostion(result.gameObject);
                stashable.SwapItemInInventory(selfSlotNumber,targetSlotNumber);
                break;
            }
            else if (result.gameObject.CompareTag("Order Post"))
            {
                Debug.Log("Targeted the right one");
            }
            transform.position = startPos;
        }
    }

    

    private bool RaycastWorldSpace()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
        foreach (var n in hit)
        {
            GameObject go = n.collider.gameObject;
            if (go.CompareTag("Order Post"))
            {
                //go.GetComponent<OrderPost>().InsertingItem(itemSlot, slotNumber);
                currentPost = go.GetComponent<OrderPost>();
                NavmeshReachableInformation navInfo = new(currentPost.GetReachingTransfrom().position, ReachedInserting);
                playerMovementRefrence.val.MovetoTarget(navInfo);
                return true;
            }
            else if (go.CompareTag("Lava Remover"))
            {
                if (lavaRefrence == null)
                {
                    lavaRefrence = go.GetComponent<LavaItemRemover>();
                }
                NavmeshReachableInformation navInfo = new(lavaRefrence.GetReachingTransform().position, ReachedLava);
                playerMovementRefrence.val.MovetoTarget(navInfo);
                return true;
            }
            else if (go.CompareTag("Item Stash"))
            {
                if (itemStashLevelObject == null)
                {
                    itemStashLevelObject = go.GetComponent<ItemStashLevelObject>();
                }
                NavmeshReachableInformation navInfo = new(itemStashLevelObject.GetReachingTransform().position, ReachedStash);
                playerMovementRefrence.val.MovetoTarget(navInfo);
                return true;
            }
        }
        return false;
    }

    private void ReachedInserting()
    {
        if(currentPost != null)
        {
            currentPost.InsertingItem(itemSlot, slotNumber);
        }
        currentPost = null;
    }

    private void ReachedLava()
    {
        if (lavaRefrence != null)
        {
            lavaRefrence.InsertItem(slotNumber);
        }
    }
    private void ReachedStash()
    {
        if (itemStashLevelObject != null)
        {
            itemStashLevelObject.AddItemToStash(itemSlot, slotNumber);
        }
    }


    /// <summary>
    /// To swap to new parent and slot. Should be called from outside of the script by another itemslot.
    /// </summary>
    /// <param name="parent"></param>
    public void SwapToNewPostion(GameObject parent)
    {
        transform.SetParent(parent.transform, true);
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = Vector3.zero;
    }

    public void OnDrop(PointerEventData eventData)
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemSlot.GetItemTypeValue() == ItemType.empty)
        {
            return;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemSlot.GetItemTypeValue() == ItemType.empty || isDragging)
        {
            return;
        }
        stashable.SetActiveItem(slotNumber);
    }
}