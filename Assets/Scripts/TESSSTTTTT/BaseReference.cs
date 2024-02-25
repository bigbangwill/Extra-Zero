using UnityEngine;
using System;

[System.Serializable]
//[CreateAssetMenu(fileName = "New Reference", menuName = "References/Reference")]
public class BaseReference<T> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] protected T _initial = default(T);

    public event Action<T> OnGet = null;
    public event Action<T> OnSet = null;
    protected T _val;

    public T initial => _initial;

    public T val
    {
        get
        {
            OnGet?.Invoke(_val);
            // Debug.Log($"{name} getting {_val?.ToString()}");
            return _val;
        }
        set
        {
            // Debug.Log($"setting {name} to {value?.ToString()} delegate {OnSet != null}");
            _val = value;
            OnSet?.Invoke(_val);
        }
    }

    public void InvokeOnSet()
    {
        OnSet?.Invoke(_val);
    }

    public void InvokeOnGet()
    {
        OnGet?.Invoke(_val);
    }

    public void SetInitial()
    {
        // Debug.Log($"{name} setting initial");
        _val = _initial;
    }

    public void Set(T newValue)
    {
        // Debug.Log($"{name} setting to {newValue?.ToString()}");
        _val = newValue;
        OnSet?.Invoke(_val);
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        // Debug.Log($"calling deserialize");
        OnGet = null;
        OnSet = null;
        _val = _initial;
    }

    public bool HasSetDelegate => OnSet != null;
}