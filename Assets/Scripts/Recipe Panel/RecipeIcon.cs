using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class RecipeIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI itemStack;
    [SerializeField] private bool stackMatters;

    private ItemBehaviour setItem;
    private RecipePanel recipePanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (setItem != null)
        {
            recipePanel = GetComponentInParents<RecipePanel>();
            if (recipePanel == null)
            {
                Debug.LogWarning("HEREHREHRERH");
            }
            recipePanel.IconPrefabClick(setItem);
        }
    }

    public void SetItem(ItemBehaviour target)
    {
        if(target == null)
        {
            setItem = null;
            itemIcon.sprite = null;
            itemText.text = string.Empty;
            return;
        }
        setItem = target;
        itemIcon.sprite = target.IconRefrence();
        itemText.text = target.GetName();
        if (stackMatters)
        {
            if (target.IsStackable())
            {
                itemStack.text = target.CurrentStack().ToString();
            }
            else
                itemStack.gameObject.SetActive(false);
        }

    }

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

}
