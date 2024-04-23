using ExtraZero.Dialogue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{

    [SerializeField] private Dialogue currentDialogue;
    [SerializeField] private TextMeshProUGUI textObject;

    [SerializeField] private float textAddingSpeed;


    private float currentTimer = 0;

    private DialogueNode currentNode;
    private string leftToShow;


    private bool isAdding;

    private DialogueManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (DialogueManagerRefrence)FindSORefrence<DialogueManager>.FindScriptableObject("Dialogue Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Update()
    {
        if (isAdding)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > textAddingSpeed)
            {
                textObject.text += leftToShow[0];
                leftToShow = leftToShow[1..];
                currentTimer = 0;
                if (leftToShow.Length == 0)
                {
                    isAdding = false;
                }
            }
        }
    }


    public void ShowDialogue(Dialogue input)
    {
        gameObject.SetActive(true);
        currentDialogue = input;
        currentNode = currentDialogue.GetRootNode();
        textObject.text = string.Empty;
        leftToShow = currentNode.GetText();
        isAdding = true;
    }


    public void Skip()
    {
        if (isAdding)
        {
            isAdding = false;
            textObject.text = currentNode.GetText();
        }
        else
        {
            GoNextChild();
        }
    }


    private void GoNextChild()
    {
        var next = currentDialogue.GetAllChildren(currentNode);
        if (next.Any())
        {
            currentNode = next.First();
            textObject.text = string.Empty;
            leftToShow = currentNode.GetText();
            isAdding = true;
        }
        else
        {
            FinishedDialogue();
            return;
        }
    }

    public void FinishedDialogue()
    {
        gameObject.SetActive(false);
    }


}
