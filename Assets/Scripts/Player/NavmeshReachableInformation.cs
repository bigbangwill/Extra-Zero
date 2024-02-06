using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NavmeshReachableInformation
{
    private Vector2 navmeshDistination;
    private Action callingMethod;


    public NavmeshReachableInformation(Vector2 navmeshDistination, Action callingMethod)
    {
        this.navmeshDistination = navmeshDistination;
        this.callingMethod = callingMethod;
    }

    public Vector2 GetDistination()
    {
        return navmeshDistination;
    }

    public Action GetCallingMethod()
    {        
        return callingMethod;
    }


}