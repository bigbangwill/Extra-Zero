using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class HerbalismSpot : MonoBehaviour , IPointerDownHandler
{
    [SerializeField] private float zeroHarvest;
    [SerializeField] private float soonHarvest;
    [SerializeField] private float perfectHarvest;
    [SerializeField] private float lateHarvest;

    [SerializeField] private Image image;
    [SerializeField] private Sprite defaultImage;
    [SerializeField] private Sprite lockedImage;
    [SerializeField] private Color colorStart;
    [SerializeField] private Color colorEnd;

    private float duration;

    private Color startingColor;


    private float currentTimer;
    private float maxTimer;

    private Seed currentGrowingSeed;

    private bool isGrowing = false;

    [SerializeField] private HerbalismPost post;

    [SerializeField] private bool isLocked = true;

    private float maxTimerHarvestUpgradeModifier = 0;

    public bool IsLocked { get { return isLocked; } }

    private void Start()
    {
        startingColor = image.color;
        SetState();
    }

    private void Update()
    {
        if (isGrowing)
            Grow();
    }


    private void SetState()
    {
        if (isLocked)
            SetLocked();
        else
            SetUnlocked();
    }

    public void SetLocked()
    {
        isLocked = true;
        image.sprite = lockedImage;
    }

    public void SetUnlocked()
    {
        isLocked = false;
        image.sprite = defaultImage;
    }


    /// <summary>
    /// Used to check on raycast if it's not growing any seed inside to call the PlaceNewSeed.
    /// </summary>
    /// <returns></returns>
    public bool IsGrowing()
    {
        return isGrowing;
    }

    /// <summary>
    /// gets called after IsGrowing method is false.
    /// </summary>
    /// <param name="seed"></param>
    public void PlaceNewSeed(Seed seed)
    {
        currentGrowingSeed = seed;
        maxTimer = seed.GetMaxHarvestTimer() + maxTimerHarvestUpgradeModifier;
        currentTimer = 0;
        isGrowing = true;
        duration = seed.GetMaxHarvestTimer();
    }

    // Gets called in the update method to procced the timer.
    private void Grow()
    {
        currentTimer += Time.deltaTime;
        ChangeColor(currentTimer);
        if (currentTimer >= maxTimer)
        {
            Decayed();
        }
    }

    // Change color to show the current state of the herb.
    private void ChangeColor(float timerValue)
    {
        float normalizedTime = Mathf.Clamp01(timerValue / duration);
        Color lerpedColor = Color.Lerp(colorStart, colorEnd, normalizedTime);
        image.color = lerpedColor;
    }


    // Gets called from the OnPointerDown method to start the harvest method.
    private void Harvest()
    {

        float timerValue = maxTimer / 10;
        float zeroValue = 1 * timerValue;
        float soonValue = 4 * timerValue + zeroValue;
        float perfectValue = 2 * timerValue + soonValue;
        float decayedValue = 3 * timerValue + perfectValue;


        if (currentTimer >= decayedValue)
        {
            int harvestValue = currentGrowingSeed.GetHarvestAmount() / 3;
            currentGrowingSeed.SetHarvestAmount(harvestValue);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Decayed");
        }
        else if (currentTimer >= perfectValue)
        {
            int harvestValue = currentGrowingSeed.GetHarvestAmount();
            currentGrowingSeed.SetHarvestAmount(harvestValue);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Perfect");
        }
        else if (currentTimer >= soonValue)
        {
            int harvestValue = currentGrowingSeed.GetHarvestAmount() / 2;
            currentGrowingSeed.SetHarvestAmount(harvestValue);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Soon");
        }
        else if (currentTimer >= zeroValue)
        {
            int harvestValue = currentGrowingSeed.GetHarvestAmount() / 3;
            currentGrowingSeed.SetHarvestAmount(harvestValue);
            post.SeedHarvested(currentGrowingSeed);
            Debug.Log("Zero");
        }
        ResetMethod();
    }

    // Called when the player didnt click the spot in time.
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

    /// <summary>
    /// To reset the slot back to the default value and make it ready for next seed.
    /// </summary>
    public void ResetMethod()
    {
        isGrowing = false;
        currentGrowingSeed = null;
        image.color = startingColor;
    }

    public void UpgradeOrbit(bool isQubit)
    {
        if (!isQubit)
        {
            maxTimerHarvestUpgradeModifier = 1;
        }
        else
        {
            maxTimerHarvestUpgradeModifier = 2;
        }
    }
}
