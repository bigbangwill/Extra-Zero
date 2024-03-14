using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class AlchemyLineMiniGame : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 startPos;
    private Mouse mouse;

    [SerializeField] private float minDistance;
    [SerializeField] private TextMeshProUGUI lineLenghtText;
    [SerializeField] private int lineSafeCount;

    private float lineMaxLenght;
    private float currentLineLenght = 0;

    [SerializeField] private bool canDraw = false;
    [SerializeField] private bool needsReset = false;

    private AlchemyMiniGame alchemyMiniGameScript;

    private void OnEnable()
    {
        lr = GetComponent<LineRenderer>();
        alchemyMiniGameScript = GetComponent<AlchemyMiniGame>();
        lr.positionCount = 1;
        startPos = transform.position;
        mouse = Mouse.current;
    }

    private void Update()
    {
        if (canDraw)
        {
            float textDistance = Vector3.Distance(lr.GetPosition(lr.positionCount - 1), lr.GetPosition(0));
            lineLenghtText.text = (currentLineLenght + textDistance).ToString();
            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                lr.positionCount = 1;
                startPos = transform.position;
                currentLineLenght = 0;
                Debug.Log("2");
            }

            if (Touchscreen.current.primaryTouch.press.isPressed && !needsReset)
            {
                Debug.Log("3");
                Vector3 currentPos = Camera.main.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
                currentPos.z = 0;
                if (Vector3.Distance(currentPos, startPos) >= minDistance)
                {
                    Debug.Log("4");
                    if (startPos == transform.position)
                    {
                        lr.SetPosition(0, currentPos);
                        Debug.Log("5");
                    }
                    else
                    {
                        lr.positionCount++;
                        lr.SetPosition(lr.positionCount - 1, currentPos);
                        Debug.Log("6");
                    }
                    float distance = Vector3.Distance(startPos, currentPos);
                    if (lr.positionCount == 1)
                        distance = 0;
                    currentLineLenght += distance;
                    startPos = currentPos;
                    Debug.Log("7");
                }
                if (currentLineLenght + textDistance > 10)
                {
                    needsReset = true;
                    lr.positionCount++;
                    lr.SetPosition(lr.positionCount - 1, lr.GetPosition(0));
                    float distance = Vector3.Distance(lr.GetPosition(lr.positionCount - 2), lr.GetPosition(lr.positionCount - 1));
                    currentLineLenght += distance;
                    //BakeMesh();
                    Debug.Log("8");
                }
            }

            if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            {
                if (lr.positionCount <= lineSafeCount)
                {
                    lr.positionCount = 1;
                    startPos = transform.position;
                    currentLineLenght = 0;
                    Debug.Log("9");
                    //return;
                }
                needsReset = false;
                lr.positionCount++;
                lr.SetPosition(lr.positionCount - 1, lr.GetPosition(0));
                float distance = Vector3.Distance(lr.GetPosition(lr.positionCount - 2), lr.GetPosition(lr.positionCount - 1));
                currentLineLenght += distance;
                Debug.Log("10");
                BakeMesh();
            }
        }

    }


    private void BakeMesh()
    {
        Vector3[] vecArray = new Vector3[lr.positionCount];

        lr.GetPositions(vecArray);
        Vector2[] array2 = new Vector2[vecArray.Length];
        for (int i = 0; i < vecArray.Length; i++)
        {
            array2[i].x = vecArray[i].x - transform.position.x;
            array2[i].y = vecArray[i].y - transform.position.y;

        }

        PolygonCollider2D polyCollider = GetComponent<PolygonCollider2D>();
        polyCollider.SetPath(0, array2);
        List<Collider2D> collider2Ds = new();
        List<GameObject> targetedFloatingObject = new();
        polyCollider.Overlap(collider2Ds);
        lineLenghtText.text = currentLineLenght.ToString();
        // To find all of the floating object that have their center in the created mesh.
        foreach (var go in collider2Ds)
        {
            if (go.CompareTag("Alchemy Floating Object"))
            {
                if (polyCollider.OverlapPoint(go.transform.position))
                {
                    targetedFloatingObject.Add(go.gameObject);
                }
            }
        }


        alchemyMiniGameScript.DrawRoundFinished(targetedFloatingObject);
        // To reset the line
        lr.positionCount = 1;
        startPos = transform.position;
        currentLineLenght = 0;

    }




    /// <summary>
    /// this method gets called from AlchemyMiniGame.cs to change it when it should draw or now.
    /// </summary>
    /// <param name="draw"></param>
    public void CanDraw(bool draw, float lineMaxLenght)
    {
        lr.positionCount = 1;
        startPos = transform.position;
        mouse = Mouse.current;
        currentLineLenght = 0;
        canDraw = draw;
        this.lineMaxLenght = lineMaxLenght;
    }


}

