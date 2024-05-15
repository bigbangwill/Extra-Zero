using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabButton : MonoBehaviour, IDragHandler, IEndDragHandler , IBeginDragHandler
{

    private enum GrabButtonSeed { Chamomile , Lavender , Sage , Patchouli , Hellebore }

    [SerializeField] private HerbalismPost post;
    [SerializeField] private GrabButtonSeed holdingSeedEnum;


    private Vector3 startPos;


    private bool isInit = false;


    private PlayerInventory playerInventory;




    [SerializeField] private TextMeshProUGUI stackText;
    private Seed holdingSeed;
    private int stackCount;
    private List<Seed> holdingSeedList = new();


    private void LoadSORefrence()
    {
        playerInventory = ((PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence")).val;
    }




    private void Start()
    {
        LoadSORefrence();
        isInit = true;
        switch (holdingSeedEnum)
        {
            case GrabButtonSeed.Chamomile: holdingSeed = new Seed.Chamomile();break;
            case GrabButtonSeed.Lavender: holdingSeed = new Seed.Lavender();break;
            case GrabButtonSeed.Sage: holdingSeed = new Seed.Sage();break;
            case GrabButtonSeed.Patchouli: holdingSeed = new Seed.Patchouli();break;
            case GrabButtonSeed.Hellebore: holdingSeed = new Seed.Hellebore();break;
        }
        holdingSeed.SetCurrentStack(1);
    }

    private void OnEnable()
    {
        if (!isInit)
            return;

    }

    public void SetStack(List<Seed> list)
    {
        holdingSeedList.Clear();
        holdingSeedList = list;
        stackCount = 0;
        foreach (Seed seed in holdingSeedList)
        {
            stackCount += seed.CurrentStack();
        }
        stackText.text = stackCount.ToString();
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        startPos = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the dragged object directly using Input.mousePosition
        transform.position = Input.mousePosition;

        // Check for hovering over a specific UI object
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        // Perform a raycast to check if the dragged object is hovering over the specific UI object
        List<RaycastResult> results = new(); // Adjust the array size as needed
        EventSystem.current.RaycastAll(pointerEventData, results);

        // Check if the UI object you're interested in is among the results
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Herbalism Spot"))
            {
                HerbalismSpot spot = result.gameObject.GetComponent<HerbalismSpot>();
                if (!spot.IsGrowing() && !spot.IsLocked)
                {
                    //if (post.CanGetNextSeed())
                    //{
                    //    Seed cloned = (Seed)post.GetNextSeed().Clone();
                    //    spot.PlaceNewSeed(cloned);
                    //}

                    if (stackCount > 0)
                    {
                        Seed Cloned = (Seed)holdingSeed.Clone();
                        spot.PlaceNewSeed(Cloned);
                        //holdingSeedList[0].SetCurrentStack(holdingSeedList[0].CurrentStack() - 1);
                        playerInventory.HaveItemInInventory(holdingSeed, true);
                        stackCount--;
                        stackText.text = stackCount.ToString();
                    }


                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPos;
    }
}