using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TalentManager : MonoBehaviour
{
    [SerializeField] private List<TalentTreeOrbitalMovement> talentTrees = new();

    private List<NodeMovement> orbits = new();

    private NodeMovement currentTargetedNode;

    private NodeMovement[] closeOrbits = new NodeMovement[3];

    private Dictionary<NodeMovement, float> currentDistance = new();


    private void Start()
    {
        GetAllOrbs();
    }

    private void GetAllOrbs()
    {
        foreach (var talent in talentTrees)
        {
            foreach(Transform orbit in talent.GetAllNodes())
            {
                orbits.Add(orbit.GetComponent<NodeMovement>());
            }
        }
    }


    public void SetTargetNode(NodeMovement targetNode)
    {
        currentDistance.Clear();
        foreach (var orbit in closeOrbits)
        {
            if(orbit != null)
                orbit.SetColor(Color.blue);
        }
        if (targetNode == currentTargetedNode)
        {
            currentTargetedNode.SetColor(Color.blue);
            currentTargetedNode = null;
            return;
        }

        closeOrbits = new NodeMovement[3];
        if(currentTargetedNode != null)
            currentTargetedNode.SetColor(Color.blue);
        currentTargetedNode = targetNode;
        foreach (var orbit in orbits)
        {
            if (orbit == targetNode)
            {

                continue;
            }
            Vector3 targetNodeChild = targetNode.transform.GetChild(0).position;
            Vector3 orbitNodeChild = orbit.transform.GetChild(0).position;
            float distance = Vector3.Distance(targetNodeChild, orbitNodeChild);
            Debug.Log(distance);
            currentDistance.Add(orbit, distance);
        }

        var sortedDistances = currentDistance.OrderBy(x => x.Value);
        closeOrbits = sortedDistances.Take(3).Select(pair => pair.Key).ToArray();
        currentTargetedNode.SetColor(Color.green);
        closeOrbits[0].SetColor(Color.red);
        closeOrbits[1].SetColor(Color.red);
        closeOrbits[2].SetColor(Color.red);

    }
    


    
}