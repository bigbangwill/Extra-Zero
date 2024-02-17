using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMovement : MonoBehaviour
{

    public bool shouldMove;
    public float speed;
    public float slowSpeed;
    public int val;

    private void Update()
    {
        if (shouldMove)
        {
            transform.Rotate(Vector3.forward * val, Time.deltaTime * speed);
        }
    }

    public void Move(int val)
    {
        shouldMove = true;
        this.val = val;
    }

    public void SlowDown()
    {
        speed = slowSpeed;
    }

}