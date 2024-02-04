using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRepairable
{
    public bool Repair();
    public bool NeedsRepair();
}