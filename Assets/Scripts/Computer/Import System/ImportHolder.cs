using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class ImportHolder : MonoBehaviour
{
    private List<BluePrintItem> importedBluePrintList = new();

    [SerializeField] private GameObject blueprintUIPrefab;
    [SerializeField] private RectTransform contentUIGO;
    [SerializeField] private RectTransform scrollViewUIGO;

    [SerializeField] private int columnCount; // How many in a line?
    [SerializeField] private float startingPointSkip;
    [SerializeField] private float offsetRow;
    [SerializeField] private float offsetStartingRow;
    [SerializeField] private float offsetPrefabWidth;

    [SerializeField] private Button sendToPrinterButton;

    [SerializeField] private ItemPrinter itemPrinterRefrence;

    private BluePrintItem activeImport = null;
    private GameObject activeImportUIGO = null;
    private int activeNumber;

    private void Start()
    {
        importedBluePrintList.Add(new BluePrintItem.WalkingStick());
        importedBluePrintList.Add(new BluePrintItem.Gun());
        importedBluePrintList.Add(new BluePrintItem.Hoe());
        importedBluePrintList.Add(new BluePrintItem.Plant());
        InitializeUI();
    }

    private void OnEnable()
    {
        InitializeUI();
    }


    /// <summary>
    /// Method to be called when a new BluePrintItem has been imported to the system
    /// </summary>
    /// <param name="importedItem"></param>
    public void ImportNewItem(BluePrintItem importedItem)
    {
        if (!importedBluePrintList.Contains(importedItem))
        {
            importedBluePrintList.Add(importedItem);
            if (isActiveAndEnabled)
            {
                InitializeUI();
            }
        }
        else
        {
            Debug.LogWarning("Already Added");
        }
    }

    /// <summary>
    /// A bool method to check if the player wants to research a blueprint that is already added to the list.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool IfBluePrintAllreadyImported(BluePrintItem item)
    {
        if (importedBluePrintList.Contains(item))
        {
            Debug.Log("Already added");
            return true;
        }
        else
        {
            Debug.Log("Was not added Before");
            return false;
        }
    }

    /// <summary>
    /// To set the curret active ImportedUIPrefab 
    /// </summary>
    /// <param name="number"></param>
    /// <param name="currentActive"></param>
    public void SetActiveImport(int number, GameObject currentActive)
    {
        if (activeNumber == number)
        {
            if(activeImportUIGO != null)
                activeImportUIGO.SetActive(false);
            activeImportUIGO = null;
            activeImport = null;
            activeNumber = -1;
            SetSendButtonState(false);
        }
        else
        {
            activeImport = importedBluePrintList[number];
            if(activeImportUIGO != null)
                activeImportUIGO.SetActive(false);
            activeImportUIGO = currentActive;
            activeNumber = number;
            SetSendButtonState(true);
        }
    }

    /// <summary>
    /// Method to set the sent to printer button state to make it intractable or not
    /// </summary>
    /// <param name="shouldActive"></param>
    private void SetSendButtonState(bool shouldActive)
    {
        sendToPrinterButton.interactable = shouldActive;
    }

    /// <summary>
    /// Redraw the whole ui for import holder
    /// </summary>
    public void InitializeUI()
    {
        foreach (Transform child in contentUIGO.transform)
        {
            Destroy(child.gameObject);
        }

        float prefabHeight = blueprintUIPrefab.GetComponent<RectTransform>().rect.height;
        float prefabWidth = blueprintUIPrefab.GetComponent<RectTransform>().rect.width;

        float onePrefabHeight = prefabHeight + offsetRow;
        int rowCount = Mathf.CeilToInt((float)importedBluePrintList.Count / columnCount);
        float totalHeight = rowCount * onePrefabHeight;
        contentUIGO.sizeDelta = new Vector2(0, totalHeight);
        float distance = scrollViewUIGO.rect.width / columnCount;

        float startingPointWidth = -1 * (scrollViewUIGO.rect.width - startingPointSkip) / 2;
        float startingPointHeight = (contentUIGO.rect.height / 2) - ((prefabHeight / 2) + offsetStartingRow);

        int currentInstantiated = 0;

        for(int j = 0 ; j < rowCount; j++)
        {
            for (int i = 0 ; i < columnCount; i++)
            {
                if (currentInstantiated >= importedBluePrintList.Count)
                {
                    break;
                }
                currentInstantiated++;
                GameObject go = Instantiate(blueprintUIPrefab, contentUIGO);
                RectTransform goRT = go.GetComponent<RectTransform>();
                goRT.anchoredPosition = new Vector2(
                    startingPointWidth + distance * i, startingPointHeight - (onePrefabHeight * j));
                ImportedUI importedUI = go.GetComponent<ImportedUI>();
                BluePrintItem targetBluePrint = importedBluePrintList[currentInstantiated - 1];
                List<ItemBehaviour> materialNeeded = targetBluePrint.materialsList;
                importedUI.SetActiveMethod(new Action<int,GameObject>(SetActiveImport));
                importedUI.SetNumber(currentInstantiated - 1);


                importedUI.SetItemImage(targetBluePrint.IconRefrence());
                importedUI.SetItemNameText(targetBluePrint.GetName());

                if (materialNeeded.Count == 1)
                {
                    importedUI.SetMaterialIcon1(materialNeeded[0].IconRefrence());
                    importedUI.SetCountMaterialText1(materialNeeded[0].CurrentStack().ToString());
                    importedUI.Initialize(1);
                }
                else if (materialNeeded.Count == 2)
                {
                    importedUI.SetMaterialIcon1(materialNeeded[0].IconRefrence());
                    importedUI.SetCountMaterialText1(materialNeeded[0].CurrentStack().ToString());
                    importedUI.SetMaterialIcon2(materialNeeded[1].IconRefrence());
                    importedUI.SetCountMaterialText2(materialNeeded[1].CurrentStack().ToString());
                    importedUI.Initialize(2);
                }
                else if (materialNeeded.Count == 3)
                {
                    importedUI.SetMaterialIcon1(materialNeeded[0].IconRefrence());
                    importedUI.SetCountMaterialText1(materialNeeded[0].CurrentStack().ToString());
                    importedUI.SetMaterialIcon2(materialNeeded[1].IconRefrence());
                    importedUI.SetCountMaterialText2(materialNeeded[1].CurrentStack().ToString());
                    importedUI.SetMaterialIcon3(materialNeeded[2].IconRefrence());
                    importedUI.SetCountMaterialText3(materialNeeded[2].CurrentStack().ToString());
                    importedUI.Initialize(3);
                }
                else
                    Debug.LogError("Might wanna check here for sure. NOW");
            }
        }
    }

    public void SentButtonClicked()
    {
        itemPrinterRefrence.SentToPrinter(activeImport);
    }



}
