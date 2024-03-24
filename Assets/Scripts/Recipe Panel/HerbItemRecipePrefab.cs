using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HerbItemRecipePrefab : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private RecipeIcon seedIcon;

    public void InitItem(Herb item)
    {
        icon.sprite = item.IconRefrence();
        itemName.text = item.GetName();

        seedIcon.SetItem(item.GetSeed());
    }

    public void Close()
    {
        Destroy(gameObject);
    }

}
