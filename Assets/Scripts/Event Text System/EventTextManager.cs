using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextType { Information, Error, Warning}

public class EventTextManager : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    private EventTextManagerRefrence refrence;

    private void SetSORefrence()
    {
        refrence = (EventTextManagerRefrence)FindSORefrence<EventTextManager>.FindScriptableObject("Event Text Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetSORefrence();
    }

    private void Start()
    {
        CreateNewText("HEY",TextType.Error);
        CreateNewText("HEY", TextType.Information);
        CreateNewText("HEY", TextType.Warning);
    }


    public void CreateNewText(string text, TextType textType)
    {
        GameObject textGO = Instantiate(textPrefab, transform);
        textGO.transform.position = startPos.position;
        Color backgroundColor;
        Color textColor;
        switch(textType)
        {
            case TextType.Information: backgroundColor = Color.cyan;textColor = Color.gray; break;
            case TextType.Warning: backgroundColor = Color.yellow;textColor = Color.black; break;
            case TextType.Error: backgroundColor = Color.red;textColor = Color.black; break;
            default: Debug.LogWarning("CHECK HERE ASAP"); backgroundColor = Color.white;textColor = Color.black; break;
        }
        textGO.GetComponent<EventTextPrefab>().SetText(endPos, text, textColor, backgroundColor);
    }



}
