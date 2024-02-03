using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepairable
{
    public void Repair();
    public bool NeedsRepair();
}