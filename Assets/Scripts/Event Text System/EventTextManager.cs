using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        CreateNewText("TEST",Color.black,Color.white);
    }

    public void CreateNewText(string text, Color backgroundColor,Color textColor)
    {
        GameObject textGO = Instantiate(textPrefab, transform);
        textGO.transform.position = startPos.position;
        textGO.GetComponent<EventTextPrefab>().SetText(endPos,text,backgroundColor,textColor);
    }



}
