using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderPostUI : MonoBehaviour
{

    [SerializeField] private GameObject imagePrefab;
    [SerializeField] private GameObject filledImagePrefab;
    [SerializeField] private GameObject unFilledImagePrefab;
    [SerializeField] private Image image;


    /// <summary>
    /// To create the related prefab in the world canvas and set their icons.
    /// </summary>
    /// <param name="order"></param>
    public void SetOrderImage(Order order)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach(ItemBehaviour item in order.GetOrderItems())
        {
            GameObject go = Instantiate(imagePrefab);
            go.GetComponent<Image>().sprite = item.IconRefrence();
            go.GetComponentInChildren<TextMeshProUGUI>().text = item.CurrentStack().ToString();
            go.transform.SetParent(transform, false);
        }
        foreach (ItemBehaviour item in order.GetFilledItems())
        {
            GameObject go = Instantiate(filledImagePrefab);
            go.GetComponent<Image>().sprite = item.IconRefrence();
            go.transform.SetParent(transform, false);
        }
    }

    public void SetUnfullfilledOrderImage(Order order)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemBehaviour item in order.GetOrderItems())
        {
            GameObject go = Instantiate(unFilledImagePrefab);
            go.GetComponent<Image>().sprite = item.IconRefrence();
            go.transform.SetParent(transform, false);
        }
        foreach (ItemBehaviour item in order.GetFilledItems())
        {
            GameObject go = Instantiate(filledImagePrefab);
            go.GetComponent<Image>().sprite = item.IconRefrence();
            go.transform.SetParent(transform, false);
        }
    }
    
    
}