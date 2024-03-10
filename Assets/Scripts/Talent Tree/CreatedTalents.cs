using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatedTalents
{
    private static bool isCreated = false;
    private static List<TalentLibrary> talentsList;

    public static bool IsCreated { get { return isCreated; } }

    public static void CreateTalents(List<TalentLibrary> list)
    {
        isCreated = true;
        talentsList = list;
    }


    public static IEnumerable<TalentLibrary> GetTalents()
    {
        if (!IsCreated)
            Debug.LogError("Check here asap");
        return talentsList;
    }
    
    
}