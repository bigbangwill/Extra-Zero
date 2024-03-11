using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HerbalismPost : MonoBehaviour
{

    private List<Seed> inventorySeeds = new();
    private List<QueueIcon> queueIcons = new List<QueueIcon>();

    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private RectTransform contentUIGO;
    [SerializeField] private float iconOffset;
    [SerializeField] private float startingPointOffset;
    [SerializeField] private ItemStash stash;
    [SerializeField] private GameObject herbalismStashGO;


    [SerializeField] private List<HerbalismSpot> herbalismSpots = new();


    private bool isStarted = false;

    private HerbalismPostRefrence refrence;
    private PlayerInventoryRefrence inventoryRefrence;

    private void SetRefrence()
    {
        refrence = (HerbalismPostRefrence)FindSORefrence<HerbalismPost>.FindScriptableObject("Herbalism Post Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
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

    private void OnEnable()
    {
        if (!isStarted)
        {
            StartCoroutine(StashActive());
        }
    }
    private void Start()
    {
        LoadSORefrence();
        isStarted = true;
        inventoryRefrence.val.HaveEmptySlot(new Seed.Lavender(20), true);
        inventoryRefrence.val.HaveEmptySlot(new Seed.Sage(20), true);
        inventoryRefrence.val.HaveEmptySlot(new Seed.Chamomile(20), true);
    }


    public void UpgradeOrbitUnlockSlots(bool isQubit)
    {
        int counter = 0;
        foreach (var spot in herbalismSpots)
        {
            if (spot.IsLocked)
            {
                counter++;
                spot.SetUnlocked();
                if (counter > 3)
                    break;
            }
        }
        if (isQubit)
        {
            foreach (var spot in herbalismSpots)
            {
                if (spot.IsLocked)
                {
                    counter++;
                    spot.SetUnlocked();
                }
            }
        }
    }
    

    public void UpgradeOrbitMaxGrowTimer(bool isQubit)
    {
        foreach(var herbalismSpot in herbalismSpots)
        {
            herbalismSpot.UpgradeOrbit(isQubit);
        }
    }

   
    // To handle the first enable to set the needed refrences.
    private IEnumerator StashActive()
    {
        herbalismStashGO.gameObject.SetActive(true);
        yield return null;
        herbalismStashGO.gameObject.SetActive(false);
    }


    /// <summary>
    /// A checker to let the grab button if it can put the next seed in the spot.
    /// </summary>
    /// <returns></returns>
    public bool CanGetNextSeed()
    {
        if (inventorySeeds.Count == 0)
            return false;
        else
            return true;
    }

    /// <summary>
    /// if the CanGetNextSeed method returns true this will return the needed Seed.
    /// </summary>
    /// <returns></returns>
    public Seed GetNextSeed()
    {
        Seed targetSeed = inventorySeeds[0];
        if (targetSeed.CurrentStack() > 1)
        {
            int currentStack = targetSeed.CurrentStack() - 1;
            targetSeed.SetCurrentStack(currentStack);
            queueIcons[0].SetText(currentStack.ToString());
            return targetSeed;
        }
        else
        {
            inventorySeeds.RemoveAt(0);
            InitializeUI();
            return targetSeed;
        }



    }

    /// <summary>
    /// Will get called from herbalism spot to finilize the harvesting method and add it to the stash.
    /// </summary>
    /// <param name="seed"></param>
    public void SeedHarvested(Seed seed)
    {
        Herb harvested = seed.Harvest();
        if (harvested.CurrentStack() == 0)
        {
            Debug.Log("Was a zero harvest");
        }
        else if (stash.HaveEmptySlot(harvested, false))
        {
            stash.HaveEmptySlot(harvested, true);
            Debug.Log("Added to inventory" + harvested.CurrentStack());
        }
        else
        {
            Debug.Log("Dont have empty Slot");
        }
    }

    /// <summary>
    /// This method gets called from the button to deposit every Seed in the player inventory.
    /// </summary>
    public void DepostHerbsButton()
    {
        List<Seed> newSeeds = inventoryRefrence.val.SearchInventoryOfItemBehaviour<Seed>(ItemType.seed);
        foreach (var seed in newSeeds)
        {
            inventorySeeds.Add(seed);
        }
        foreach (var item in newSeeds)
        {
            inventoryRefrence.val.HaveItemInInventory(item, true);
        }
        InitializeUI();
    }

    /// <summary>
    /// To send the first seed to the last place in the list.
    /// </summary>
    public void TopToBottomButton()
    {
        if (inventorySeeds.Count <= 0)
            return;
        Seed firstSeed = inventorySeeds[0];
        inventorySeeds.RemoveAt(0);
        inventorySeeds.Add(firstSeed);
        InitializeUI();
    }

    
    

    public void InitializeUI()
    {
        foreach (Transform child in contentUIGO.transform)
        {
            Destroy(child.gameObject);
        }

        float prefabHeight = iconPrefab.GetComponent<RectTransform>().rect.height;
        float oneIconHeight = prefabHeight + iconOffset;

        float totalHeight = oneIconHeight * inventorySeeds.Count;
        contentUIGO.sizeDelta = new Vector2(0, totalHeight);
        float startingPoint = (totalHeight / 2) - startingPointOffset;
        queueIcons.Clear();

        for(int i = 0; i < inventorySeeds.Count; i++)
        {
            GameObject go = Instantiate(iconPrefab,contentUIGO);
            RectTransform goRT = go.GetComponent<RectTransform>();
            QueueIcon icon = go.GetComponent<QueueIcon>();
            goRT.anchoredPosition = new Vector2(0, startingPoint - (oneIconHeight * i));
            Debug.Log(inventorySeeds[i].CurrentStack());
            icon.SetText(inventorySeeds[i].CurrentStack().ToString());
            icon.SetImage(inventorySeeds[i].IconRefrence());
            queueIcons.Add(icon);
        }
    }


}
