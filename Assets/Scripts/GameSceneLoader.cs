using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneLoader : MonoBehaviour
{
    [SerializeField] private Camera talentCamera;
    private void Start()
    {
        GameObject talentCanvas = GameObject.FindGameObjectWithTag("Talent Canvas");
        talentCanvas.GetComponent<Canvas>().worldCamera = talentCamera;
    }


}