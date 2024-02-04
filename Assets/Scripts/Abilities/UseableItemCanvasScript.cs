using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum OverlayState { RepairMode}

public class UseableItemCanvasScript : SingletonComponent<UseableItemCanvasScript>, IPointerDownHandler
{
    #region Sinleton
    public static UseableItemCanvasScript Instance
    {
        get { return ((UseableItemCanvasScript)_Instance); }
        set { _Instance = value; }
    }
    #endregion

    [SerializeField] private List<OrderPostHealth> repairables = new();
 


    private OverlayState currentState;
    public event Action UsedItemEvent;


    [SerializeField] private Color repairColor;
    private Image overlayImage;

    private void Start()
    {
        overlayImage = GetComponent<Image>();
    }

    public void SetDelegate(Action action ,OverlayState state)
    {
        currentState = state;
        UsedItemEvent += action;
        overlayImage.enabled = true;
        switch (state) 
        {
            case OverlayState.RepairMode: overlayImage.color = repairColor; SetHealingUIOn(); break;
            default: break;        
        }
    }

    public void RemoveDelegate(Action action)
    {
        overlayImage.enabled = false;
        UsedItemEvent -= action;

        switch (currentState)
        {
            case OverlayState.RepairMode: SetHealingUIOff(); break;
            default: break;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(eventData.position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(targetPos, Vector2.zero);
        if (hits.Length > 0)
        {
            switch (currentState)
            {
                case OverlayState.RepairMode: if (RepairRaycast(hits))return; break;
                default: Debug.Log("Couldnt find the needed item"); break;
            }
        }
    }

    private void SetHealingUIOn()
    {
        foreach (var repair in repairables)
        {
            repair.TurnUIUXOn();
        }
    }

    private void SetHealingUIOff()
    {
        foreach (var repair in repairables)
        {
            repair.TurnUIUXOff();
        }
    }







    private bool RepairRaycast(RaycastHit2D[] hits)
    {
        foreach (RaycastHit2D hit in hits)
        {
            IRepairable repairable = hit.collider.GetComponent<IRepairable>();
            if (repairable != null)
            {
                if (repairable.NeedsRepair())
                {
                    if (repairable.Repair())
                    {
                        UsedItemEvent.Invoke();
                    }
                }
                return true;
            }
        }
        return false;
    }
}