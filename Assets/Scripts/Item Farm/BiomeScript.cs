using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class BiomeScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private GameObject dragObject;
    private enum SeedToHold { Chamomile,Sage,Lavander};

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



    private void Start()
    {
        seedIcon = dragObject.GetComponent<Image>();
        switch (holdingSeedEnum)
        {
            case SeedToHold.Chamomile: holdingSeed = new Seed.Chamomile(1);break;
            case SeedToHold.Sage: holdingSeed = new Seed.Sage(1);break;
            case SeedToHold.Lavander: holdingSeed = new Seed.Lavender(1); break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
        seedIcon.sprite = holdingSeed.IconRefrence();
    }


    private void OnEnable()
    {
        currentRefilTimer = 0;
        currentStack = 0;
        UpdateText();
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
