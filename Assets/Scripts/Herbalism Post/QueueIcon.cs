using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QueueIcon : MonoBehaviour
{

    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI stackText;

    public void SetImage(Sprite sprite)
    {
        Image.sprite = sprite;
    }
    public void SetText(string text)
    {
        stackText.text = text;
    }
    
    
}