using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyHerbsPopUp : MonoBehaviour
{
    [SerializeField] private List<PopUpSlot> childImagesList = new();

    private void Start()
    {
        foreach (Transform t in transform)
        {
            childImagesList.Add(t.GetComponent<PopUpSlot>());
        }
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Method to set active the needed slots of the prefab and give them their needed image.
    /// </summary>
    /// <param name="herbList"></param>
    public void SetHerb(List<Herb> herbList)
    {
        for (int i = 1; i < childImagesList.Count; i++)
        {
            if (i - 1 < herbList.Count)
            {
                childImagesList[i].gameObject.SetActive(true);
                childImagesList[i].SetHerb(herbList[i - 1]);
            }
            else
            {
                childImagesList[i].gameObject.SetActive(false);
            }
        }
    }
}