using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class BiomeScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private GameObject dragObject;
    private enum SeedToHold { Chamomile, Sage, Lavander, Hellebore, Patchouli };

    [SerializeField] SeedToHold holdingSeedEnum;

    private Image seedIcon;
    private Seed holdingSeed;

    private List<Action<Seed>> queuedActions = new();



    // for herb holding stats
    private int maxStack = 10;
    private int currentStack;
    private float refilTimer = 5;
    private float currentRefilTimer;

    [SerializeField] private Image cooldownFillImage;
    [SerializeField] private TextMeshProUGUI currentHoldingStack;

    private bool isLocked = false;
    [SerializeField] private GameObject lockedGO;


    private float savedTime;


    private void Awake()
    {
        seedIcon = dragObject.GetComponent<Image>();
        switch (holdingSeedEnum)
        {
            case SeedToHold.Chamomile: holdingSeed = new Seed.Chamomile(1);break;
            case SeedToHold.Sage: holdingSeed = new Seed.Sage(1);break;
            case SeedToHold.Lavander: holdingSeed = new Seed.Lavender(1); break;
            case SeedToHold.Hellebore : holdingSeed = new Seed.Hellebore(1); break;
            case SeedToHold.Patchouli : holdingSeed = new Seed.Patchouli(1); break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
        seedIcon.sprite = holdingSeed.IconRefrence();
        savedTime = Time.time;
        currentRefilTimer = 0;
        currentStack = 0;
    }


    private void OnEnable()
    {
        //currentRefilTimer = 0;
        //currentStack = 0;
        //UpdateText();
        float currentSavedTime = Time.time - savedTime;
        int count = (int)(currentSavedTime / refilTimer);
        if (count > 0)
        {
            if (count + currentStack >= maxStack)
            {
                currentStack = maxStack;
            }
            else
            {
                currentStack += count;
            }
        }
        currentRefilTimer += currentSavedTime % refilTimer;
        UpdateText();
    }

    private void OnDisable()
    {
        savedTime = Time.time;
    }


    private void Update()
    {
        if (currentStack < maxStack)
        {
            currentRefilTimer += Time.deltaTime;
            cooldownFillImage.fillAmount = currentRefilTimer / refilTimer;
            if (currentRefilTimer >= refilTimer)
            {
                TimerMet();
                currentRefilTimer = 0;
            }
        }
    }

    public int GetSeedTier()
    {
        return holdingSeed.GetSeedTier();
    }

    public void SetLockedState(bool locked)
    {
        isLocked = locked;
        lockedGO.SetActive(locked);
    }

    private void TimerMet()
    {
        if (currentStack < maxStack)
        {
            currentStack++;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        currentHoldingStack.text = currentStack + " / " + maxStack;
    }

    private bool canDrag = false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked)
            return;
        queuedActions.Clear();
        if (currentStack > 0)
        {
            dragObject.SetActive(true);
            currentStack--;
            UpdateText();
            canDrag = true;
        }
        else
        {
            canDrag = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(canDrag)
            dragObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canDrag)
        {
            Seed cloneSeed = (Seed)holdingSeed.Clone();
            dragObject.SetActive(false);
            foreach (var action in queuedActions)
            {
                action(cloneSeed);
            }
            RaycastForHerb(eventData,cloneSeed);
            canDrag = false;
        }
    }

    private void RaycastForHerb(PointerEventData eventData,Seed readySeed)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Farm Pouch"))
            {
                SeedFarmPouch pouch = result.gameObject.GetComponent<SeedFarmPouch>();
                pouch.HitWithSeed(readySeed);
            }
        }
    }

    public void CollidedWithaBonus(Action<Seed> action)
    {
        queuedActions.Add(action);
    }
}
