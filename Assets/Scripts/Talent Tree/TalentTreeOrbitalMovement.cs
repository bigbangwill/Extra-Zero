using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TalentTreeOrbitalMovement : MonoBehaviour
{
    public float speed;
    public List<Transform> nodeList = new List<Transform>();
    public bool shouldMove;
    public float timeOffset = 5f;

    private void Start()
    {
        float angleStep = 360f / nodeList.Count; // Angle between each object

        for (int i = 0; i < nodeList.Count; i++)
        {
            nodeList[i].Rotate(new Vector3(0 , angleStep * i, 0));
        }
        StartCoroutine(StartMovement());
    }

    public List<Transform> GetAllNodes()
    {
        return nodeList;
    }
 
    private IEnumerator StartMovement()
    {
        float timeSpent = 0;
        int counter = 0;
        int sending = 1;
        while (true)
        {
            if (timeSpent > timeOffset)
            {
                nodeList[counter].GetComponent<NodeMovement>().Move(sending);
                sending *= -1;
                counter++;
                timeSpent = 0;
            }
            timeSpent += Time.deltaTime;
            yield return null;
            if (counter > nodeList.Count - 1)
            {
                foreach (Transform t in nodeList)
                {
                    t.GetComponent<NodeMovement>().SlowDown();
                }
                break;
            }
        }
    }


}
