using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTest : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Here" + collision.gameObject.name);
    }

}