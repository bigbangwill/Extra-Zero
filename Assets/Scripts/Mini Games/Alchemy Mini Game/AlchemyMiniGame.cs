using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class AlchemyMiniGame : MonoBehaviour
{
    #region Variables
    // To import all of the prefabs from editor.
    [SerializeField] private List<GameObject> floatingObjectPrefabList = new();
    [SerializeField] private Transform cauldronCircleTransfrom;
    [SerializeField] private GameObject minigameCanvas;
    // the amount of the real radius we need to ignore
    // so we dont spawn objects on the Exact max range of cauldron
    [SerializeField] private float circleRadiusOffset;
    [SerializeField] private Vector2 sideEffectMeterRange;
    [SerializeField] private Vector2 sideEffectStartSideRange;
    // To calculate their current value to show on UI.
    private int sideEffectCurrentValue;
    // To calculate their current value to show on UI.
    private int critChanceCurrentValue;
    // Max count of floating objects in one round.
    [SerializeField] private int floatingObjectMaxCount;
    [SerializeField] private int critMaxChance;
    [SerializeField] private Vector2 minimumRarityChanceRange;
    [SerializeField] private Vector2 mediumRarityChanceRange;
    [SerializeField] private Vector2 maximumRarityChanceRange;
    // Parent of instatiated items.
    [SerializeField] private Transform floatObjectsParent;
    [SerializeField] private GameObject alchemyCanvas;


    [Header("Floating Stats")]
    [SerializeField] private int moreNecrotic;
    [SerializeField] private int moreCrit;
    [SerializeField] private int moreRegenerative;


    [Header("Mini Game Init stat")]
    [SerializeField] private int minigameRounds;
    [SerializeField] private float maxLineLenght;
    private int currentMinigameRound;

    [Header("UI Refrence")]
    [SerializeField] private Slider sideEffectSlider;
    [SerializeField] private Slider critChanceSlider;

    private AlchemyLineMiniGame alchemyLineScript;

    private List<GameObject> activeFloatingObjectsList = new();

    private PotionItem pregamePotion;
    private List<PotionEffect> regenerativePotionEffectList = new();
    private List<PotionEffect> necroticPotionEffectList = new();
    private List<PotionEffect> neutralPotionEffectList = new();
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private AlchemyRewardPanel rewardPanelScript;
    [SerializeField] private AlchemyPost post;
    #endregion


    private RaycastMovement raycastMovement;

    private int critPassiveUpgrade;

    private void LoadSoRefrence()
    {
        raycastMovement = ((RaycastMovementRefrence)FindSORefrence<RaycastMovement>.FindScriptableObject("Raycast Movement Refrence")).val;
    }

    private void Awake()
    {
        alchemyLineScript = GetComponent<AlchemyLineMiniGame>();
        InitUI();
        // To add of the existing potion effect in the list for reward system.
        List<Type> potionEffectTypes = Assembly.GetAssembly(typeof(PotionEffect))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(typeof(PotionEffect))).ToList();
        foreach (var type in potionEffectTypes)
        {
            PotionEffect potionEffect = (PotionEffect)Activator.CreateInstance(type);
            PotionEffect.SideEffect sideEffect = potionEffect.GetSideEffect();
            if (sideEffect == PotionEffect.SideEffect.Regenerative)
                regenerativePotionEffectList.Add(potionEffect);
            else if (sideEffect == PotionEffect.SideEffect.Necrotic)
                necroticPotionEffectList.Add(potionEffect);
            else
                neutralPotionEffectList.Add(potionEffect);
        }
    }

    private void Start()
    {
        LoadSoRefrence();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            Vector2 pos = Camera.main.transform.position;
            transform.position = pos;
        }
    }


    public void SetCritPassiveAmount(int amount)
    {
        critPassiveUpgrade = amount;
    }


    private void OnEnable()
    {
        raycastMovement = ((RaycastMovementRefrence)FindSORefrence<RaycastMovement>.FindScriptableObject("Raycast Movement Refrence")).val;
        raycastMovement.ShouldMove(false);
    }

    private void OnDisable()
    {
        raycastMovement = ((RaycastMovementRefrence)FindSORefrence<RaycastMovement>.FindScriptableObject("Raycast Movement Refrence")).val;
        raycastMovement.ShouldMove(true);
    }

    private void ActiveRewardPanel()
    {
        rewardPanel.SetActive(true);
        int crit = UnityEngine.Random.Range(0, critMaxChance + 1) - critPassiveUpgrade;
        List<PotionEffect> targetEffects = new();
        bool isCrit;
        if (crit <= critChanceCurrentValue)
            isCrit = true;
        else
            isCrit = false;


        if (sideEffectCurrentValue >= sideEffectStartSideRange.y)
            targetEffects = regenerativePotionEffectList;
        else if (sideEffectCurrentValue <= sideEffectStartSideRange.x)
            targetEffects = necroticPotionEffectList;
        else
            targetEffects = null;

            rewardPanelScript.EnableReward(targetEffects, isCrit, pregamePotion);
    }

    private void InitUI()
    {
        sideEffectSlider.minValue = sideEffectMeterRange.x;
        sideEffectSlider.maxValue = sideEffectMeterRange.y;
        sideEffectSlider.value = 0;

        critChanceSlider.minValue = 0;
        critChanceSlider.maxValue = critMaxChance;
        critChanceSlider.value = 0;
    }

    private void SetSliderValues()
    {
        sideEffectSlider.value = sideEffectCurrentValue;
        critChanceSlider.value = critChanceCurrentValue;
    }

    /// <summary>
    /// This method gets called after the create potion on the alchemy canvas gets clicked and it will start 
    /// to init every element for the mini game.
    /// </summary>
    public void CreatePotionButtonClicked(PotionItem potion)
    {
        gameObject.SetActive(true);
        post.SetMiniGameStatus(true);
        pregamePotion = potion;
        alchemyLineScript.enabled = true;
        critChanceCurrentValue = 0;
        sideEffectCurrentValue = 0;
        cauldronCircleTransfrom.gameObject.SetActive(true);
        minigameCanvas.SetActive(true);
        currentMinigameRound = 0;
        InstantiateFloatingObject(floatingObjectMaxCount);
        SetMeters();
        alchemyLineScript.CanDraw(true, maxLineLenght);
    }

    /// <summary>
    /// Will get called from the line script to tell that one round has passed.
    /// </summary>
    /// <param name="removedObjects"></param>
    public void DrawRoundFinished(List<GameObject> removedObjects)
    {
        currentMinigameRound++;
        int removedCount = removedObjects.Count;
        foreach (GameObject obj in removedObjects)
        {
            if(activeFloatingObjectsList.Contains(obj))
                activeFloatingObjectsList.Remove(obj);
            Destroy(obj);
        }
        InstantiateFloatingObject(removedCount);
        SetMeters();
        if (currentMinigameRound >= minigameRounds)
        {
            alchemyLineScript.CanDraw(false, maxLineLenght);
            ActiveRewardPanel();
            Debug.Log("Drawing is over");
        }
    }

    // This method will instantiate the needed floating objects and will take the count in to summon that 
    // amount.
    private void InstantiateFloatingObject(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 targetPos = GetRandomPointInCircle();
            int random = UnityEngine.Random.Range(0,floatingObjectPrefabList.Count);
            while (Physics2D.OverlapCircle(targetPos, floatingObjectPrefabList[1].transform.localScale.x/2, LayerMask.GetMask("Floating Objects")) != null)
            {
                // If there's a collision, generate a new position and check again
                targetPos = GetRandomPointInCircle();
            }
            // Once a valid position is found, spawn the object
            GameObject go = Instantiate(floatingObjectPrefabList[random], targetPos, Quaternion.identity);
            go.transform.SetParent(floatObjectsParent);
            activeFloatingObjectsList.Add(go);

            // To set the quality of them.
            int randomQuality = (int)UnityEngine.Random.Range(minimumRarityChanceRange.x, maximumRarityChanceRange.y+1);
            AlchemyFloatingObject floatingObject = go.GetComponent<AlchemyFloatingObject>();
            if (randomQuality >= minimumRarityChanceRange.x && randomQuality <= minimumRarityChanceRange.y)
                floatingObject.SetState(0);
            else if (randomQuality >= mediumRarityChanceRange.x && randomQuality <= mediumRarityChanceRange.y)
                floatingObject.SetState((FloatingObjectEnumQuality)1);
            else if (randomQuality >= maximumRarityChanceRange.x)
                floatingObject.SetState((FloatingObjectEnumQuality)2);
            

        }
    }

    // Will find a random point to summon the floating objects within the cauldron circle.
    private Vector2 GetRandomPointInCircle()
    {
        return cauldronCircleTransfrom.position +
                UnityEngine.Random.insideUnitSphere * (cauldronCircleTransfrom.localScale.x - circleRadiusOffset) / 2;
    }

    // this method will get called when ever there is a need to calculate the current
    // combo of the floating objects in the cauldron.
    private void SetMeters()
    {
        critChanceCurrentValue = 0;
        sideEffectCurrentValue = 0;

        foreach (var obj in activeFloatingObjectsList)
        {
            AlchemyFloatingObject alchemyFloatingObject = obj.GetComponent<AlchemyFloatingObject>();
            FloatingObjectEnumQuality quality = alchemyFloatingObject.GetQuality();
            FloatingObjectEnumState state = alchemyFloatingObject.GetState();
            switch (state)
            {
                case FloatingObjectEnumState.MoreRegenerative:
                    MoreRegenerative(quality);
                    break;
                case FloatingObjectEnumState.MoreNecrotic:
                    MoreNecrotic(quality);
                    break;
                case FloatingObjectEnumState.MoreCritChance:
                    MoreCritChance(quality);
                    break;
                case FloatingObjectEnumState.LessNecroticMoreCritChance:
                    LessNecroticMoreCritChance(quality);
                    break;
                case FloatingObjectEnumState.LessRegenerativeMoreCritChance:
                    LessRegenerativeMoreCritChance(quality);
                    break;
                case FloatingObjectEnumState.LessCritMoreNecrotic:
                    LessCritMoreNecrotic(quality);
                    break;
                case FloatingObjectEnumState.LessCritMoreRegenerative:
                    LessCritMoreRegenerative(quality);
                    break;
                default:
                    Debug.LogWarning("Have to check Here");
                    break;
            }
        }
        SetSliderValues();
    }


    // Will find the needed modifer int for SetMeters method.
    private int GetQualityModifer(FloatingObjectEnumQuality quality)
    {
        int modifer;
        switch (quality)
        {
            case 0: modifer = 1; break;
            case (FloatingObjectEnumQuality)1: modifer = 2; break;
            case (FloatingObjectEnumQuality)2: modifer = 3; break;
                default: modifer = 1; Debug.LogWarning("Need to check here"); break;
        }
        return modifer;
    }

    #region Floating Objects Actions
    private void MoreNecrotic(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        sideEffectCurrentValue -= moreNecrotic * modifer;
        CheckValuesAreWithinRange();
    }

    private void MoreRegenerative(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        sideEffectCurrentValue += moreRegenerative * modifer;
        CheckValuesAreWithinRange();
    }

    private void MoreCritChance(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        critChanceCurrentValue += moreCrit * modifer;
        CheckValuesAreWithinRange();
    }

    private void LessNecroticMoreCritChance(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        int lessNecrotic = moreNecrotic / 2 * modifer;

        critChanceCurrentValue += moreCrit * modifer / 2;

        if (sideEffectCurrentValue <= -lessNecrotic)
            sideEffectCurrentValue -= lessNecrotic;
        else if (sideEffectCurrentValue < 0 && sideEffectCurrentValue >= lessNecrotic)
            sideEffectCurrentValue = 0;
        CheckValuesAreWithinRange();
    }

    private void LessRegenerativeMoreCritChance(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        int lessRegen = moreRegenerative / 2 * modifer;

        critChanceCurrentValue += moreCrit * modifer / 2;

        if (sideEffectCurrentValue >= lessRegen)
            sideEffectCurrentValue -= lessRegen;
        else if (sideEffectCurrentValue > 0 && sideEffectCurrentValue <= lessRegen)
            sideEffectCurrentValue = 0;
        CheckValuesAreWithinRange();

    }

    private void LessCritMoreNecrotic(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        sideEffectCurrentValue -= moreNecrotic / 2 * modifer;
        critChanceCurrentValue -= moreCrit * modifer / 2;
        if (critChanceCurrentValue < 0)
            critChanceCurrentValue = 0;
        CheckValuesAreWithinRange();
    }

    private void LessCritMoreRegenerative(FloatingObjectEnumQuality quality)
    {
        int modifer = GetQualityModifer(quality);
        sideEffectCurrentValue += moreRegenerative / 2 * modifer;
        critChanceCurrentValue -= moreCrit * modifer / 2;
        if (critChanceCurrentValue < 0)
            critChanceCurrentValue = 0;
        CheckValuesAreWithinRange();
    }
    #endregion

    // this method will check that if the sliders didnt pass the max values.
    private void CheckValuesAreWithinRange()
    {
        // to check the side effect current value
        if (sideEffectCurrentValue > sideEffectMeterRange.y)
            sideEffectCurrentValue = (int)sideEffectMeterRange.y;
        else if (sideEffectCurrentValue < sideEffectMeterRange.x)
            sideEffectCurrentValue = (int)sideEffectMeterRange.x;
        // to check the crit current value
        if(critChanceCurrentValue > critMaxChance)
            critChanceCurrentValue = critMaxChance;
        else if(critChanceCurrentValue < 0)
            critChanceCurrentValue = 0;

    }


    public void ResetBackToStart()
    {
        post.SetMiniGameStatus(false);
        pregamePotion = null;
        foreach (var floatingobj in activeFloatingObjectsList)
        {
            Destroy(floatingobj);
        }
        activeFloatingObjectsList.Clear();
        currentMinigameRound = 0;
        critChanceCurrentValue = 0;
        sideEffectCurrentValue = 0;

        alchemyCanvas.SetActive(true);
        gameObject.SetActive(false);
    }



}