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
    [SerializeField] private Image cooldownImage;

    [SerializeField] private GameObject lockedGO;
    private bool isLocked = false;

    private Image farmIcon;

    private MaterialItem setItem;
    private PlayerInventory playerInventory;
    private int maxThreshold;
    private int currentThreshold;

    private int currentMiniGameChance = 50;
    private bool miniGameIsOn = false;
    private int potionValue = 0;

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
        cooldownImage.sprite = setItem.GetFarmIcon();
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
    }

    public void SetLockedState(bool locked)
    {
        if (locked)
        {
            isLocked = true;
            lockedGO.SetActive(true);
        }
        else
        {
            isLocked = false;
            lockedGO.SetActive(false);
        }
    }


    public int GetTier()
    {
        return setItem.GetItemTier();
    }

    public void StrenghtPotionBuff()
    {
        potionValue = 1;
    }

    public void StrenghtPotionReset()
    {
        potionValue = 0;
    }

    private void Pickaxed()
    {
        if (isPunishing)
            return;
        if(miniGameIsOn)
        {
            DidntHitMiniGame();
            return;
        }
        CheckMiniGame();
        currentThreshold += 1 + potionValue;
        if (currentThreshold >= maxThreshold)
        {
            GiveReward();
            currentThreshold = 0;
        }
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
    }

    private void Pickaxed(int count)
    {
        currentThreshold += count + potionValue;
        if (currentThreshold >= maxThreshold)
        {
            GiveReward();
            currentThreshold -= maxThreshold;
        }
        currentTreshText.text = $"{currentThreshold} / {maxThreshold}";
        CheckMiniGame();
    }

    // should set it to add to stash and then the player can pick them up.
    private void GiveReward()
    {
        if (!playerInventory.HaveEmptySlot(setItem, true))
        {
            Debug.Log("Is full");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isLocked)
            Pickaxed();
    }

    private void CheckMiniGame()
    {
        int random = Random.Range(0, 101);
        if (!miniGameIsOn && random <= currentMiniGameChance)
        {
            miniGameObject.transform.localPosition = new Vector2(Random.Range(rt.rect.xMin, rt.rect.xMax), Random.Range(rt.rect.yMin, rt.rect.yMax));
            miniGameObject.SetActive(true);
            miniGameIsOn = true;
        }
    }

    public void MiniGameClicked()
    {
        miniGameObject.SetActive(false);
        miniGameIsOn = false;
        Pickaxed(2);
    }

    public void DidntHitMiniGame()
    {
        miniGameObject.SetActive(false);
        miniGameIsOn = false;
        StartCoroutine(Punish());
    }

    private bool isPunishing = false;
    private float punishTimer = 3;
    private float currentPunishTimer = 0;

    private void OnEnable()
    {
        if (currentPunishTimer > 0)
        {
            StartCoroutine(ResumePunish());
        }
    }

    private void OnDisable()
    {
        StopCoroutine(Punish());
    }

    private IEnumerator Punish()
    {
        isPunishing = true;
        cooldownImage.gameObject.SetActive(true);
        currentPunishTimer = punishTimer;
        while (true)
        {
            currentPunishTimer -= Time.deltaTime;
            cooldownImage.fillAmount = currentPunishTimer / punishTimer;
            if(currentPunishTimer <= 0)
            {
                isPunishing = false;
                cooldownImage.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }

    private IEnumerator ResumePunish()
    {
        while (true)
        {
            currentPunishTimer -= Time.deltaTime;
            cooldownImage.fillAmount = currentPunishTimer / punishTimer;
            if (currentPunishTimer <= 0)
            {
                isPunishing = false;
                cooldownImage.gameObject.SetActive(true);
                break;
            }
            yield return null;
        }
    }

}