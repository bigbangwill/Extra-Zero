using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BasicMaterialScript : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TextMeshProUGUI currentTreshText;
    [SerializeField] private GameObject miniGameObject;
    private Image farmIcon;

    private MaterialItem setItem;
    private PlayerInventory playerInventory;
    private int maxThreshold;
    private int currentThreshold;

    private int currentMiniGameChance = 20;
    private bool miniGameIsOn = false;

    private RectTransform rt;

    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void Init(MaterialItem item,PlayerInventory inventory)
    {

        farmIcon = GetComponent<Image>();
        setItem = item;
        playerInventory = inventory;
        maxThreshold = item.GetMaxThreshold();
        currentThreshold = 0;
        farmIcon.sprite = setItem.GetFarmIcon();
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
    }

    private void Pickaxed()
    {

        currentThreshold++;
        if (currentThreshold >= maxThreshold)
        {
            GiveReward();
            currentThreshold = 0;
        }
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
    }

    // should set it to add to stash and then the player can pick them up.
    private void GiveReward()
    {
        Debug.Log(setItem.CurrentStack());
        if (playerInventory.HaveEmptySlot(setItem, true))
        {
            Debug.Log("Gave the reward");
        }
        else
        {
            Debug.Log("Is full");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Pickaxed();
    }

    private void CheckMiniGame()
    {
        int random = Random.Range(0, 101);
        if (!miniGameIsOn && random <= currentMiniGameChance)
        {
            miniGameObject.transform.position = new Vector2(Random.Range(rt.rect.xMin, rt.rect.xMax), Random.Range(rt.rect.yMin, rt.rect.yMax));
            miniGameObject.SetActive(true);
            miniGameIsOn = true;
        }
    }

    public void MiniGameClicked()
    {
        Debug.Log("Double Damage");
        Pickaxed();
        Pickaxed();
        miniGameObject.SetActive(false);
        miniGameIsOn = false;
    }

    public void DidntHitMiniGame()
    {
        miniGameObject.SetActive(false);
        miniGameIsOn = false;
    }
}