using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSlot : MonoBehaviour
{
    private Herb containingHerb;
    [SerializeField] private Image herbImage;

    /// <summary>
    /// Method to save the Herb class and set it's image.
    /// </summary>
    /// <param name="herb"></param>
    public void SetHerb(Herb herb)
    {
        containingHerb = herb;
        herbImage.sprite = herb.IconRefrence();
    }

    /// <summary>
    /// Should get called OnPointerUp to return the selected herb.
    /// </summary>
    /// <returns></returns>
    public Herb HerbSelected()
    {
        return containingHerb;
    }

}