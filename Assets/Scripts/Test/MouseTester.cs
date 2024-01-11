using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseTester : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 startPos;
    private Mouse mouse;

    [SerializeField] private float minDistance;


    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1;
        startPos = transform.position;
        mouse = Mouse.current;
    }


    private void Update()
    {

        if (mouse.leftButton.wasPressedThisFrame)
        {
            lr.positionCount = 1;
            startPos = transform.position;
        }

        if (mouse.leftButton.isPressed)
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0;
            if (Vector3.Distance(currentPos, startPos) >= minDistance)
            {
                if (startPos == transform.position)
                {
                    lr.SetPosition(0, currentPos);
                }
                else
                {
                    lr.positionCount++;
                    lr.SetPosition(lr.positionCount - 1, currentPos);
                }
                startPos = currentPos;
            }
        }


        if (mouse.leftButton.wasReleasedThisFrame)
        {
            Debug.Log("here");
            lr.positionCount++;
            lr.SetPosition(lr.positionCount - 1, lr.GetPosition(0));
            BakeMesh();
        }
    }


    private void BakeMesh()
    {
        Mesh lineBakedMesh = new();
        Vector3[] vecArray = new Vector3[lr.positionCount];
        Vector3[] vecArrayWithCenter = new Vector3[lr.positionCount + 1];


        lr.GetPositions(vecArray);
        Vector2[] array2 = new Vector2[vecArray.Length];
        for (int i = 0; i < vecArray.Length; i++)
        {
            array2[i].x = vecArray[i].x;
            array2[i].y = vecArray[i].y;
        }

        PolygonCollider2D polyCollider = GetComponent<PolygonCollider2D>();
        polyCollider.SetPath(0,array2);
        List<Collider2D> collider2Ds = new();
        Debug.Log(polyCollider.Overlap(collider2Ds));

        foreach(var i in collider2Ds)
        {
            Debug.Log(i.gameObject.name);
        }
    }


    private Vector3 FindCenter(Vector3[] vectors)
    {

        Vector3 center = new();


        float sumX = 0;
        float sumY = 0;
        foreach (var vec in vectors)
        {
            sumX += vec.x;
            sumY += vec.y;
        }
        center.x = sumX / vectors.Length;
        center.y = sumY / vectors.Length;
        center.z = 0;

        return center;


    }

}