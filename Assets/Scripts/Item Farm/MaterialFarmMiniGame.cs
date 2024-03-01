using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialFarmMiniGame : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] BasicMaterialScript basicMaterialScript;
    private float maxTimer = 5;
    private float currentTimer = 0;

    public void OnPointerClick(PointerEventData eventData)
    {
        basicMaterialScript.MiniGameClicked();
    }

    public void OnEnable()
    {
        

    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= maxTimer)
            {
                Debug.Log("Stoped");
                break;
            }
            yield return null;
        }
    }
}
