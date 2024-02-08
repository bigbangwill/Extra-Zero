using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using System.Reflection;
using System.Linq;

public class WaveManager : SingletonComponent<WaveManager>
{
    #region Singleton
    public static WaveManager Instance
    {
        get { return (WaveManager)_Instance; }
        set { _Instance = value; }
    }
    #endregion

    [SerializeField] private List<WaveDifficultySO> waveDifficultyList = new();

    private List<PermanentWaveEffectLibrary.HarderSideEffects> hardEffectList = new();
    private List<PermanentWaveEffectLibrary.RewardSideEffects> rewardEffectList = new();

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

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        List<Type> childTypesList = Assembly.GetAssembly(typeof(PermanentWaveEffectLibrary.HarderSideEffects))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract 
        && TheType.IsSubclassOf(typeof(PermanentWaveEffectLibrary.HarderSideEffects))).ToList();
        foreach (Type childType in childTypesList)
        {
            PermanentWaveEffectLibrary.HarderSideEffects targetEffect = (PermanentWaveEffectLibrary.HarderSideEffects)Activator.CreateInstance(childType);
            hardEffectList.Add(targetEffect);
        }
        childTypesList.Clear();
        childTypesList = Assembly.GetAssembly(typeof(PermanentWaveEffectLibrary.RewardSideEffects))
        .GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract
        && TheType.IsSubclassOf(typeof(PermanentWaveEffectLibrary.RewardSideEffects))).ToList();
        foreach (Type childType in childTypesList)
        {
            PermanentWaveEffectLibrary.RewardSideEffects targetEffect = (PermanentWaveEffectLibrary.RewardSideEffects)Activator.CreateInstance(childType);
            rewardEffectList.Add(targetEffect);
        }

        Debug.Log(hardEffectList.Count);
        Debug.Log(rewardEffectList.Count);
    }


    public WaveDifficultySO GetNextWave()
    {

        if (selectedWave != null)
        {
            return selectedWave;
        }
        else
        {
            return GetRandomNextWave();
        }
    }

    private WaveDifficultySO GetRandomNextWave()
    {
        WaveDifficultySO targetWave = waveDifficultyList[UnityEngine.Random.Range(0, waveDifficultyList.Count)];
        return targetWave;
    }

    public abstract class PermanentWaveEffectLibrary
    {
        private Sprite iconEffect;
        private string iconAddress;
        private string description;
        private string specificName;

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

        public abstract void ImpactEffect();


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
                }

                public override void ImpactEffect()
                {
                    Instance.timerOfOneWaveEffectsApplied += 2f;
                    Debug.Log(GetEffectDescription() + "  2");
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
                }

                public override void ImpactEffect()
                {
                    Instance.timerOfAFullCycleEffectsApplied += 2f;
                    Debug.Log(GetEffectDescription() + "  2");
                }
            }

            public class IncreasedSummonFrequency : HarderSideEffects
            {
                public IncreasedSummonFrequency()
                {
                    specificName = "Pulse";
                    description = "Increased Night Time";
                    SetIconAddress();
                    LoadIcon();
                }

                public override void ImpactEffect()
                {
                    Instance.orderFrequencyEffectsApplied -= 0.5f;
                    Debug.Log(GetEffectDescription() + "  0.5f");
                }
            }

            public class IncreaseOrderCombinationCount : HarderSideEffects
            {
                public IncreaseOrderCombinationCount()
                {
                    specificName = "Rotation";
                    description = "Increased Order Combination Count";
                    SetIconAddress();
                    LoadIcon();
                }

                public override void ImpactEffect()
                {
                    Instance.orderCombinationEffectsApplied += 1;
                    Debug.Log(GetEffectDescription() + "  1");
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
                }

                public override void ImpactEffect()
                {
                    PlayerInventory.Instance.HaveEmptySlot(new MaterialItem.TitaniumAlloy(10), true);
                    Debug.Log(GetEffectDescription() + "  10");
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
                }

                public override void ImpactEffect()
                {
                    PlayerInventory.Instance.HaveEmptySlot(new MaterialItem.Plastic(10), true);
                    Debug.Log(GetEffectDescription() + "  10");
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
                }

                public override void ImpactEffect()
                {
                    PlayerInventory.Instance.HaveEmptySlot(new MaterialItem.AluminumAlloy(10), true);
                    Debug.Log(GetEffectDescription() + "  10");
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
                }

                public override void ImpactEffect()
                {
                    PlayerInventory.Instance.HaveEmptySlot(new MaterialItem.Ceramic(10), true);
                    Debug.Log(GetEffectDescription() + "  10");
                }
            }
        }
    }
}