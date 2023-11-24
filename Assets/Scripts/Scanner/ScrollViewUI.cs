using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollViewUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{


    [SerializeField] private GameObject hologramUIPrefab;
    [SerializeField] private RectTransform scrollViewUIGO;
    [SerializeField] private RectTransform contentUIGO;


    [SerializeField] private int widthOffset;

    private List<BluePrintItem> iBList;
    [HideInInspector] public bool isDragging = false;


    private ScannerHologramUI currentActiveHologramUI;

    //To block the very first OnEnable method that calls before the Start method.
    bool started = false;

    private void Start()
    {
        started = true;
        OnEnable();
        
    }


    private void OnEnable()
    {
        if (started)
        {
            InitializeUI();
            EventManager.Instance.RefreshUIAddListener(InitializeUI);
            ScannerSlotManager.Instance.refreshUI = InitializeUI;
        }
    }
    private void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.RefreshUIRemoveListener(InitializeUI);
        }
    }

    /// <summary>
    /// Will get called to redraw the UI for scrollview
    /// </summary>
    public void InitializeUI()
    {
        iBList = new();
        iBList = PlayerInventory.Instance.SearchInventoryOfItemBehaviour<BluePrintItem>(ItemType.bluePrint);
        foreach (Transform child in contentUIGO.transform)
        {
            Destroy(child.gameObject);
        }
        float prefabHeight;
        prefabHeight = hologramUIPrefab.GetComponent<RectTransform>().rect.height;
        contentUIGO.sizeDelta = new Vector2(0,iBList.Count * prefabHeight);
        float instantiateStartPoint = (contentUIGO.rect.height / 2) - prefabHeight /2;
        for (int i = 0; i < iBList.Count; i++)
        {
            GameObject go = Instantiate(hologramUIPrefab,contentUIGO);
            RectTransform goRT = go.GetComponent<RectTransform>();
            goRT.anchoredPosition = new Vector2(0, instantiateStartPoint - (prefabHeight * i));
            ScannerHologramUI hologramUIGO = go.GetComponent<ScannerHologramUI>();
            hologramUIGO.image.sprite = iBList[i].IconRefrence();
            hologramUIGO.nameText.text = iBList[i].GetName() + " Hologram";
            hologramUIGO.timerText.text = iBList[i].ImportTimer().ToString() + "Seconds to import";
            hologramUIGO.scrollViewUI = this;
            hologramUIGO.cacheItem = iBList[i];
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }


    public void SetActiveHologram(ScannerHologramUI hologramUI)
    {
        if (currentActiveHologramUI != null && currentActiveHologramUI != hologramUI)
        {
            currentActiveHologramUI.IsDeactive();
        }
        if (currentActiveHologramUI == hologramUI)
        {
            currentActiveHologramUI.IsDeactive();
            currentActiveHologramUI = null;
            ScannerSlotManager.Instance.currentHologram = null;
            return;
        }
        currentActiveHologramUI = hologramUI;
        ScannerSlotManager.Instance.currentHologram = currentActiveHologramUI;
    }

}