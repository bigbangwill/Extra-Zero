using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStashable
{

    public void InitUI();
    public bool HaveEmptySlot(ItemBehaviour item, bool shouldAdd);
    public void RemoveItemFromInventory(int i);
    public bool HaveItemInInventory(ItemBehaviour item, bool shouldRemove);
    public void SwapItemInInventory(int a, int b);
    public List<T> SearchInventoryOfItemBehaviour<T>(ItemType _ItemType);
    public ItemBehaviour ItemRefrence(int slot);



}
