using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbalismPost : MonoBehaviour
{

    public Queue<Seed> seedQueue = new();

    private bool isOnHold = false;
    private int holdStack;
    private Seed holdSeed;


    private void Start()
    {
        Seed targetSeed1 = new Seed.Sage(3);
        Seed targetSeed2 = new Seed.Lavender(3);
        Seed targetSeed3 = new Seed.Chamomile(3);
        seedQueue.Enqueue(targetSeed1);
        seedQueue.Enqueue(targetSeed2);
        seedQueue.Enqueue(targetSeed3);
    }

    public bool CanGetNextSeed()
    {
        if (seedQueue.Count == 0)
            return false;
        else
            return true;
    }

    //public Seed GetNextSeed()
    //{
    //    Seed targetSeed = seedQueue.Peek();
    //    if (targetSeed.CurrentStack() > 1)
    //    {
    //        isOnHold = true;
    //        holdStack = targetSeed.CurrentStack();
    //        holdSeed = 


    //    }
    //}

    //private void SetSeedOnHold(Seed seed)
    //{

    //}


    public void AddSeedToQueue(Seed seed)
    {
        seedQueue.Enqueue(seed);
    }

    public void SeedHarvested(Seed seed)
    {
        
    }

    public void DepostHerbsButton()
    {
        List<Seed> inventorySeeds = PlayerInventory.Instance.SearchInventoryOfItemBehaviour<Seed>(ItemType.seed);
        foreach (var item in inventorySeeds)
        {
            seedQueue.Enqueue((Seed)item);
        }
    }


}
