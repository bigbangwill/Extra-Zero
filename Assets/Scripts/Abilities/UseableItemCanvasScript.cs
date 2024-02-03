using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseableItemCanvasScript : SingletonComponent<UseableItemCanvasScript>
{
    #region Sinleton
    public static UseableItemCanvasScript Instance
    {
        get { return ((UseableItemCanvasScript)_Instance); }
        set { _Instance = value; }
    }
    #endregion

}