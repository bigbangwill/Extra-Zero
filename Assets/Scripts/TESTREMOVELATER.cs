using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class TESTREMOVELATER : MonoBehaviour
{

    public void OnDestroy()
    {
        Debug.Log("Do it?");
    }
}