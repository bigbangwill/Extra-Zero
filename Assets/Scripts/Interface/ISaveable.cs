using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    public void AddISaveableToDictionary();

    public object Save();
    public void Load(object savedData);
    public string GetName();
}