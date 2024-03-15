using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using System.Reflection;
using System.Linq;
using Unity.VisualScripting;

public class WaveManager : MonoBehaviour
{
    //#region Singleton
    //public static WaveManager Instance
    //{
    //    get { return (WaveManager)_Instance; }
    //    set { _Instance = value; }
    //}
    //#endregion

    [SerializeField] private int totalWaveOptionCount;
    
    [SerializeField] private List<WaveDifficultySO> waveDifficultyList = new();

    [SerializeField] private WaveChosingUI chosingUI;

    private List<HarderSideEffects> hardEffectList = new();
    private List<RewardSideEffects> rewardEffectList = new();

    #region Bad Side Multiplier
    protected int orderCombinationEffectsApplied;
    protected int combinationRandomnessEffectsApplied;
    protected float orderFrequencyEffectsApplied;
    protected float frequencyRandomnessEffectsApplied;
    protected float timerOfOneWaveEffectsApplied;
    protected float timerOfOneWaveRandomnessEffectsApplied;
    protected float timerOfAFullCycleEffectsApplied;
    protected float walkingOrderSpeedEffectsApplied;
    protected float orderFulfillTimerEffectsApplied;
    #endregion

    #region Good Side Multiplier

    #endregion

    private WaveDifficultySO selectedWave;


    protected WaveManagerRefrence refrence;
    private OrderManagerRefrence orderManagerRefrence;

    private void LoadSORefrence()
    {
        orderManagerRefrence = (OrderManagerRefrence)FindSORefrence<OrderManager>.FindScriptableObject("Order Manager Refrence");
    }

    private void SetRefrence()
    {
        refrence = (WaveManagerRefrence)FindSORefrence<WaveManager>.FindScriptableObject("Wave Manager Refrence");
        if (refrence == null)
        {
            Debug.LogWarning("Didnt find it");
            return;
        }
        refrence.val = this;
    }

    private void Awake()
    {
        SetRefrence();
    }

    private void Start()
    {
        LoadSORefrence();
        Init();
    }

    public void UpgradeOrbitHarderLessEffective(bool isQubit)
    {
        foreach (var wave in hardEffectList)
        {
            wave.SetWaveState(isQubit);
        }
    }

    public void UpgradeOrbitMoreReward(bool isQubit)
    {
        foreach (var wave in rewardEffectList)
        {
            wave.SetWaveState(isQubit);
        }
    }


