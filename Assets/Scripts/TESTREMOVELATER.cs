using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class TESTREMOVELATER : MonoBehaviour
{

    public List<GameObject> nodes = new();

#if UNITY_EDITOR
    [ContextMenu("Rename GameObjects")]
    void RenameGameObjects()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].name = i.ToString();
            nodes[i].GetComponent<CampaignNodeScript>().SetNodeName(nodes[i].name);
        }
    }
#endif
}