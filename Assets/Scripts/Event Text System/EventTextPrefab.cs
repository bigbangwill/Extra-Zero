using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventTextPrefab : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textContext;
    [SerializeField] private float speed;

    private Image backgroundImage;
    private Transform endPos;
    private float dist;

    public void SetText(Transform endPos,string text, Color textColor,Color backgroundColor)
    {
        textContext.text = text;
        textContext.color = textColor;
        backgroundImage.color = backgroundColor;
        this.endPos = endPos;
        dist = Vector2.Distance(transform.position, endPos.position);
    }

    private void Start()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void Update()
    {
        float distance = Vector2.Distance(transform.position, endPos.position);
        if (distance <= 0)
        {
            Destroy(gameObject);
            return;
        }
        textContext.alpha = distance / dist;
        var tempColor = backgroundImage.color;
        tempColor.a = distance / dist;
        backgroundImage.color = tempColor;
        transform.position = Vector2.Lerp(transform.position, endPos.position, Time.deltaTime * speed);
    }


}
