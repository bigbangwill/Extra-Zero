using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronMovement : MonoBehaviour
{
    public Transform center; // Reference to the center/core
    public float rotationSpeed = 50f; // Speed of rotation
    public bool minus;

    void Update()
    {
        // Ensure there's a center to rotate around
        if (center != null)
        {
            // Calculate the direction from the electron to the center
            Vector3 directionToCenter = center.position - transform.position;

            // Calculate the rotation step based on speed and time
            float rotationStep = rotationSpeed * Time.deltaTime;

            int indicate = 1;
            // Rotate the electron around the center
            if (minus)
                indicate = -1;
            transform.RotateAround(center.position,indicate * ( Vector3.forward ), rotationStep);
        }
        else
        {
            Debug.LogError("Center/Core is not assigned!");
        }
    }
}
