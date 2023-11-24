using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinderDoneButton : MonoBehaviour
{
    public void DoneClicked()
    {
        transform.parent.gameObject.SetActive(false);
        EventManager.Instance.Resume();
    }
    
    
}