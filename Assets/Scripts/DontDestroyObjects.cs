using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObjects : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
