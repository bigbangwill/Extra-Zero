using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,
    IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    
    public int slotNumber;

    private IStashable stashable;
    private Image imageComponent;
    private TextMeshProUGUI stackText;

    private bool isStarted = false;

    private ItemBehaviour itemSlot;

    #region Graphic raycast related
    private Vector2 startPos;
    private bool isDragging = false;
    private EventSystem m_EventSystem;
    private PointerEventData m_PointerEventData;

    private GraphicRaycaster m_GraphicRaycaster;

    #endregion



    private void Start()
    {
        imageComponent = GetComponent<Image>();
        stackText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        isStarted = true;
        stackText.text = " ";

        m_EventSystem = EventSystem.current;
        m_GraphicRaycaster = GetComponentInParents<GraphicRaycaster>();
    }

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
            if (itemSlot.IsStackable())
                stackText.text = itemSlot.CurrentStack().ToString();
            else stackText.text = " ";
             
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

    

    public void OnDrag(PointerEventData eventData)
    {
        if (itemSlot.ItemTypeValue() == ItemType.empty)
        {
            return;
        }
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemSlot.ItemTypeValue() == ItemType.empty)
        {
            return;
        }
        startPos = transform.position;
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (itemSlot.ItemTypeValue() == ItemType.empty)
        {
            return;
        }        
        m_PointerEventData = new(m_EventSystem);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new();
        m_GraphicRaycaster.Raycast(m_PointerEventData, results);
        if (results.Count <= 1)
        {
            Debug.Log("Throw out");
            transform.position = startPos;
            stashable.RemoveItemFromInventory(slotNumber);
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
                PlayerInventory.Instance.SwapItemInInventory(selfSlotNumber,targetSlotNumber);
                break;
            }
            transform.position = startPos;
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
        if (itemSlot.ItemTypeValue() == ItemType.empty)
        {
            return;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (itemSlot.ItemTypeValue() == ItemType.empty || isDragging)
        {
            return;
        }
        PlayerInventory.Instance.SetActiveItem(slotNumber);
    }
}