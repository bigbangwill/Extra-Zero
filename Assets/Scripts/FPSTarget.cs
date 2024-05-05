using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSTarget : MonoBehaviour
{
    [SerializeField] private int targetFPS = 0;
    private void Awake()
    {
        Application.targetFrameRate = targetFPS;
    }
}
