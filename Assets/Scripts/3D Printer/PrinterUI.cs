using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrinterUI : MonoBehaviour
{
    [SerializeField] private PrinterManager printerManager;

    private void OnEnable()
    {
        printerManager.isActive = true;
        printerManager.RefreshUI();
    }

    private void OnDisable()
    {
        printerManager.isActive = false;
    }
}
