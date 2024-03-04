using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedFarmPouch : MonoBehaviour
{
    [SerializeField] SeedFarmManager seedFarmManager;

    public void HitWithSeed(Seed seed)
    {
        seedFarmManager.HitWithSeed(seed);
    }
}
