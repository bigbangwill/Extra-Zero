using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextType { Information, Error, Warning}

public class EventTextManager : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform endPos;

    [SerializeField] private float maxWaitTimer;

    private float currentWaitTimer;

    private List<GameObject> textObjects = new();
    private bool isWainting = false;

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


    private void Update()
    {
        if(textObjects.Count > 0 && !isWainting)
        {
            isWainting = true;
            StartCoroutine(countDown(textObjects[0]));
        }
    }


    public void CreateNewText(string text, TextType textType)
    {
        GameObject textGO = Instantiate(textPrefab, transform);
        textGO.SetActive(false);
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
        textObjects.Add(textGO);

    }

    private IEnumerator countDown(GameObject target)
    {
        target.SetActive(true);
        textObjects.Remove(target);        
        yield return new WaitForSecondsRealtime(.5f);
        isWainting = false;
        yield break;
    }



}
