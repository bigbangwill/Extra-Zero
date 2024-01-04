using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class MouseTester : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 startPos;

    [SerializeField] private float minDistance;

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 1;
        startPos = transform.position;
    }


    private void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            lr.positionCount = 1;
            startPos = transform.position;
        }


        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentPos.z = 0;
            if(Vector3.Distance(currentPos, startPos) >= minDistance)
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


        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("here");
            lr.positionCount++;
            lr.SetPosition(lr.positionCount -1, lr.GetPosition(0));
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


        #region CherkList
        //int[] triangles = new int[(vecArray.Length - 1) * 3];
        //Vector3 center = FindCenter(vecArray);

        //for (int i = 0; i < vecArray.Length; i++)
        //{
        //    vecArrayWithCenter[i] = vecArray[i];
        //}
        //vecArrayWithCenter[vecArrayWithCenter.Length - 1] = center;

        //if (GetComponent<MeshFilter>() != null)
        //{
        //    Destroy(GetComponent<MeshFilter>());
        //}

        //if (GetComponent<MeshCollider>() != null)
        //{
        //    Destroy(GetComponent<MeshCollider>());
        //}
        //transform.AddComponent<MeshFilter>();
        //GetComponent<MeshFilter>().mesh = lineBakedMesh;

        //lineBakedMesh.vertices = vecArrayWithCenter;

        //for (int i = 0; i < vecArray.Length - 2; i++)
        //{
        //    triangles[i * 3] = i;
        //    triangles[i * 3 + 1] = i + 1;
        //    triangles[i * 3 + 2] = vecArrayWithCenter.Length - 1;
        //}


        ////triangles[(vecArray.Length - 2) * 3 - 3] = vecArrayWithCenter.Length - 2;
        ////triangles[(vecArray.Length - 2) * 3 - 2] = 0;
        ////triangles[(vecArray.Length - 2) * 3 - 1] = vecArrayWithCenter.Length - 1;

        //triangles[(vecArray.Length - 2) * 3] = vecArray.Length - 2;
        //triangles[(vecArray.Length - 2) * 3 + 1] = 0;
        //triangles[(vecArray.Length - 2) * 3 + 2] = vecArrayWithCenter.Length - 1;


        //lineBakedMesh.triangles = triangles;

        //lr.BakeMesh(lineBakedMesh, Camera.main, true);

        //transform.AddComponent<MeshCollider>().sharedMesh = lineBakedMesh;
        #endregion
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