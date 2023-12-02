using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class AlchemyPost : MonoBehaviour
{
    [SerializeField] private AlchemyHerbsPopUp popUp;

    private List<Herb> inventoryHerbs;
    [SerializeField] private List<string> herbNames;
    private List<Herb> sendingHerbs;
    

    private void Start()
    {
        PotionLibrary.Initialize();
        PlayerInventory.Instance.HaveEmptySlot(new Herb.Sage(50), true);
        PlayerInventory.Instance.HaveEmptySlot(new Herb.Chamomile(50), true);
        PlayerInventory.Instance.HaveEmptySlot(new Herb.Lavender(50), true);
        PlayerInventory.Instance.HaveEmptySlot(new Herb.Lavender(50), true);
        PlayerInventory.Instance.HaveEmptySlot(new Herb.Sage(50), true);
        PlayerInventory.Instance.HaveEmptySlot(new Herb.Sage(50), true);
        SetLists();
    }

    private void OnEnable()
    {
        SetLists();
    }

    
    
    // To reset the needed lists to clear and refill them from the very begining.
    private void SetLists()
    {
        inventoryHerbs = new();
        herbNames = new();
        sendingHerbs = new();
        List<Herb> herbs = PlayerInventory.Instance.SearchInventoryOfItemBehaviour<Herb>(ItemType.herb);
        foreach (Herb herb in herbs)
        {
            Type type = herb.GetType();
            inventoryHerbs.Add((Herb)Activator.CreateInstance(type,new object[]{ herb.CurrentStack()}));
        }

       

        SetSendingList();
    }

    // to reset the sendingHerbs and herbNames list to refresh the ui after the OnPointerUp event.
    private void SetSendingList()
    {
        herbNames.Clear();
        sendingHerbs.Clear();
        foreach (var herb in inventoryHerbs)
        {
            if (herb.CurrentStack() >= 5)
            {
                if (!herbNames.Contains(herb.GetName()))
                {
                    herbNames.Add(herb.GetName());
                    sendingHerbs.Add(herb);
                }
            }
        }
    }

    /// <summary>
    /// Method to start the activation of the herb selection UI.
    /// </summary>
    public void SlotsOnPointerDown()
    {
        Vector3 pos = Input.mousePosition;
        popUp.gameObject.SetActive(true);
        popUp.GetComponent<RectTransform>().position = pos;
        popUp.SetHerb(sendingHerbs);
    }

    /// <summary>
    /// Method to set the herb selection UI back to deactive.
    /// </summary>
    public void SlotsOnPointerUp()
    {
        popUp.gameObject.SetActive(false);
        SetSendingList();
    }

}