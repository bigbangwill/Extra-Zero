using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To set their state so the manager will know what to do with it.
public enum FloatingObjectEnumState
{
    MoreNecrotic, MoreRegenerative, MoreCritChance, LessNecroticMoreCritChance, LessRegenerativeMoreCritChance,
    LessCritMoreNecrotic, LessCritMoreRegenerative
}
// To set their quality so the manager will know how to calculate their true value.
public enum FloatingObjectEnumQuality{ Lesser, Medium, Powerfull }


public class AlchemyFloatingObject : MonoBehaviour
{

    [SerializeField] private FloatingObjectEnumState floatingObjectState;
    private FloatingObjectEnumQuality floatingObjectQuality;

    // Will get set in the editor
    [SerializeField] private Color lesserColor;
    [SerializeField] private Color mediumColor;
    [SerializeField] private Color powerfullColor;

    /// <summary>
    /// Should get called on instantiation.
    /// </summary>
    /// <param name="qualityEnum"></param>
    public void SetState(FloatingObjectEnumQuality qualityEnum)
    {
        SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
        floatingObjectQuality = qualityEnum;
        switch (floatingObjectQuality)
        {
            case (FloatingObjectEnumQuality)0:
                renderer.color = lesserColor;
                break;
            case (FloatingObjectEnumQuality)1:
                renderer.color = mediumColor;
                break;
            case (FloatingObjectEnumQuality)2:
                renderer.color = powerfullColor;
                break;
            default:
                Debug.LogWarning("Checkhere");
                break;
        }
    }

    /// <summary>
    /// This is for the call to get the info that the mini game manager needs. needs GetState as well.
    /// </summary>
    /// <returns></returns>
    public FloatingObjectEnumQuality GetQuality()
    {
        return floatingObjectQuality;
    }


    /// <summary>
    /// This is for the call to get the info that the mini game manager needs.
    /// </summary>
    /// <returns></returns>
    public FloatingObjectEnumState GetState()
    {
        return floatingObjectState;
    }



}