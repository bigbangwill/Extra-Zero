using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EconomyCanvas : MonoBehaviour
{

    [SerializeField] private Image inGameIcon;
    [SerializeField] private TextMeshProUGUI textInGame;
    [SerializeField] private Image outGameIcon;
    [SerializeField] private TextMeshProUGUI textOutGame;

    private static EconomyCanvas instance;

    private EconomyManagerRefrence economyManagerRefrence;

    private bool isInitialized = false;

    private void LoadSORefrence()
    {
        economyManagerRefrence = (EconomyManagerRefrence)FindSORefrence<EconomyManager>.FindScriptableObject("Economy Manager Refrence");
    }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }



    private void OnEnable()
    {
        if(isInitialized)
        {
            LoadSORefrence();
            economyManagerRefrence.val.AddListener(RefreshUI);
        }
    }
    private void Start()
    {
        isInitialized = true;
        OnEnable();
        RefreshUI();
    }

    private void OnDisable()
    {
        if(economyManagerRefrence.val != null) 
            economyManagerRefrence.val.RemoveListener(RefreshUI);
    }

    public void RefreshUI()
    {
        string inGameCurrentStack = economyManagerRefrence.val.InGameCurrencyCurrentStack.ToString();
        string inGameMaxStack = economyManagerRefrence.val.InGameCurrencyMaxStack.ToString();
        textInGame.text = inGameCurrentStack + " / " + inGameMaxStack;

        string outGameCurrentStack = economyManagerRefrence.val.OutGameCurrencyCurrentStack.ToString();
        string outGameMaxStack = economyManagerRefrence.val.OutGameCurrencyMaxStack.ToString();

        textOutGame.text = outGameCurrentStack + " / " + outGameMaxStack;
    }


    
}