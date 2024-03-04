using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class BiomeScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private GameObject dragObject;
    private enum SeedToHold { Chamomile,Sage,Lavander};

    [SerializeField] SeedToHold holdingSeedEnum;

    private Image seedIcon;
    private Seed holdingSeed;

    private List<Action<Seed>> queuedActions = new();

    //private List<Func<Seed>> queuedFunctions = new();

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


    public void OnBeginDrag(PointerEventData eventData)
    {
        dragObject.SetActive(true);
        queuedActions.Clear();
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragObject.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Seed cloneSeed = (Seed)holdingSeed.Clone();
        dragObject.SetActive(false);
        foreach (var action in queuedActions)
        {
            action(cloneSeed);
        }

        RaycastForHerb(eventData,cloneSeed);
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
