using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedItemRecipePrefab : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;

    [SerializeField] private Image biomeIcon;

    [SerializeField] private Sprite chamomileBiome;
    [SerializeField] private Sprite sageBiome;
    [SerializeField] private Sprite lavenderBiome;
    [SerializeField] private Sprite patchouliBiome;
    [SerializeField] private Sprite helleboreBiome;


    public void InitIcon(Seed item)
    {
        icon.sprite = item.IconRefrence();
        itemName.text = item.GetName();

        switch (item.GetName())
        {
            case "Chamomile": biomeIcon.sprite = chamomileBiome; break;
            case "Lavender": biomeIcon.sprite = lavenderBiome; break;
            case "Sage": biomeIcon.sprite = sageBiome; break;
            case "Patchouli": biomeIcon.sprite = patchouliBiome; break;
            case "Hellebore": biomeIcon.sprite = helleboreBiome; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }

    }

    public void Close()
    {
        Destroy(gameObject);
    }


}
