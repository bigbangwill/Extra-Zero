using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class BiomeScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private GameObject dragObject;
    private enum SeedToHold { Chamomile,Sage,Lavander};

    [SerializeField] SeedToHold holdingSeedEnum;

    private Seed holdingSeed;

    private void Start()
    {
        switch (holdingSeedEnum)
        {
            case SeedToHold.Chamomile: holdingSeed = new Seed.Chamomile(1);break;
            case SeedToHold.Sage: holdingSeed = new Seed.Sage(1);break;
            case SeedToHold.Lavander: holdingSeed = new Seed.Lavender(1); break;
            default: Debug.LogWarning("CHECK HERE ASAP"); break;
        }
    }




    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastForHerb(eventData);
    }

    private void RaycastForHerb(PointerEventData eventData)
    {
        List<RaycastResult> results = new();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Farm Pouch"))
            {
                SeedFarmPouch pouch = result.gameObject.GetComponent<SeedFarmPouch>();
                pouch.HitWithSeed(holdingSeed);
            }
        }
    }

}
