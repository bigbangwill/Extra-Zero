using UnityEngine;

[CreateAssetMenu(fileName = "Player Inventory Refrence", menuName = "References/Player Inventory")]
public class PlayerInventoryRefrence : BaseReference<PlayerInventory>
{

    [ContextMenu("Check Current Value")]
    void DebugCheckCurrentValue()
    {
        if (_val == null)
        {
            Debug.Log($"Current value is null");
            return;
        }
        Debug.Log($"Current value is {_val.gameObject.name}", _val.gameObject);
    }
}