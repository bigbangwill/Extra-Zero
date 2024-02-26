using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum OverlayState { RepairMode}

public class UseableItemCanvasScript : MonoBehaviour, IPointerDownHandler
{
    //#region Sinleton
    //public static UseableItemCanvasScript Instance
    //{
    //    get { return ((UseableItemCanvasScript)_Instance); }
    //    set { _Instance = value; }
    //}
    //#endregion

    [SerializeField] private List<OrderPostHealth> repairables = new();
    [SerializeField] private GameObject repairIconPrefab;
    [SerializeField] private Transform repairInstantiateParent;
    [SerializeField] private Transform repairInfoPanel;

    private IRepairable currentRepairable;

    private OverlayState currentState;
    public event Action UsedItemEvent;

    private Button currentRelatedButton;
    private bool isOnRepairMode = false;

    public bool IsOnRepairMode { get => isOnRepairMode; set => isOnRepairMode = value;}


    [SerializeField] private Color repairColor;
    private Image overlayImage;

    private PlayerInventoryRefrence inventoryRefrence;
    private UsableCanvasManagerRefrence refrence;


    private void SetRefrence()
    {
        refrence = (UsableCanvasManagerRefrence)FindSORefrence<UseableItemCanvasScript>.FindScriptableObject("Event Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        Debug.Log("We did find it");
        refrence.val = this;
    }


    private void LoadSORefrence()
    {
        inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        overlayImage = GetComponent<Image>();
        inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Ceramic(10), true);
        inventoryRefrence.val.HaveEmptySlot(new MaterialItem.AluminumAlloy(10), true);
        inventoryRefrence.val.HaveEmptySlot(new MaterialItem.TitaniumAlloy(10), true);
        inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Plastic(10), true);
    }

    public void SetDelegate(Action action ,OverlayState state,Transform UIPanel,Transform parent, Button repairButton)
    {
        currentState = state;
        UsedItemEvent += action;
        overlayImage.enabled = true;
        repairInstantiateParent = parent;
        repairInfoPanel = UIPanel;
        currentRelatedButton = repairButton;
        isOnRepairMode = true;
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
        isOnRepairMode = false;

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
        repairInfoPanel.gameObject.SetActive(true);
        foreach (var repair in repairables)
        {
            repair.TurnUIUXOn();
        }
    }

    private void SetHealingUIOff()
    {
        repairInfoPanel.gameObject.SetActive(false);
        foreach (var repair in repairables)
        {
            repair.TurnUIUXOff();
        }
    }
    

    public void CallRepair()
    {
        foreach (Transform transform in repairInstantiateParent)
        {
            Destroy(transform.gameObject);
        }

        List<ItemBehaviour> repairMaterials = currentRepairable.RepairMaterials().ToList();
        
        foreach (var material in repairMaterials)
        {
            GameObject repair = Instantiate(repairIconPrefab);
            repair.transform.SetParent(repairInstantiateParent);
            repair.GetComponent<Image>().sprite = material.IconRefrence();
            repair.GetComponentInChildren<TextMeshProUGUI>().text = material.CurrentStack().ToString();
        }
        currentRelatedButton.onClick.RemoveAllListeners();
        currentRelatedButton.onClick.AddListener(() => CheckAndRepair());
    }

    private void CheckAndRepair()
    {
        if (currentRepairable.NeedsRepair())
        {
            bool haveEveryMaterial = true;
            foreach (var item in currentRepairable.RepairMaterials())
            {
                if (!inventoryRefrence.val.HaveItemInInventory(item, false))
                {
                    haveEveryMaterial = false;
                    return;
                }
            }
            if (haveEveryMaterial)
            {
                currentRepairable.Repair();
                CallRepair();
            }
        }
    }




    private bool RepairRaycast(RaycastHit2D[] hits)
    {
        currentRepairable = null;
        foreach (RaycastHit2D hit in hits)
        {
            IRepairable repairable = hit.collider.GetComponent<IRepairable>();
            if (repairable != null)
            {
                if (currentRepairable != null)
                    currentRepairable.Targeted = false;
                currentRepairable = repairable;
                currentRepairable.Targeted = true;
                if (repairable.NeedsRepair())
                {
                    NavmeshReachableInformation navInfo = new(repairable.GetReachingTransfrom().position,
                        CallRepair);
                    PlayerMovement.Instance.MovetoTarget(navInfo);
                }
                return true;
            }
        }
        return false;
    }
}