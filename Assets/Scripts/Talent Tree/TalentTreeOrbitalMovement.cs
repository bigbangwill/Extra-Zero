using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalentTreeOrbitalMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> circleList = new List<Transform>();
    [SerializeField] private float rotateSpeedX;
    [SerializeField] private float rotateSpeedY;
    [SerializeField] private float rotateSpeedZ;

    private void Update()
    {

        foreach (Transform circles in circleList)
        {
            circles.Rotate(Vector3.forward,rotateSpeedZ * Time.deltaTime);
        }
    }


}
