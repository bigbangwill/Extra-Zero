using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabButton : MonoBehaviour, IDragHandler, IEndDragHandler , IBeginDragHandler
{

    [SerializeField] private HerbalismPost post;

    private Vector3 startPos;

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
                if (!spot.IsGrowing())
                {
                    if(post.CanGetNextSeed())
                        spot.PlaceNewSeed(post.GetNextSeed());
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = startPos;
    }
}