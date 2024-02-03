using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private OverlayState state;

    private IRepairable repairable;

    public delegate void RelatedDelegate();

    private RelatedDelegate currentDelegate;

    public void SetDelegate(RelatedDelegate action,OverlayState state)
    {
        currentDelegate = action;
        this.state = state;
    }

    public void RemoveDelegate()
    {
        currentDelegate = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        //Type random = searchType;

        Vector2 targetPos = Camera.main.ScreenToWorldPoint(eventData.position);

        RaycastHit2D[] hits = Physics2D.RaycastAll(targetPos, Vector2.zero);
        if (hits.Length > 0 )
        {

        }

    }
}