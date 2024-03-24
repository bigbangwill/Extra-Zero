using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialItemRecipePrefab : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private Image farmSpotIcon;

    [SerializeField] private Sprite plasticSpot;
    [SerializeField] private Sprite CeramicSpot;
    [SerializeField] private Sprite TitaniumAlloySpot;
    [SerializeField] private Sprite AluminumAlloySpot;
    [SerializeField] private Sprite StainlessSteelSpot;


    public void InitIcon(MaterialItem item)
    {
        icon.sprite = item.IconRefrence();
        itemName.text = item.GetName();

        switch (item.GetName())
        {
            case "Plastic": farmSpotIcon.sprite = plasticSpot; break;
            case "Ceramic": farmSpotIcon.sprite = CeramicSpot; break;
            case "Titanium": farmSpotIcon.sprite = TitaniumAlloySpot; break;
            case "Aluminum": farmSpotIcon.sprite = AluminumAlloySpot; break;
            case "Stainless Steel": farmSpotIcon.sprite = StainlessSteelSpot; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }

    }

    public void Close()
    {
        Destroy(gameObject);
    }

}
