using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(OrderPostHealth))]
public class OrderPostHealthImageSetter : MonoBehaviour
{
    [SerializeField] private Sprite destroyedHealthImage;
    [SerializeField] private Sprite oneHealthImage;
    [SerializeField] private Sprite twoHealthImage;
    [SerializeField] private Sprite threeHealthImage;
    [SerializeField] private Sprite fourHealthImage;
    [SerializeField] private Sprite fiveHealthImage;
    [SerializeField] private Sprite fullHealthImage;

    public Sprite SetHealthImage(int currentHealth)
    {
        switch (currentHealth)
        {
            case 0: return destroyedHealthImage;
            case 1: return oneHealthImage;
            case 2: return twoHealthImage;
            case 3: return threeHealthImage;
            case 4: return fourHealthImage;
            case 5: return fiveHealthImage;
            case 6: return fullHealthImage;
            default: return null;
        }
    }

}
