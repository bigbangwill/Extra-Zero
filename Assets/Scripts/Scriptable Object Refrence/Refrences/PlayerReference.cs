using UnityEngine;

[CreateAssetMenu(fileName = "New Player Reference", menuName = "References/Player")]
public class PlayerReference : BaseReference<Player>
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