    // To create every existed effects and add them to related list
    private void Init()
    {
        List<Type> childTypesList = Assembly.GetAssembly(typeof(HarderSideEffects))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract 
        && TheType.IsSubclassOf(typeof(HarderSideEffects))).ToList();
        foreach (Type childType in childTypesList)
        {
            HarderSideEffects targetEffect = (HarderSideEffects)Activator.CreateInstance(childType);
            hardEffectList.Add(targetEffect);
        }
        childTypesList.Clear();
        childTypesList = Assembly.GetAssembly(typeof(RewardSideEffects))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract
        && TheType.IsSubclassOf(typeof(RewardSideEffects))).ToList();
        foreach (Type childType in childTypesList)
        {
            RewardSideEffects targetEffect = (RewardSideEffects)Activator.CreateInstance(childType);
            rewardEffectList.Add(targetEffect);
        }
    }

    /// <summary>
    /// To get a random harder effect out of the list.
    /// </summary>
    /// <returns></returns>
    public HarderSideEffects GetRandomHarderEffect()
    {
        int random = UnityEngine.Random.Range(0, hardEffectList.Count);
        return hardEffectList[random];
    }

    /// <summary>
    /// To get a random reward effect out of the list
    /// </summary>
    /// <returns></returns>
    public RewardSideEffects GetRandomRewardEffect()
    {
        int random = UnityEngine.Random.Range(0, rewardEffectList.Count);
        return rewardEffectList[random];
    }

    /// <summary>
    /// Called when no wave option is chosen and a random one gets chosen here.
    /// </summary>
    public void ExecuteRandomEffectsAndWave()
    {
        WaveDifficultySO targetWave = waveDifficultyList[UnityEngine.Random.Range(0, waveDifficultyList.Count)];
        GetRandomHarderEffect().ImpactEffect();
        GetRandomRewardEffect().ImpactEffect();
        orderManagerRefrence.val.StartNewWave(ApplyCurrentEffectsToTheWave(targetWave));
    }

    /// <summary>
    /// To get random wave from the list that is inside wave manager script
    /// </summary>
    /// <returns></returns>
    public WaveDifficultySO GetRandomNextWave()
    {
        WaveDifficultySO targetWave = waveDifficultyList[UnityEngine.Random.Range(0, waveDifficultyList.Count)];
        return targetWave;
    }

    /// <summary>
    /// The return of this method is ready to be fed in to the order manager as the tweaked wave
    /// </summary>
    /// <param name="targetedWave"></param>
    /// <returns></returns>
    public WaveDifficultySO ApplyCurrentEffectsToTheWave(WaveDifficultySO targetedWave)
    {
        WaveDifficultySO tweakedWave = Instantiate(targetedWave);
        tweakedWave.orderCombination = targetedWave.orderCombination + orderCombinationEffectsApplied;
        tweakedWave.orderFrequency = targetedWave.orderFrequency + orderFrequencyEffectsApplied;
        tweakedWave.timerOfOneWave = targetedWave.timerOfOneWave + timerOfOneWaveEffectsApplied;
        tweakedWave.timerOfNightTime = targetedWave.timerOfNightTime + timerOfAFullCycleEffectsApplied;
        tweakedWave.walkingOrderSpeed = targetedWave.walkingOrderSpeed + walkingOrderSpeedEffectsApplied;
        tweakedWave.orderFulfillTimer = targetedWave.orderFulfillTimer + orderFulfillTimerEffectsApplied;
        return tweakedWave;
    }

    /// <summary>
    /// To get the number of total waveoptions the choosing canvas should instantiate.
    /// </summary>
    /// <returns></returns>
    public int GetTotalWaveOptionCount()
    {
        return totalWaveOptionCount;
    }


    public abstract class PermanentWaveEffectLibrary
    {

        protected enum UpgradeEnum { isPassive, isNotQubit, isQubit}

        protected UpgradeEnum currentUpgradeState = UpgradeEnum.isPassive;

        protected Sprite iconEffect;
        protected string iconAddress;
        protected string description;
        protected string specificName;


        protected PlayerInventoryRefrence inventoryRefrence;
        protected WaveManagerRefrence waveManagerRefrence;
        protected void LoadSORefrence()
        {
            inventoryRefrence = (PlayerInventoryRefrence)FindSORefrence<PlayerInventory>.FindScriptableObject("Player Inventory Refrence");
            waveManagerRefrence = (WaveManagerRefrence)FindSORefrence<WaveManager>.FindScriptableObject("Wave Manager Refrence");
        }

        protected void LoadIcon()
        {
            AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(iconAddress);
            handle.WaitForCompletion(); // Wait for the async operation to complete synchronously

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                iconEffect = handle.Result;
                Addressables.Release(handle);
            }
            else
            {
                Debug.LogError("Failed to load the asset" + "   " + iconAddress);
            }
        }

        public Sprite IconRefrence()
        {
            return iconEffect;
        }

        public string GetEffectDescription()
        {
            return description;
        }

        public void SetWaveState(bool isQubit)
        {
            if (!isQubit)
            {
                currentUpgradeState = UpgradeEnum.isNotQubit;
            }
            else
            {
                currentUpgradeState = UpgradeEnum.isQubit;
            }
        }

        public abstract void ImpactEffect();

        public void UpgradeOrbit(bool isQubit)
        {
            if (!isQubit)
            {
                currentUpgradeState = UpgradeEnum.isNotQubit;
            }
            else
            {
                currentUpgradeState = UpgradeEnum.isQubit;
            }
        }


    }
    public abstract class HarderSideEffects : PermanentWaveEffectLibrary
    {
        protected void SetIconAddress()
        {
            iconAddress = $"Wave Effects/BadEffectsIconSheet[{specificName}]";
        }

        public class IncresedDayTime : HarderSideEffects
        {
            public IncresedDayTime()
            {
                specificName = "Glow";
                description = "Increased Day Time";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {
                if (currentUpgradeState == UpgradeEnum.isPassive)
                    waveManagerRefrence.val.timerOfOneWaveEffectsApplied += 2f;
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    waveManagerRefrence.val.timerOfOneWaveEffectsApplied += 1.9f;
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    waveManagerRefrence.val.timerOfOneWaveEffectsApplied += 1.8f;
            }
        }

        public class IncreasedNightTime : HarderSideEffects
        {
            public IncreasedNightTime()
            {
                specificName = "Hidle";
                description = "Increased Night Time";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {                
                if (currentUpgradeState == UpgradeEnum.isPassive)
                    waveManagerRefrence.val.timerOfAFullCycleEffectsApplied += 2f;
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    waveManagerRefrence.val.timerOfOneWaveEffectsApplied += 1.9f;
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    waveManagerRefrence.val.timerOfOneWaveEffectsApplied += 1.8f;
            }
        }

        public class IncreasedSummonFrequency : HarderSideEffects
        {
            public IncreasedSummonFrequency()
            {
                specificName = "Pulse";
                description = "Increased Summon Frequency";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {
                if (currentUpgradeState == UpgradeEnum.isPassive)
                    waveManagerRefrence.val.orderFrequencyEffectsApplied -= 0.5f;
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    waveManagerRefrence.val.orderFrequencyEffectsApplied -= 0.4f;
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    waveManagerRefrence.val.orderFrequencyEffectsApplied -= 0.3f;

            }
        }

        /// <summary>
        /// FIND A FIX FOR THIS PART
        /// </summary>
        public class IncreaseOrderCombinationCount : HarderSideEffects
        {
            public IncreaseOrderCombinationCount()
            {
                specificName = "Rotation";
                description = "Increased Order Combination Count";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }


            public override void ImpactEffect()
            {
                if (currentUpgradeState == UpgradeEnum.isPassive)
                    waveManagerRefrence.val.orderCombinationEffectsApplied += 1;
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    waveManagerRefrence.val.orderCombinationEffectsApplied += 1;
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    waveManagerRefrence.val.orderCombinationEffectsApplied += 1;
            }
        }
    }

    public abstract class RewardSideEffects : PermanentWaveEffectLibrary
    {
        protected void SetIconAddress()
        {
            iconAddress = $"Wave Effects/GoodEffectsIconSheet[{specificName}]";
        }
        public class GainCurrencyOfTitanium : RewardSideEffects
        {
            public GainCurrencyOfTitanium()
            {
                specificName = "Glow";
                description = "Gained Titanium";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {
                
                

                if (currentUpgradeState == UpgradeEnum.isPassive)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.TitaniumAlloy(3), true);
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.TitaniumAlloy(5), true);
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.TitaniumAlloy(10), true);

            }
        }

        public class GainCurrencyOfPlastic : RewardSideEffects
        {
            public GainCurrencyOfPlastic()
            {
                specificName = "Glow2";
                description = "Gained Plastic";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {


                if (currentUpgradeState == UpgradeEnum.isPassive)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Plastic(3), true);
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Plastic(5), true);
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Plastic(10), true);

            }
        }

        public class GainCurrencyOfAluminumAlloy : RewardSideEffects
        {
            public GainCurrencyOfAluminumAlloy()
            {
                specificName = "Motlion";
                description = "Gained Almuminum";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {

                if (currentUpgradeState == UpgradeEnum.isPassive)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.AluminumAlloy(3), true);
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.AluminumAlloy(5), true);
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.AluminumAlloy(10), true);
            }
        }

        public class GainCurrencyOfCeramic : RewardSideEffects
        {
            public GainCurrencyOfCeramic()
            {
                specificName = "Rotation";
                description = "Gained Ceramic";
                SetIconAddress();
                LoadIcon();
                LoadSORefrence();
            }

            public override void ImpactEffect()
            {
                
                if (currentUpgradeState == UpgradeEnum.isPassive)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Ceramic(3), true);
                else if (currentUpgradeState == UpgradeEnum.isNotQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Ceramic(5), true);
                else if (currentUpgradeState == UpgradeEnum.isQubit)
                    inventoryRefrence.val.HaveEmptySlot(new MaterialItem.Ceramic(10), true);

            }
        }
    }
}