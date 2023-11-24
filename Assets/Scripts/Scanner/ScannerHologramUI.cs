using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ScannerHologramUI : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler
{
    public Image image;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI timerText;

    public ScrollViewUI scrollViewUI;
    // For keeping the original item refrence to add or remove later.
    public BluePrintItem cacheItem;
    
    [SerializeField] private GameObject activeGO;


    private void Update()
    {
        if (image.sprite == null)
        {
            image.sprite = cacheItem.IconRefrence();
        }
    }


    /// <summary>
    /// Method to get called to set it's active Gameobject on.
    /// </summary>
    private void IsActive()
    {
        activeGO.SetActive(true);
        scrollViewUI.SetActiveHologram(this);
    }

    /// <summary>
    /// Gets called from ScrollViewUI to set it back to deactive.
    /// </summary>
    public void IsDeactive()
    {
        activeGO.SetActive(false);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (scrollViewUI.isDragging)
            return;
        IsActive();

    }
}