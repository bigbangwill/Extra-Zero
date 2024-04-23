using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleNodeToUnlock : CampaignNodeScript
{

    public override void SetUnlocked()
    {
        foreach (var node in nodeConnectedFrom)
        {
            Debug.Log("Here");
            if (!node.IsDone)
            {
                Debug.Log("Hit the if");
                return;
            }
        }
        Debug.Log("Before calling base");
        IsUnlocked = true;
        campaignInfo.ResetDoneNodes();
    }

}