using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLineRenderer : MonoBehaviour
{
    
    private LineRenderer lineRenderer;
    private Transform firstNode;
    private Transform secondNode;

    private bool isReady;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(isReady)
        {
            lineRenderer.SetPosition(0, firstNode.position);
            lineRenderer.SetPosition(1, secondNode.position);
        }
    }


    public void Setup(Transform start,Transform end)
    {
        firstNode = start;
        secondNode = end;
        isReady = true;
    }
}