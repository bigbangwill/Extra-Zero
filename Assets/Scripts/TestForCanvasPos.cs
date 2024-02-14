using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForCanvasPos : MonoBehaviour
{
    public float zOffset;

    private void Update()
    {
        Vector3 pos = Camera.main.transform.position;
        pos.z = zOffset;
        transform.position = pos;
    }



}