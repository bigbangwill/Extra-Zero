using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleNodeToUnlock : CampaignNodeScript
{

    public override void SetUnlocked()
    {
        foreach (var node in nodeConnectedFrom)
        {
            if (!node.IsDone)
            {
                Debug.Log("Hit the if");
                return;
            }
        }
        IsUnlocked = true;
        campaignInfo.ResetDoneNodes();
    }

}