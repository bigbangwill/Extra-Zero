using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbalismPost : MonoBehaviour
{

    public Queue<Seed> seedQueue = new();
    private List<Seed> inventorySeeds = new();
    private List<QueueIcon> queueIcons = new List<QueueIcon>();

    [SerializeField] private GameObject iconPrefab;
    [SerializeField] private RectTransform contentUIGO;
    [SerializeField] private float iconOffset;
    [SerializeField] private float startingPointOffset;



    public bool CanGetNextSeed()
    {
        if (inventorySeeds.Count == 0)
            return false;
        else
            return true;
    }

    public Seed GetNextSeed()
    {
        //Seed targetSeed = seedQueue.Peek();
        //if (targetSeed.CurrentStack() > 1)
        //{
        //    int currentStack = targetSeed.CurrentStack();
        //    targetSeed.SetCurrentStack(currentStack -1);
        //    queueIcons[0].SetText((currentStack - 1).ToString());
        //    InitializeUI();
        //    return targetSeed;
        //}
        //else
        //{
        //    Seed dequeue = seedQueue.Dequeue();
        //    queueIcons.RemoveAt(0);
        //    inventorySeeds.Remove(dequeue);
        //    InitializeUI();
        //    return dequeue;
        //}

        Seed targetSeed = inventorySeeds[0];
        if (targetSeed.CurrentStack() > 1)
        {
            int currentStack = targetSeed.CurrentStack() - 1;
            targetSeed.SetCurrentStack(currentStack);
            queueIcons[0].SetText(currentStack.ToString());
            Debug.Log(currentStack.ToString());
            return targetSeed;
        }
        else
        {
            inventorySeeds.RemoveAt(0);
            //queueIcons.RemoveAt(0);
            InitializeUI();
            return targetSeed;
        }



    }

    public void AddSeedToQueue(Seed seed)
    {
        seedQueue.Enqueue(seed);
    }

    public void SeedHarvested(Seed seed)
    {
        
    }

    public void DepostHerbsButton()
    {
        inventorySeeds = PlayerInventory.Instance.SearchInventoryOfItemBehaviour<Seed>(ItemType.seed);
        foreach (var item in inventorySeeds)
        {
            PlayerInventory.Instance.HaveItemInInventory(item, true);
        }
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
