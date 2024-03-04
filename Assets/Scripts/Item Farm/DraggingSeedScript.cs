using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingSeedScript : MonoBehaviour
{

    private BiomeScript biomeScript;

    private void Start()
    {
        biomeScript = GetComponentInParent<BiomeScript>();
    }


    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Trigger");
        if (collision.TryGetComponent(out BonusPrefabScript bonus))
        {
            biomeScript.CollidedWithaBonus(bonus.GetAction());
            Destroy(collision.gameObject);
        }

    }


}