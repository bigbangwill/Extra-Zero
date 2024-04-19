using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForCanvasPos : MonoBehaviour
{
    public NavMeshSurface surface;

    private void Start()
    {
        surface.BuildNavMesh();
    }
}