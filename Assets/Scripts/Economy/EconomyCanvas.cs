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

    private void Start()
    {
        RefreshUI();
    }

    private void OnEnable()
    {
        EconomyManager.Instance.AddListener(RefreshUI);
    }

    private void OnDisable()
    {
        if(EconomyManager.Instance != null) 
            EconomyManager.Instance.RemoveListener(RefreshUI);
    }

    public void RefreshUI()
    {
        string inGameCurrentStack = EconomyManager.Instance.InGameCurrencyCurrentStack.ToString();
        string inGameMaxStack = EconomyManager.Instance.InGameCurrencyMaxStack.ToString();
        textInGame.text = inGameCurrentStack + " / " + inGameMaxStack;

        string outGameCurrentStack = EconomyManager.Instance.OutGameCurrencyCurrentStack.ToString();
        string outGameMaxStack = EconomyManager.Instance.OutGameCurrencyMaxStack.ToString();

        textOutGame.text = outGameCurrentStack + " / " + outGameMaxStack;
    }


    
}