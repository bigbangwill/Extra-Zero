using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;

public class TESTREMOVELATER : MonoBehaviour
{

    public List<GameObject> nodes = new();

    public float zoomSpeed = 0.1f; // Adjust the speed of zooming
    public float minScale = 0.5f;  // Minimum scale limit
    public float maxScale = 3.0f;  // Maximum scale limit

    private Vector2 initialTouchPosition0; // Initial position of the first touch
    private Vector2 initialTouchPosition1; // Initial position of the second touch
    private Vector3 initialScale;



#if UNITY_EDITOR
    [ContextMenu("Rename GameObjects")]
    void RenameGameObjects()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].name = i.ToString();
            nodes[i].GetComponent<CampaignNodeScript>().SetNodeName(nodes[i].name);
        }
    }
#endif

    private void Update()
    {
        // Check if there are two touches on the screen
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            // Check if either of the touches began this frame
            if (touch0.phase == TouchPhase.Began || touch1.phase == TouchPhase.Began)
            {
                // Record the initial touch positions and the object's initial scale
                initialTouchPosition0 = touch0.position;
                initialTouchPosition1 = touch1.position;
                initialScale = transform.localScale;
            }
            else if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                // Calculate the distance between the touches at the start and now
                float initialDistance = Vector2.Distance(initialTouchPosition0, initialTouchPosition1);
                float currentDistance = Vector2.Distance(touch0.position, touch1.position);

                // Calculate the scale factor based on the change in distance
                float scaleFactor = currentDistance / initialDistance;

                // Apply the scale factor to the object's initial scale
                Vector3 newScale = initialScale * scaleFactor;

                // Clamp the new scale to the specified min and max values
                newScale = new Vector3(
                    Mathf.Clamp(newScale.x, minScale, maxScale),
                    Mathf.Clamp(newScale.y, minScale, maxScale),
                    Mathf.Clamp(newScale.z, minScale, maxScale)
                );

                // Set the object's scale to the new scale
                transform.localScale = newScale;
            }
        }
    }


}