using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPauseable 
{
    public void Pause();
    public void Resume();

    public void OnEnable();
    public void OnDisable();
    public void OnDestroy();
    
    
}