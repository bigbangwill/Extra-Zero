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
    private float scaleShrinkerSpeed = 1;

    public void OnPointerClick(PointerEventData eventData)
    {
        StopCoroutine(StartTimer());
        basicMaterialScript.MiniGameClicked();
    }


    public void OnEnable()
    {
        StartCoroutine(StartTimer());
    }

    public void OnDisable()
    {
        StopCoroutine(StartTimer());
        BackToNormal();
    }

    private void BackToNormal()
    {
        transform.localScale = Vector2.one;
        currentTimer = 0;
    }

    private IEnumerator StartTimer()
    {
        while (true)
        {
            currentTimer += Time.deltaTime;
            Vector2 originalScale = transform.localScale;
            originalScale.x -= scaleShrinkerSpeed/maxTimer * Time.deltaTime;
            originalScale.y -= scaleShrinkerSpeed/maxTimer * Time.deltaTime;
            transform.localScale = originalScale;
            if (currentTimer >= maxTimer)
            {
                Debug.Log("Stoped");
                basicMaterialScript.DidntHitMiniGame();                
                break;
            }
            yield return null;
        }
    }
}
