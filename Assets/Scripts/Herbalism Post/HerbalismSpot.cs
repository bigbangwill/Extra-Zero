using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HerbalismSpot : MonoBehaviour , IPointerDownHandler
{

    [SerializeField] private float zeroHarvest;
    [SerializeField] private float soonHarvest;
    [SerializeField] private float perfectHarvest;
    [SerializeField] private float lateHarvest;

    [SerializeField] private Image image;
    [SerializeField] private Color colorStart;
    [SerializeField] private Color colorEnd;

    private float duration;

    private Color startingColor;


    private float currentTimer;
    private float maxTimer;

    private Seed currentGrowingSeed;

    private bool isGrowing = false;

    [SerializeField] private HerbalismPost post;

    private void Start()
    {
        startingColor = image.color;
    }

    private void Update()
    {
        if (isGrowing)
            Grow();
    }

    public bool IsGrowing()
    {
        return isGrowing;
    }

    public void PlaceNewSeed(Seed seed)
    {
        currentGrowingSeed = seed;
        maxTimer = seed.GetMaxHarvestTimer();
        currentTimer = 0;
        isGrowing = true;
        duration = seed.GetMaxHarvestTimer();
    }

    private void Grow()
    {
        currentTimer += Time.deltaTime;
        ChangeColor(currentTimer);
        if (currentTimer >= maxTimer)
        {
            Decayed();
        }
    }

    private void ChangeColor(float timerValue)
    {
        float normalizedTime = Mathf.Clamp01(timerValue / duration);
        Color lerpedColor = Color.Lerp(colorStart, colorEnd, normalizedTime);
        image.color = lerpedColor;
    }

    private void Harvest()
    {
        float harvestTimer = maxTimer / currentTimer;

        if (harvestTimer > zeroHarvest)
        {
            currentGrowingSeed.SetHarvestAmount(0);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Zero");
        }
        else if (harvestTimer > soonHarvest && harvestTimer <= zeroHarvest)
        {
            currentGrowingSeed.SetHarvestAmount(1);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Soon");
        }
        else if (harvestTimer > perfectHarvest && harvestTimer <= soonHarvest)
        {
            currentGrowingSeed.SetHarvestAmount(3);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Perfect");
        }
        else if (harvestTimer > lateHarvest && harvestTimer <= perfectHarvest)
        {
            currentGrowingSeed.SetHarvestAmount(1);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Late");
        }
        ResetMethod();
    }

    private void Decayed()
    {
        isGrowing = false;
        currentGrowingSeed = null;
        ResetMethod();
        Debug.Log("Decayed");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isGrowing)
        {
            Harvest();
        }
    }

    public void ResetMethod()
    {
        isGrowing = false;
        currentGrowingSeed = null;
        image.color = startingColor;
    }
}
