using ExtraZero.Dialogue;
using System.Collections;
using System.Collections.Generic;
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
    private bool isDone;


    private void Start()
    {
        ShowDialogue(currentDialogue);
    }

    private void Update()
    {
        if (isAdding)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer > textAddingSpeed)
            {
                textObject.text += leftToShow[0];
                leftToShow = leftToShow.Substring(1);
                currentTimer = 0;
                if (leftToShow.Length == 0)
                {
                    GoNextChild();
                }
            }
        }
    }


    public void ShowDialogue(Dialogue input)
    {
        currentDialogue = input;
        currentNode = currentDialogue.GetRootNode();
        textObject.text = string.Empty;
        leftToShow = currentNode.GetText();
        isAdding = true;
        isDone = false;
    }


    public void Skip()
    {

    }


    private void GoNextChild()
    {

    }


}
